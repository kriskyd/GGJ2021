using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SA.ScriptableData.Collection.Editor
{
	[CustomEditor(typeof(ListSpriteValue))]
	public class ListSpriteValueEditor : ListObjectValueGenericEditor<Sprite> { }
}