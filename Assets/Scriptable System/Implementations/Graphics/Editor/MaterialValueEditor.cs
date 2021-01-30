using UnityEditor;
using UnityEngine;

namespace SA.ScriptableData.Editor
{
	[CustomEditor(typeof(MaterialValue))]
	public class MaterialValueEditor : GenericObjectValueEditor<Material>
	{
		MaterialEditor materialEditor;

		protected override void OnEnable()
		{
			base.OnEnable();

			CreateEditor();

			target.ValueChanged += DisposeEditor;
			target.ValueChanged += CreateEditor;
		}

		protected override void OnDisable()
		{
			base.OnDisable();

			DisposeEditor();

			if(target != null) // destroyed
			{
				target.ValueChanged -= DisposeEditor;
				target.ValueChanged -= CreateEditor;
			}
		}

		private void CreateEditor()
		{
			if(target.Value != null)
				materialEditor = (MaterialEditor)CreateEditor(target.Value);
			else
				materialEditor = null;
		}

		private void DisposeEditor()
		{
			if(materialEditor != null)
				DestroyImmediate(materialEditor);
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorExtensions.Drawer.FoldableObjectField(value, typeof(Material));

			if(value.isExpanded)
			{
				if(materialEditor != null)
				{
					materialEditor.DrawHeader();

					bool isDefaultMaterial = !AssetDatabase.GetAssetPath(target.Value).StartsWith("Assets");

					using(new EditorGUI.DisabledGroupScope(isDefaultMaterial))
					{
						materialEditor.OnInspectorGUI();
					}
				}
			}

			CheckForModifiedProperties();
		}
	}
}