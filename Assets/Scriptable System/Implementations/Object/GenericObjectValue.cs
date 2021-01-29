using UnityEngine;

namespace SA.ScriptableData
{
	public abstract class GenericObjectValue<T> : ScriptableValue<T>
		where T : Object
	{ }
}