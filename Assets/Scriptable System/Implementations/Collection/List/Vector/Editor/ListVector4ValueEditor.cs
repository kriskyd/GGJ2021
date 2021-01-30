using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SA.ScriptableData.Collection.Editor
{
	[CustomEditor(typeof(ListVector4Value))]
	public class ListVector4ValueEditor : ScriptableListValueEditorBase<ListVector4Value, Vector4>
	{
		protected override void ArrayElementGUI(SerializedProperty arrayElement, int index)
		{
			arrayElement.vector4Value = EditorGUILayout.Vector4Field(arrayElement.displayName, arrayElement.vector4Value);
		}
	}
}