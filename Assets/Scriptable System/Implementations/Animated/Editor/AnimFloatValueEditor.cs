using SA.AnimatedValues;
using UnityEditor;

namespace SA.ScriptableData.Animated.Editor
{
	[CustomEditor(typeof(AnimFloatValue))]
	public class AnimFloatValueEditor : ScriptableAnimValueEditorBase<AnimFloatValue, AnimFloat, float> { }
}