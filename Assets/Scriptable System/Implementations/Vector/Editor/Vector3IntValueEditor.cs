using UnityEditor;
using UnityEngine;

namespace SA.ScriptableData.Editor
{
	[CustomEditor(typeof(Vector3IntValue))]
	public class Vector3IntValueEditor : ScriptableValueEditorBase<Vector3IntValue, Vector3Int> { }
}