using UnityEngine;

namespace SA.ScriptableData.Editor
{
	public abstract class GenericObjectValueEditor<T> : ScriptableValueEditorBase<GenericObjectValue<T>, T>
		where T : Object
	{ }
}