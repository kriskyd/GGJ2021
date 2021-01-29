using SA.AnimatedValues;
using UnityEditor;
using UnityEngine;

namespace SA.ScriptableData.Animated.Editor
{
	[CustomEditor(typeof(AnimVector3Value))]
	public class AnimVector3ValueEditor : ScriptableAnimValueEditorBase<AnimVector3Value, AnimVector3, Vector3> { }
}