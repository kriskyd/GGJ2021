using SA.AnimatedValues;
using UnityEngine;

namespace SA.ScriptableData.Animated
{
	[CreateAssetMenu(fileName = "AnimFloatValue", menuName = "Scriptable Data/Anim/Float", order = 'f')]
	public class AnimFloatValue : ScriptableAnimValue<AnimFloat, float> { }
}