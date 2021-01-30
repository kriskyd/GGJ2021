using SA.AnimatedValues;
using SA.ScriptableData.Editor;
using UnityEditor;
using UnityEngine;

namespace SA.ScriptableData.Animated.Editor
{
	[CustomEditor(typeof(AnimQuaternionValue))]
	public class AnimQuaternionValueEditor : ScriptableAnimValueEditorBase<AnimQuaternionValue, AnimQuaternion, Quaternion>
	{
		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorExtensions.Drawer.QuaternionField(targetValue);
			if(value != null)
				EditorExtensions.Drawer.QuaternionField(value);
			EditorGUILayout.PropertyField(speed);

			CheckForModifiedProperties();
		}

	}
}