using SA.AnimatedValues;
using UnityEditor;

namespace SA.ScriptableData.Animated.Editor
{
	public abstract class ScriptableAnimValueEditorBase<T, V, U> : ScriptableValueEditorBase<T, V>
		where T : ScriptableAnimValue<V, U>
		where V : AnimValueBase<U>
	{
		protected new SerializedProperty value;
		protected SerializedProperty targetValue;
		protected SerializedProperty speed;

		protected override void OnEnable()
		{
			base.OnEnable();

			value = base.value.FindPropertyRelative("m_Value");
			targetValue = base.value.FindPropertyRelative("target");
			speed = base.value.FindPropertyRelative("speed");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(targetValue, true);
			if(value != null) // field m_Value implemented in class derived from ScriptableAnimValue
				EditorGUILayout.PropertyField(value, true);
			EditorGUILayout.PropertyField(speed);

			CheckForModifiedProperties();
		}
	}
}