using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// Full code: https://forum.unity.com/threads/sceneview-context-menu-script-free.525029/

public class CustomContextMenuCreator : Editor
{
	static MenuSet mainMenuOptions = null;
	static Dictionary<string, List<string[]>> contextOptions;

	static void FindAllContextOptions()
	{
		mainMenuOptions = new MenuSet();

		var menuString = EditorGUIUtility.SerializeMainMenuToString();
		var menus = menuString.Split('\n');
		var pathParts = new List<string>();
		var menuPaths = new List<string>();

		contextOptions = new Dictionary<string, List<string[]>>();

		foreach(var m in menus)
		{
			var s = m.Split(new string[] { "    " }, StringSplitOptions.None);
			var n = s[s.Length - 1];

			// Add to path parts.
			if(pathParts.Count <= s.Length)
				pathParts.Add(n);
			else
				pathParts[s.Length - 1] = n;

			// Get full path.
			var path = "";
			var parts = new List<string>();
			var menuSet = mainMenuOptions;
			for(int i = 0; i < s.Length; i++)
			{
				var pp = pathParts[i];
				parts.Add(pp);
				path += pp;

				if(!menuSet.children.ContainsKey(pp))
				{
					var ms = new MenuSet();
					ms.fullPath = path;
					ms.pathPart = pp;
					menuSet.children.Add(pp, ms);
				}

				menuSet = menuSet.children[pp];

				if(i != s.Length - 1)
					path += "/";
			}

			// Context menus.
			if(path.Contains("CONTEXT"))
			{
				var cParts = path.Split('/');
				if(cParts.Length >= 3)
				{
					var component = cParts[1];
					var label = cParts[2];

					if(!contextOptions.ContainsKey(component))
						contextOptions.Add(component, new List<string[]>());

					contextOptions[component].Add(new string[] {
						// Nice label.
						component + " - " + label,
						// Actual menu item.
						path });
				}
			}
			menuPaths.Add(path);
		}
	}

	public static void ShowContextMenu()
	{
		if(mainMenuOptions == null)
			FindAllContextOptions();

		var go = Selection.activeGameObject;

		if(go != null)
		{
			ShowMenu(go);
			return;
		}
	}

	static IEnumerable<ContextItem> GetMenuFromComponent(Component mb)
	{
		if(mb == null)
			return new ContextItem[0];

		var list = new List<ContextItem>();
		var name = mb.GetType().Name;
		var methods = mb.GetType().GetMethods((BindingFlags)111100);

		// Methods marked with [ContextMenu].
		foreach(var m in methods.Where(y => y.IsDefined(typeof(ContextMenu), true)))
		{
			var atr = (ContextMenu)m.GetCustomAttributes(typeof(ContextMenu), true)[0];
			var path = atr.menuItem.Replace("/", " - ");
			list.Add(new ContextItem(path, () => m.Invoke(mb, null)));
		}

		// Custom context menus for internal classes. i.e. [ContextMenu("CONTEXT/Transform/Reset Position")]
		if(contextOptions.ContainsKey(name))
		{
			var cop = contextOptions[name];
			foreach(var c in cop)
			{
				var niceName = c[0];
				var command = c[1];
				list.Add(new ContextItem(niceName, () => { EditorApplication.ExecuteMenuItem(command); }));
			}
		}

		return list;
	}

	static void ShowMenu(GameObject go)
	{
		var components = go.GetComponents<Component>().Where(x => x != null).ToArray();

		var itemsFromAttributes = components.SelectMany(x => GetMenuFromComponent(x)).ToList();

		var menu = new GenericMenu();

		if(itemsFromAttributes.Count > 0)
		{
			for(int i = 0; i < itemsFromAttributes.Count; i++)
			{
				var mi = itemsFromAttributes[i];
				menu.AddItem(new GUIContent(mi.label), false, () => mi.callBack());
			}
		}

		menu.ShowAsContext();
	}

	public class MenuSet
	{
		public string pathPart = null;
		public string fullPath = null;
		public Dictionary<string, MenuSet> children = new Dictionary<string, MenuSet>();

		public List<string> GetSubPaths()
		{
			var l = new List<string>();

			foreach(var c in children)
				if(c.Value.children.Count == 0)
					l.Add(c.Value.fullPath);
				else
					l.AddRange(c.Value.GetSubPaths());

			return l;
		}
	}
}

public struct ContextItem
{
	public string label;
	public Action callBack;

	public ContextItem(string label, Action callBack)
	{
		this.label = label;
		this.callBack = callBack;
	}
}