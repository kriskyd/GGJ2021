using UnityEditor;

namespace SA.ScriptableData.Editor
{
	[CustomEditor(typeof(StringValue))]
	public class StringValueEditor : ScriptableValueEditorBase<StringValue, string> { }
}