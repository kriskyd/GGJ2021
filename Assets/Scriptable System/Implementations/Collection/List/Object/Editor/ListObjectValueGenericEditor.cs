using UnityEngine;

namespace SA.ScriptableData.Collection.Editor
{
	public abstract class ListObjectValueGenericEditor<T> : ScriptableListValueEditorBase<ListObjectValue<T>, T>
		where T : Object
	{ }
}