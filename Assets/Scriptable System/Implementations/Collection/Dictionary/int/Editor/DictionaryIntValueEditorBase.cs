using SA.ScriptableData.Editor;
using UnityEditor;
using UnityEngine;

namespace SA.ScriptableData.Collection.Editor
{
	public abstract class DictionaryIntValueEditorBase<TValue> : ScriptableDictionaryValueEditorBase<int, TValue>
	{
		protected override void DictionaryElementGUI(SerializedProperty keyElement, SerializedProperty valueElement, int index, bool keyDuplicated)
		{
			using(new EditorGUILayout.HorizontalScope())
			{
				EditorExtensions.EditorModifiers.PushLabelWidth(EditorExtensions.Text.GetEditorLabelWidth(keyLabel.text));
				if(keyDuplicated)
					EditorExtensions.EditorModifiers.PushLabelColor(Color.red);
				EditorGUILayout.PropertyField(keyElement, keyLabel, GUILayout.ExpandWidth(false));
				if(keyDuplicated)
					EditorExtensions.EditorModifiers.PopLabelColor();
				EditorExtensions.EditorModifiers.PopLabelWidth();

				EditorExtensions.EditorModifiers.PushLabelWidth(EditorExtensions.Text.GetEditorLabelWidth(valueLabel.text));
				EditorGUILayout.PropertyField(valueElement, valueLabel);
				EditorExtensions.EditorModifiers.PopLabelWidth();
			}
		}
	}
}