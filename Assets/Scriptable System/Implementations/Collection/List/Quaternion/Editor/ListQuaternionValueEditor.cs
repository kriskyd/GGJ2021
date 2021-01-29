using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SA.ScriptableData.Collection.Editor
{
	[CustomEditor(typeof(ListQuaternionValue))]
	public class ListQuaternionValueEditor : ScriptableListValueEditorBase<ListQuaternionValue, Quaternion>
	{
		protected override void ArrayElementGUI(SerializedProperty arrayElement, int index)
		{
			var vector4 = new Vector4(
				arrayElement.quaternionValue.x,
				arrayElement.quaternionValue.y,
				arrayElement.quaternionValue.z,
				arrayElement.quaternionValue.w);
			vector4 = EditorGUILayout.Vector4Field(arrayElement.displayName, vector4);
			arrayElement.quaternionValue = new Quaternion(vector4.x, vector4.y, vector4.z, vector4.w);
		}
	}
}