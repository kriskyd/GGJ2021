using UnityEngine;

namespace SA.ScriptableData
{
	[CreateAssetMenu(fileName = "StringValue", menuName = "Scriptable Data/Single/String", order = 's')]
	public class StringValue : ScriptableValue<string> { }
}