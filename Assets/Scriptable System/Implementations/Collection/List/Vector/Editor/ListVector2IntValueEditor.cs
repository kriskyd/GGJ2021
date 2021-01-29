using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SA.ScriptableData.Collection.Editor
{
	[CustomEditor(typeof(ListVector2IntValue))]
	public class ListVector2IntValueEditor : ScriptableListValueEditorBase<ListVector2IntValue, Vector2Int> { }

}