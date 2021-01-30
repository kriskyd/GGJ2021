using System.Collections.Generic;
using UnityEngine;

namespace SA.ScriptableData.Collection
{
	[CreateAssetMenu(fileName = "ListQuaternion", menuName = "Scriptable Data/Collection/List/Quaternion", order = 'q')]
	public class ListQuaternionValue : ScriptableListValue<Quaternion> { }
}