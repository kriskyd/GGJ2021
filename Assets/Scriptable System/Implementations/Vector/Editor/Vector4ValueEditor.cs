using UnityEditor;
using UnityEngine;

namespace SA.ScriptableData.Editor
{
	[CustomEditor(typeof(Vector4Value))]
	public class Vector4ValueEditor : ScriptableValueEditorBase<Vector4Value, Vector4>
	{
		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			value.vector4Value = EditorGUILayout.Vector4Field(value.displayName, value.vector4Value);

			CheckForModifiedProperties();
		}
	}
}