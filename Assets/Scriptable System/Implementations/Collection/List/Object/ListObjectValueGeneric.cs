using System.Collections.Generic;
using UnityEngine;

namespace SA.ScriptableData.Collection
{
	public abstract class ListObjectValue<T> : ScriptableListValue<T>
		where T : Object
	{ }
}