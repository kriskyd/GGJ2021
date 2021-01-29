using System.Collections.Generic;
using UnityEditor;

namespace SA.ScriptableData.Collection.Editor
{
	[CustomEditor(typeof(ListStringValue))]
	public class ListStringValueEditor : ScriptableListValueEditorBase<ListStringValue, string> { }
}