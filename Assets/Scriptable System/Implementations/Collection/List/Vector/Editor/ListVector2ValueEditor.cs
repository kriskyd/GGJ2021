using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SA.ScriptableData.Collection.Editor
{
	[CustomEditor(typeof(ListVector2Value))]
	public class ListVector2ValueEditor : ScriptableListValueEditorBase<ListVector2Value, Vector2> { }

}