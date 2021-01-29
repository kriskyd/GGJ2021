using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SA.ScriptableData.Collection.Editor
{
	[CustomEditor(typeof(ListVector3IntValue))]
	public class ListVector3IntValueEditor : ScriptableListValueEditorBase<ListVector3IntValue, Vector3Int> { }
}