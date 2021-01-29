using SA.AnimatedValues;
using UnityEngine;

namespace SA.ScriptableData.Animated
{
	[CreateAssetMenu(fileName = "AnimBoolValue", menuName = "Scriptable Data/Anim/Bool", order = 'b')]
	public class AnimBoolValue : ScriptableAnimValue<AnimBool, bool> { }
}