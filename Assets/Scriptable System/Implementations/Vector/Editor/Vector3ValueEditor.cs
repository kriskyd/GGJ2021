using UnityEditor;
using UnityEngine;

namespace SA.ScriptableData.Editor
{
	[CustomEditor(typeof(Vector3Value))]
	public class Vector3ValueEditor : ScriptableValueEditorBase<Vector3Value, Vector3> { }
}