using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public static class ShortcutExtensions
{
	/* Key mapping
	 * % - Ctrl / Cmd
	 * # - Shift
	 * & - Alt
	 * LEFT/RIGHT/UP/DOWN - Arrow keys
	 * F1...F2 - F keys
	 * HOME, END, PGUP, PGDN
	 */

	/* Rules:
	 * Create           <=>         ctrl + alt
	 * Edit             <=>         ctrl / ctrl + shift
	 * GUI              <=>         ctrl + alt
	 * Hierarchy        <=>         ctrl + shift
	 */

	#region Create

	[MenuItem("Shortcuts/Create/Empty child %&n", priority = 1)] // ctrl + alt + n
	public static void CreateEmptyChild()
	{
		GameObject[] selected = Selection.objects.Select(x => x as GameObject).ToArray();
		List<GameObject> created = new List<GameObject>();
		foreach(GameObject go in selected)
		{
			GameObject child = new GameObject("GameObject");
			Undo.RegisterCreatedObjectUndo(child, "Add child");
			child.transform.SetParent(go.transform);
			if(child.transform.parent.GetComponent<RectTransform>() != null)
				child.AddComponent<RectTransform>();
			child.transform.localPosition = Vector3.zero;
			child.transform.localRotation = Quaternion.identity;
			child.transform.localScale = Vector3.one;
			created.Add(child);
		}
		Selection.objects = created.ToArray();
	}

	[MenuItem("Shortcuts/Create/Empty child %&n", true)]
	private static bool CreateEmptyChildValidation()
	{
		return Selection.gameObjects.Length != 0;
	}

	[MenuItem("Shortcuts/Create/Animation in Controller", priority = 2)]
	private static void CreateAnimation()
	{
		UnityEditor.Animations.AnimatorController controller = Selection.objects[0] as UnityEditor.Animations.AnimatorController;
		var clipName = "Selected";
		var clip = UnityEditor.Animations.AnimatorController.AllocateAnimatorClip(clipName);
		AssetDatabase.AddObjectToAsset(clip, controller);
		var state = controller.AddMotion(clip);
	}

	[MenuItem("Shortcuts/Create/Animation in Controller", true)]
	private static bool CreateAnimationValidation()
	{
		return Selection.objects?[0] is UnityEditor.Animations.AnimatorController;
	}

	#endregion

	#region Edit

	[MenuItem("Shortcuts/Edit/Paste as child %#v", priority = 1)] // ctrl + shift + v
	private static void PasteAsChildren()
	{
		GameObject[] selected = Selection.gameObjects;
		List<GameObject> pasted = new List<GameObject>();
		foreach(GameObject go in selected)
		{
			GameObject child = new GameObject("GameObject");
			Undo.RegisterCreatedObjectUndo(child, "Paste as child");
			child.transform.SetParent(go.transform);
			Selection.activeGameObject = child;
			EditorApplication.ExecuteMenuItem("Edit/Paste");
			pasted.Add(Selection.activeGameObject);
			UnityEngine.Object.DestroyImmediate(child);
		}
		Selection.objects = pasted.ToArray();
	}

	[MenuItem("Shortcuts/Edit/Paste as child %#v", true)]
	private static bool PasteAsChildrenValidation()
	{
		return Selection.gameObjects.Length != 0;
	}

	[MenuItem("Shortcuts/Edit/Toggle activeSelf %&a", priority = 2)] // ctrl + alt + a // ctrl + shift + a doesn't work in 2018.4.2
	private static void ChangeObjectActiveState()
	{
		Undo.RecordObjects(Selection.objects, "Toggle activeSelf");
		foreach(var v in Selection.gameObjects)
		{
			v.SetActive(!v.activeSelf);
		}
	}

	[MenuItem("Shortcuts/Edit/Toggle activeSelf %&a", true)]
	private static bool ChangeObjectActiveStateValidation()
	{
		return Selection.gameObjects.Length != 0;
	}

	private struct ObjectNameData
	{
		public GameObject gameObject;
		public string baseName;
		public List<string> sentenceParts;
		public string[] numericalFormat;
		public int number;

		public ObjectNameData(GameObject gameObject)
		{
			this.gameObject = gameObject;
			baseName = gameObject.name;
			sentenceParts = new List<string>();
			numericalFormat = new string[] { string.Empty, string.Empty };
			number = 0;
		}

		public void UpdateName(bool withNumber)
		{
			string newName = string.Join(" ", sentenceParts.ToArray());
			if(withNumber)
				newName += " " + numericalFormat[0] + number + numericalFormat[1];
			gameObject.name = newName;
		}
	}

	private static bool IsDuplicating;

	[MenuItem("Shortcuts/Edit/Duplicate object %#d", priority = 3)]
	private static void DuplicateObjectWithProperNaming()
	{
		IsDuplicating = true;
		FormatObjectWithProperNaming();
		int childIndex = Selection.activeTransform.GetSiblingIndex();
		EditorApplication.ExecuteMenuItem("Edit/Duplicate");
		var name = Selection.activeGameObject.name;
		var split = name.Split(' ').ToList();
		split.RemoveAt(split.Count - 1);
		Selection.activeGameObject.name = string.Join(" ", split.ToArray());
		Selection.activeTransform.SetSiblingIndex(childIndex + 1);
		FormatObjectWithProperNaming();
		IsDuplicating = false;
	}

	[MenuItem("Shortcuts/Edit/Duplicate object %#d", true)]
	private static bool DuplicateObjectWithProperNamingValidation()
	{
		return Selection.activeGameObject != null && Selection.gameObjects.Length == 1;
	}

	[MenuItem("Shortcuts/Edit/Format name %#r", priority = 4)]
	private static void FormatObjectWithProperNaming()
	{
		Undo.RecordObject(Selection.activeGameObject, "Change name");
		var data = GetObjectData(Selection.activeGameObject);

		Transform current = Selection.activeTransform;
		Transform parent = current.parent;
		List<int> existingIndexes = new List<int>();
		List<ObjectNameData> childrenData = new List<ObjectNameData>();

		foreach(Transform child in parent)
		{
			if(child == current) continue;
			Undo.RecordObject(child.gameObject, "Change name");
			childrenData.Add(GetObjectData(child.gameObject));
		}

		foreach(var v in childrenData)
		{
			if(v.sentenceParts.SequenceEqual(data.sentenceParts))
			{
				existingIndexes.Add(v.number);
				v.UpdateName(v.number != 0);
			}
		}

		int i = 0;
		while(existingIndexes.Contains(++i)) ;
		data.number = i;
		data.UpdateName(existingIndexes.Count > 0 || IsDuplicating);
	}

	[MenuItem("Shortcuts/Edit/Format name %#r", true)]
	private static bool FormatObjectWithProperNamingValidation()
	{
		return Selection.activeGameObject != null && Selection.gameObjects.Length == 1;
	}

	private static ObjectNameData GetObjectData(GameObject go)
	{
		ObjectNameData result = new ObjectNameData(go);
		var selectedObject = go;
		var startName = selectedObject.name;

		// Decode actual sentence from base name
		var nameParts = ToSentenceCase(startName).ToList();
		var namePartsWithoutNumbers = new List<string>(nameParts);

		// Find out current index and numerical format
		if(HasNumericalFormat(nameParts.Last(), out result.numericalFormat, out result.number))
		{
			namePartsWithoutNumbers.RemoveAt(namePartsWithoutNumbers.Count - 1);
		}
		else
		{
			nameParts.Add(result.number.ToString());
		}
		result.sentenceParts = new List<string>(namePartsWithoutNumbers);

		return result;
	}

	public static string[] ToSentenceCase(string str)
	{
		str = str.Replace("_", " ");
		var firstPass = Regex.Replace(str, "[a-z][A-Z0-9]", m => m.Value[0] + " " + m.Value[1]);
		var secondPass = Regex.Replace(firstPass, "[0-9][a-zA-Z]", m => m.Value[0] + " " + m.Value[1]);
		return secondPass.ToTitleCase().Split(' ');
	}

	public static bool HasNumericalFormat(string str, out string[] format, out int number)
	{
		var match = Regex.Match(str, @"(^.*?[^\d]*)(\d+)([^\d]*.*$)");
		if(match.Groups.Count != 4)
		{
			format = new string[] { string.Empty, string.Empty };
			number = 0;
			return false;
		}
		else
		{
			format = new string[] { match.Groups[1].Value, match.Groups[3].Value };
			number = int.Parse(match.Groups[2].Value);
			return true;
		}
	}


	#endregion

	#region GUI

	#region GUI/Canvas

	private static Vector2 CanvasHDSize = new Vector2(1920, 1080);

	[MenuItem("Shortcuts/GUI/Canvas/Add canvas pack %&c", priority = 1)] // ctrl + alt + c
	private static void CanvasCreatePack()
	{
		GameObject[] selected = Selection.gameObjects;

		foreach(GameObject go in selected)
		{
			if(go.GetComponent<Canvas>() == null)
				Undo.AddComponent<Canvas>(go);
			if(go.GetComponent<CanvasScaler>() == null)
				Undo.AddComponent<CanvasScaler>(go);
			if(go.GetComponent<GraphicRaycaster>() == null)
				Undo.AddComponent<GraphicRaycaster>(go);
			if(go.GetComponent<CanvasGroup>() == null)
				Undo.AddComponent<CanvasGroup>(go);
		}
	}

	[MenuItem("Shortcuts/GUI/Canvas/Add canvas pack %&c", true)]
	private static bool CanvasCreatePackValidation()
	{
		return Selection.gameObjects.Length != 0;
	}

	[MenuItem("Shortcuts/GUI/Canvas/Change canvas into HD world space %&h", priority = 2)] // ctrl + alt + h
	private static void CanvasWorldSpaceHD()
	{
		GameObject[] selected = Selection.gameObjects;

		foreach(GameObject go in selected)
		{
			Undo.RegisterCompleteObjectUndo(go.GetComponent<Canvas>(), "Change canvas properties");
			Undo.RegisterCompleteObjectUndo(go.GetComponent<RectTransform>(), "Change canvas properties");
			go.GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;
			go.GetComponent<RectTransform>().sizeDelta = new Vector2(CanvasHDSize.x, CanvasHDSize.y);
			go.GetComponent<RectTransform>().localScale = new Vector3(1f / CanvasHDSize.x, 1f / CanvasHDSize.x, 1f);
		}
	}

	[MenuItem("Shortcuts/GUI/Canvas/Change canvas into HD world space %&h", true)]
	private static bool CanvasWorldSpaceHDValidation()
	{
		return Selection.gameObjects.Length != 0 && Selection.gameObjects.All(x => x.GetComponent<Canvas>() != null);
	}

	[MenuItem("Shortcuts/GUI/Canvas/Change canvas into HD overlay %&o", priority = 3)] // ctrl + alt + o
	private static void CanvasCameraOverlay()
	{
		GameObject[] selected = Selection.gameObjects;

		foreach(GameObject go in selected)
		{
			Undo.RegisterCompleteObjectUndo(go.GetComponent<Canvas>(), "Change canvas properties");
			Undo.RegisterCompleteObjectUndo(go.GetComponent<CanvasScaler>(), "Change canvas properties");
			go.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
			go.GetComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			go.GetComponent<CanvasScaler>().screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
			go.GetComponent<CanvasScaler>().referenceResolution = CanvasHDSize;
			go.GetComponent<CanvasScaler>().matchWidthOrHeight = 1f;
		}
	}

	[MenuItem("Shortcuts/GUI/Canvas/Change canvas into HD overlay %&o", true)]
	private static bool CanvasCameraOverlayValidation()
	{
		return Selection.gameObjects.Length != 0 && Selection.gameObjects.All(x => x.GetComponent<Canvas>() != null && x.GetComponent<CanvasScaler>() != null);
	}

	#endregion

	[MenuItem("Shortcuts/GUI/Image %&i", priority = 51)] // ctrl + alt + i
	private static void AddImageComponent()
	{
		bool toggle = true;
		foreach(GameObject go in Selection.gameObjects)
		{
			if(go.GetComponent<Image>() == null)
			{
				Undo.AddComponent<Image>(go);
				toggle = false;
			}
		}

		if(toggle)
		{
			ToggleEnabled(Selection.gameObjects.Select(x => x.GetComponent<Image>()));
		}
	}

	[MenuItem("Shortcuts/GUI/Image %&i", true)]
	private static bool AddImageComponentValidation()
	{
		return Selection.gameObjects.Length != 0;
	}

	[MenuItem("Shortcuts/GUI/Button %&b", priority = 52)] // ctrl + alt + b
	private static void AddButtonComponent()
	{
		foreach(GameObject go in Selection.gameObjects)
		{
			if(go.GetComponent<Image>() == null)
				Undo.AddComponent<Image>(go);
			if(go.GetComponent<Button>() == null)
			{
				Undo.AddComponent<Button>(go);
				go.GetComponent<Button>().navigation = new Navigation() { mode = Navigation.Mode.None };
			}
		}
	}

	[MenuItem("Shortcuts/GUI/Button %&b", true)]
	private static bool AddButtonComponentValidation()
	{
		return Selection.gameObjects.Length != 0 && Selection.gameObjects.Any(x => x.GetComponent<Button>() == null);
	}

	[MenuItem("Shortcuts/GUI/Text %&t", priority = 53)] // ctrl + alt + t
	public static void AddTextComponent()
	{
		foreach(GameObject go in Selection.gameObjects)
		{
			if(go.GetComponent<TextMeshProUGUI>() == null)
				Undo.AddComponent<TextMeshProUGUI>(go);
		}
	}

	[MenuItem("Shortcuts/GUI/Text %&t", true)]
	private static bool AddTextComponentValidation()
	{
		return Selection.gameObjects.Length != 0 && Selection.gameObjects.Any(x => x.GetComponent<TextMeshProUGUI>() == null);
	}

	[MenuItem("Shortcuts/GUI/Mask %&m", priority = 54)] // ctrl + alt + m
	private static void AddMaskComponent()
	{
		if(Selection.gameObjects.Any(x => x.GetComponent<Mask>() == null))
		{
			foreach(GameObject go in Selection.gameObjects)
			{
				if(go.GetComponent<Mask>() == null)
					Undo.AddComponent<Mask>(go);
				if(go.GetComponent<Image>() == null)
					Undo.AddComponent<Image>(go);
			}
		}
		else
		{
			foreach(GameObject go in Selection.gameObjects)
			{
				Mask mask = go.GetComponent<Mask>();
				mask.showMaskGraphic = !mask.showMaskGraphic;
			}
		}
	}

	[MenuItem("Shortcuts/GUI/Mask %&m", true)]
	private static bool AddMaskComponentValidation()
	{
		return Selection.gameObjects.Length != 0;
	}

	#region Layout

	[MenuItem("Shortcuts/GUI/LayoutElement %&l", priority = 101)] // ctrl + alt + l
	private static void AddLayoutElementComponent()
	{
		foreach(GameObject go in Selection.gameObjects)
		{
			if(go.GetComponent<LayoutElement>() == null)
				Undo.AddComponent<LayoutElement>(go);
		}
	}

	[MenuItem("Shortcuts/GUI/LayoutElement %&l", true)]
	private static bool AddLayoutElementComponentValidation()
	{
		return Selection.gameObjects.Length != 0 && Selection.gameObjects.Any(x => x.GetComponent<LayoutElement>() == null);
	}

	[MenuItem("Shortcuts/GUI/IgnoreLayout %&k", priority = 102)] // ctrl + alt + k
	private static void IgnoreLayoutElementComponent()
	{
		foreach(GameObject go in Selection.gameObjects)
		{
			LayoutElement le = go.GetComponent<LayoutElement>();
			if(le != null)
			{
				Undo.RegisterCompleteObjectUndo(le, "Ignore layout properties");
				le.ignoreLayout = !le.ignoreLayout;
			}
		}
	}

	[MenuItem("Shortcuts/GUI/IgnoreLayout %&k", true)]
	private static bool IgnoreLayoutElementComponentValidation()
	{
		return Selection.gameObjects.Length != 0 && Selection.gameObjects.All(x => x.GetComponent<LayoutElement>() != null);
	}

	[MenuItem("Shortcuts/GUI/ContentSizeFitter %&w", priority = 103)] // ctrl + alt + w
	private static void AddContentSizeFitterComponent()
	{
		foreach(GameObject go in Selection.gameObjects)
		{
			if(go.GetComponent<ContentSizeFitter>() == null)
				Undo.AddComponent<ContentSizeFitter>(go);
		}
	}

	[MenuItem("Shortcuts/GUI/ContentSizeFitter %&w", true)]
	private static bool AddContentSizeFitterComponentValidation()
	{
		return Selection.gameObjects.Length != 0 && Selection.gameObjects.Any(x => x.GetComponent<ContentSizeFitter>() == null);
	}

	[MenuItem("Shortcuts/GUI/HorizontalLayoutGroup %&y", priority = 104)] // ctrl + alt + w
	private static void AddHorizontalLayoutGroupComponent()
	{
		foreach(GameObject go in Selection.gameObjects)
		{
			if(go.GetComponent<LayoutGroup>() == null)
			{
				var layoutGroup = Undo.AddComponent<HorizontalLayoutGroup>(go);
				layoutGroup.childControlHeight = true;
				layoutGroup.childControlWidth = true;
				layoutGroup.childForceExpandHeight = false;
				layoutGroup.childForceExpandWidth = false;
			}
		}
	}

	[MenuItem("Shortcuts/GUI/HorizontalLayoutGroup %&y", true)]
	private static bool AddHorizontalLayoutGroupComponentValidation()
	{
		return Selection.gameObjects.Length != 0 && Selection.gameObjects.Any(x => x.GetComponent<LayoutGroup>() == null);
	}

	[MenuItem("Shortcuts/GUI/VerticalLayoutGroup %&u", priority = 105)] // ctrl + alt + w
	private static void AddVerticalLayoutGroupComponent()
	{
		foreach(GameObject go in Selection.gameObjects)
		{
			if(go.GetComponent<LayoutGroup>() == null)
			{
				var layoutGroup = Undo.AddComponent<VerticalLayoutGroup>(go);
				layoutGroup.childControlHeight = true;
				layoutGroup.childControlWidth = true;
				layoutGroup.childForceExpandHeight = false;
				layoutGroup.childForceExpandWidth = false;
			}
		}
	}

	[MenuItem("Shortcuts/GUI/VerticalLayoutGroup %&u", true)]
	private static bool AddVerticalLayoutGroupComponentValidation()
	{
		return Selection.gameObjects.Length != 0 && Selection.gameObjects.Any(x => x.GetComponent<LayoutGroup>() == null);
	}


	#endregion

	#endregion

	#region Hierarchy

	[MenuItem("Shortcuts/Hierarchy/Move object up %#[", priority = 1)] // ctrl + shift + arrow up
	private static void HierarchyMoveObjectUp()
	{
		Transform[] selected = Selection.transforms;

		Undo.RegisterFullObjectHierarchyUndo(selected[0].parent, "Move object");

		selected = selected.OrderBy(x => x.GetSiblingIndex()).ToArray();

		foreach(Transform t in selected)
		{
			if(t.GetSiblingIndex() - 1 >= 0)
				t.SetSiblingIndex(t.GetSiblingIndex() - 1);
		}
	}

	[MenuItem("Shortcuts/Hierarchy/Move object up %#[", true)]
	private static bool HierarchyMoveObjectUpValidation()
	{
		if(Selection.transforms.Length == 0
			|| Selection.transforms.Any(x => x.parent == null
			|| x.parent != Selection.transforms[0].parent))
			return false;

		return true;
	}

	[MenuItem("Shortcuts/Hierarchy/Move object down %#]", priority = 2)] // ctrl + shift + arrow down
	private static void HierarchyMoveObjectDown()
	{
		Transform[] selected = Selection.transforms;

		Undo.RegisterFullObjectHierarchyUndo(selected[0].parent, "Move object");

		selected = selected.OrderByDescending(x => x.GetSiblingIndex()).ToArray();

		foreach(Transform t in selected)
		{
			if(t.GetSiblingIndex() + 1 < t.parent.childCount)
				t.SetSiblingIndex(t.GetSiblingIndex() + 1);
		}
	}

	[MenuItem("Shortcuts/Hierarchy/Move object down %#]", true)]
	private static bool HierarchyMoveObjectDownValidation()
	{
		if(Selection.transforms.Length == 0
			|| Selection.transforms.Any(x => x.parent == null
			|| x.parent != Selection.transforms[0].parent))
			return false;

		return true;
	}

	[MenuItem("Shortcuts/Hierarchy/Focus Hierarchy %h")]
	private static void FocusOnHierarchy()
	{
		EditorWindow[] allWindows = Resources.FindObjectsOfTypeAll<EditorWindow>();
		foreach(var v in allWindows)
		{
			if(v.ToString().Contains("SceneHierarchyWindow"))
			{
				v.Focus();
			}
		}
	}

	#endregion

	#region MonoBehaviour extensions

	[MenuItem("CONTEXT/Component/Separate component as child", priority = 1001)]
	static void SeparateComponentAsChild(MenuCommand command)
	{
		Component body = (Component)command.context;
		var staticFlags = GameObjectUtility.GetStaticEditorFlags(body.gameObject);
		var layer = body.gameObject.layer;
		var tag = body.gameObject.tag;
		GameObject child = new GameObject("Child: " + body.GetType().Name);
		Undo.RegisterCreatedObjectUndo(child, "Add child");
		child.transform.SetParent(body.transform);
		child.transform.SetAsFirstSibling();
		child.transform.localPosition = Vector3.zero;
		child.transform.localRotation = Quaternion.identity;
		child.transform.localScale = Vector3.one;
		Component c = Undo.AddComponent(child, body.GetType());
		body.CopyTo(ref c);
		Undo.DestroyObjectImmediate(body);
		GameObjectUtility.SetStaticEditorFlags(child, staticFlags);
		child.layer = layer;
		child.tag = tag;
		Selection.activeGameObject = child;
	}

	[MenuItem("CONTEXT/Component/Separate component as child", true)]
	static bool SeparateComponentAsChildValidation(MenuCommand command)
	{
		Component body = (Component)command.context;
		return !(body is Transform);
	}

	[MenuItem("CONTEXT/RectTransform/Fit to parent")]
	static void FitRectToParent()
	{
		foreach(var v in Selection.gameObjects)
		{
			RectTransform rt = v.transform as RectTransform;
			Undo.RecordObject(rt, "Fit to parent");
			rt.anchorMin = Vector2.zero;
			rt.anchorMax = Vector2.one;
			rt.offsetMin = Vector2.zero;
			rt.offsetMax = Vector2.zero;
		}
	}

	[MenuItem("Shortcuts/Atlases %&g")]
	static void CreateAtlases()
	{
		EditorApplication.ExecuteMenuItem("Assets/Create/Sprite Atlas");
		var e = new Event() { keyCode = KeyCode.Return, type = EventType.KeyDown, };
		EditorWindow.focusedWindow.SendEvent(e);
		var selected = Selection.activeObject;
		SpriteAtlas atlas = (SpriteAtlas)selected;
		var path = AssetDatabase.GetAssetPath(selected);
		var folderPath = path.Substring(0, path.LastIndexOf('/') + 1);
		var splitPath = path.Split('/');
		var folderName = splitPath[splitPath.Length - 2];
		AssetDatabase.RenameAsset(path, folderName);

		List<string> sprites = new List<string>();
		sprites.AddRange(Directory.GetFiles(folderPath, "*.png", SearchOption.AllDirectories));
		sprites.AddRange(Directory.GetFiles(folderPath, "*.jpg", SearchOption.AllDirectories));
		List<Sprite> spriteObjects = new List<Sprite>();
		foreach(string sprite in sprites)
		{
			string assetPath = sprite.Replace(Application.dataPath, "").Replace('\\', '/');
			Debug.Log(assetPath);
			Sprite s = (Sprite)AssetDatabase.LoadAssetAtPath(assetPath, typeof(Sprite));
			spriteObjects.Add(s);
		}
		atlas.Add(spriteObjects.ToArray());
	}

	#endregion

	#region Navigation

	[MenuItem("GameObject/Custom/Buttons Auto Navigation", priority = 0)]
	private static void SetAutoNavigation()
	{
		Button[] buttons = Selection.activeGameObject.GetComponentsInChildren<Button>();
		Undo.RecordObjects(buttons, "Change button navigation");

		foreach(var btn in buttons)
		{
			Navigation nav = btn.navigation;
			nav.mode = Navigation.Mode.Explicit;
			btn.navigation = nav;
		}

		foreach(var btn in buttons)
		{
			Navigation nav = btn.navigation;
			nav.selectOnDown = Extensions.FindSelectable(btn, buttons, Vector3.down, false);
			nav.selectOnUp = Extensions.FindSelectable(btn, buttons, Vector3.up, false);
			nav.selectOnLeft = Extensions.FindSelectable(btn, buttons, Vector3.left, false);
			nav.selectOnRight = Extensions.FindSelectable(btn, buttons, Vector3.right, false);
			btn.navigation = nav;
		}
	}

	#endregion Navigation

	#region Game

	[MenuItem("Shortcuts/Window/Pause game &q", priority = 2)] // ctrl + q
	private static void PauseGame()
	{
		EditorApplication.isPaused = !EditorApplication.isPaused;
	}

	[MenuItem("Shortcuts/Window/Pause game &q", true)]
	private static bool PauseGameValidation()
	{
		return EditorApplication.isPlaying;
	}

	[MenuItem("Shortcuts/Window/Maximize Game View &w", priority = 1)] // alt + w
	public static void MaximizeGameView()
	{
		EditorWindow.focusedWindow.maximized = !EditorWindow.focusedWindow.maximized;
		UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
	}

	[MenuItem("Shortcuts/Window/Maximize Game View &w", true)]
	public static bool MaximizeGameViewValidation()
	{
		return EditorWindow.focusedWindow != null && EditorWindow.focusedWindow.GetType().FullName == "UnityEditor.GameView";
	}

	#endregion Game

	[MenuItem("Shortcuts/Clear All Player Prefs")]
	public static void ClearPlayerPrefs()
	{
		if(EditorUtility.DisplayDialog("Clear All PlayerPrefs Data", "WARNING: This will delete ALL PlayerPrefs data in your project, not just the keys used by Rewired (because there is no way to search for keys by prefix). This cannot be undone! Are you sure?", "DELETE", "Cancel"))
		{
			PlayerPrefs.DeleteAll();
		}
	}

	[MenuItem("Shortcuts/Force Context Menu #/")]
	public static void ForceContextMenu()
	{
		CustomContextMenuCreator.ShowContextMenu();
	}

	private static void ToggleEnabled(IEnumerable<MonoBehaviour> components)
	{
		foreach(var v in components)
		{
			v.enabled = !v.enabled;
		}
	}

}
