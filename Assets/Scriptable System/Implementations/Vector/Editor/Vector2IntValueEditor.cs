using UnityEditor;
using UnityEngine;

namespace SA.ScriptableData.Editor
{
	[CustomEditor(typeof(Vector2IntValue))]
	public class Vector2IntValueEditor : ScriptableValueEditorBase<Vector2IntValue, Vector2Int> { }
}