using UnityEditor;

namespace SA.ScriptableData.Editor
{
	[CustomEditor(typeof(IntValue))]
	public class IntValueEditor : ScriptableValueEditorBase<IntValue, int> { }
}