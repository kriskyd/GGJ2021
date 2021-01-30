using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SA.ScriptableData.Collection.Editor
{
	[CustomEditor(typeof(ListColorValue))]
	public class ListColorValueEditor : ScriptableListValueEditorBase<ListColorValue, Color> { }
}