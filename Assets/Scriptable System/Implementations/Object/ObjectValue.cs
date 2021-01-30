using UnityEngine;

namespace SA.ScriptableData
{
	[CreateAssetMenu(fileName = "ObjectValue", menuName = "Scriptable Data/Object", order = 'o')]
	public class ObjectValue : GenericObjectValue<Object> { }
}