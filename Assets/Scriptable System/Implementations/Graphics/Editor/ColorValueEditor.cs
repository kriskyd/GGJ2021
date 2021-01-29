using UnityEditor;
using UnityEngine;

namespace SA.ScriptableData.Editor
{
	[CustomEditor(typeof(ColorValue))]
	public class ColorValueEditor : ScriptableValueEditorBase<ColorValue, Color> { }
}