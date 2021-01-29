using SA.AnimatedValues;
using UnityEngine;

namespace SA.ScriptableData.Animated
{
	[CreateAssetMenu(fileName = "AnimVector3Value", menuName = "Scriptable Data/Anim/Vector3", order = 'v')]
	public class AnimVector3Value : ScriptableAnimValue<AnimVector3, Vector3> { }
}