using UnityEditor;
using UnityEngine;

namespace SA.ScriptableData.Editor
{
	[CustomEditor(typeof(SpriteValue))]
	public class SpriteValueEditor : GenericObjectValueEditor<Sprite>
	{
		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			BigSpritePropertyDrawer drawer = new BigSpritePropertyDrawer(64f, 64f);
			GUIContent content = new GUIContent(value.displayName);
			Rect drawerRect = EditorGUILayout.GetControlRect(true, drawer.GetPropertyHeight(value, content));
			drawer.OnGUI(drawerRect, value, content);

			CheckForModifiedProperties();
		}
	}
}