using UnityEditor;
using UnityEngine;

namespace SA.ScriptableData.Editor
{
	[CustomEditor(typeof(QuaternionValue))]
	public class QuaternionValueEditor : ScriptableValueEditorBase<QuaternionValue, Quaternion>
	{
		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorExtensions.Drawer.QuaternionField(value);

			CheckForModifiedProperties();
		}
	}
}