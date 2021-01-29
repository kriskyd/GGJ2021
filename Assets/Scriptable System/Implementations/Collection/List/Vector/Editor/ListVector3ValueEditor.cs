using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SA.ScriptableData.Collection.Editor
{
	[CustomEditor(typeof(ListVector3Value))]
	public class ListVector3ValueEditor : ScriptableListValueEditorBase<ListVector3Value, Vector3> { }
}