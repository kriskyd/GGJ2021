using System.Collections.Generic;
using SA.ScriptableData.Editor;
using UnityEditor;

namespace SA.ScriptableData.Collection.Editor
{
	public abstract class ScriptableICollectionValueEditorBase<T, V, U> : ScriptableValueEditorBase<T, V>
		where T : ScriptableICollectionValue<V, U>
		where V : ICollection<U>, new()
	{
		public new T target => base.target;

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorExtensions.Collection.DrawArrayProperty(serializedObject, value, this.ArrayElementGUI);

			CheckForModifiedProperties();
		}

		protected override void CheckForModifiedProperties()
		{
			if(serializedObject.hasModifiedProperties)
			{
				serializedObject.ApplyModifiedProperties();
				target.OnCollectionChanged();
			}
		}

		protected virtual void ArrayElementGUI(SerializedProperty arrayElement, int index)
		{
			EditorGUILayout.PropertyField(arrayElement);
		}
	}
}