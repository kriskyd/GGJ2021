using SA.AnimatedValues;
using UnityEngine;

namespace SA.ScriptableData.Animated
{
	[CreateAssetMenu(fileName = "AnimQuaternionValue", menuName = "Scriptable Data/Anim/Quaternion", order = 'q')]
	public class AnimQuaternionValue : ScriptableAnimValue<AnimQuaternion, Quaternion> { }
}