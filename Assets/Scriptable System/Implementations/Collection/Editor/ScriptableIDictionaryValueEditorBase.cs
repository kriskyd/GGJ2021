using System.Collections.Generic;
using SA.ScriptableData.Editor;
using UnityEditor;
using UnityEngine;

namespace SA.ScriptableData.Collection.Editor
{
	public abstract class ScriptableIDictionaryValueEditorBase<T, V, TKey, TValue> : ScriptableICollectionValueEditorBase<T, V, KeyValuePair<TKey, TValue>>
		where T : ScriptableIDictionaryValue<V, TKey, TValue>
		where V : IDictionary<TKey, TValue>, new()
	{
		protected SerializedProperty keys;
		protected SerializedProperty values;
		protected GUIContent keyLabel;
		protected GUIContent valueLabel;

		protected override void OnEnable()
		{
			base.OnEnable();
			keys = serializedObject.FindProperty("keys");
			values = serializedObject.FindProperty("values");
			keyLabel = new GUIContent("Key");
			valueLabel = new GUIContent("Value");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorExtensions.Collection.DrawDictionaryProperty(serializedObject, keys, values, this.DictionaryElementGUI);

			CheckForModifiedProperties();
		}

		protected virtual void DictionaryElementGUI(SerializedProperty keyElement, SerializedProperty valueElement, int index, bool keyDuplicated)
		{
			using(new EditorGUILayout.HorizontalScope())
			{
				EditorExtensions.EditorModifiers.PushLabelWidth(38f);
				if(keyDuplicated)
					EditorExtensions.EditorModifiers.PushLabelColor(Color.red);
				EditorGUILayout.PropertyField(keyElement, keyLabel);
				if(keyDuplicated)
					EditorExtensions.EditorModifiers.PopLabelColor();
				EditorExtensions.EditorModifiers.PopLabelWidth();

				EditorExtensions.EditorModifiers.PushLabelWidth(52f);
				EditorGUILayout.PropertyField(valueElement, valueLabel);
				EditorExtensions.EditorModifiers.PopLabelWidth();
			}
		}
	}
}