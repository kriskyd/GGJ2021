using UnityEditor;
using UnityEngine;

namespace SA.ScriptableData.Editor
{
	[CustomEditor(typeof(Vector2Value))]
	public class Vector2ValueEditor : ScriptableValueEditorBase<Vector2Value, Vector2> { }
}