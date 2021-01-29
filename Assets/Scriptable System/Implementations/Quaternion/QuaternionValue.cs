using UnityEngine;

namespace SA.ScriptableData
{
	[CreateAssetMenu(fileName = "QuaternionValue", menuName = "Scriptable Data/Quaternion", order = 'q')]
	public class QuaternionValue : ScriptableValue<Quaternion> { }
}