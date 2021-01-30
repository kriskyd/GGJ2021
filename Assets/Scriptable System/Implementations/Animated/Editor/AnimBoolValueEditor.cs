using SA.AnimatedValues;
using UnityEditor;

namespace SA.ScriptableData.Animated.Editor
{
	[CustomEditor(typeof(AnimBoolValue))]
	public class AnimBoolValueEditor : ScriptableAnimValueEditorBase<AnimBoolValue, AnimBool, bool>
	{
		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(targetValue, true);
			if(value != null) // field m_Value implemented in class derived from ScriptableAnimValue
				value.floatValue = EditorGUILayout.Toggle("Value", value.floatValue == 1f) ? 1f : 0f;
			EditorGUILayout.PropertyField(speed);

			CheckForModifiedProperties();
		}
	}
}