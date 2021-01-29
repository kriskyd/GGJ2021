using UnityEditor;
using UnityEngine;

namespace SA.ScriptableData.Editor
{
	[CustomEditor(typeof(ObjectValue))]
	public class ObjectValueEditor : GenericObjectValueEditor<Object> { }
}