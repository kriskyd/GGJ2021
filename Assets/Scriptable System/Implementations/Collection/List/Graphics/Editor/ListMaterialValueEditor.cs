using System.Collections.Generic;
using SA.ScriptableData.Editor;
using UnityEditor;
using UnityEngine;

namespace SA.ScriptableData.Collection.Editor
{
	[CustomEditor(typeof(ListMaterialValue))]
	public class ListMaterialValueEditor : ScriptableListValueEditorBase<ListMaterialValue, Material>
	{
		List<MaterialEditor> materialEditors = new List<MaterialEditor>();

		protected override void OnEnable()
		{
			base.OnEnable();

			if(target.Value != null)
			{
				CreateEditors();
				target.CollectionChanged += DisposeEditors;
				target.CollectionChanged += CreateEditors;
			}
		}

		protected override void OnDisable()
		{
			base.OnDisable();

			DisposeEditors();
			if(target != null) // destroyed
			{
				target.CollectionChanged -= DisposeEditors;
				target.CollectionChanged -= CreateEditors;
			}
		}

		private void CreateEditors()
		{
			materialEditors = new List<MaterialEditor>();
			for(int i = 0; i < target.Value.Count; i++)
			{
				materialEditors.Add(null);
				if(target.Value[i] != null)
					materialEditors[i] = (MaterialEditor)CreateEditor(target.Value[i]);
			}
		}

		private void DisposeEditors()
		{
			for(int i = 0; i < materialEditors.Count; i++)
				if(materialEditors[i] != null)
					DestroyImmediate(materialEditors[i]);
		}

		protected override void ArrayElementGUI(SerializedProperty arrayElement, int index)
		{
			EditorExtensions.Drawer.FoldableObjectField(arrayElement, typeof(Material));

			if(arrayElement.isExpanded)
			{
				if(materialEditors.Count > index && materialEditors[index] != null)
				{
					materialEditors[index].DrawHeader();

					bool isDefaultMaterial = !AssetDatabase.GetAssetPath(target.Value[index]).StartsWith("Assets");

					using(new EditorGUI.DisabledGroupScope(isDefaultMaterial))
					{
						materialEditors[index].OnInspectorGUI();
					}
				}
			}

			CheckForModifiedProperties();
		}
	}
}