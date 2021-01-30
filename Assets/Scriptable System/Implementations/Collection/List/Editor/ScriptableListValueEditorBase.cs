using System.Collections.Generic;

namespace SA.ScriptableData.Collection.Editor
{
	public abstract class ScriptableListValueEditorBase<T, V> : ScriptableIListValueEditorBase<T, List<V>, V>
		where T : ScriptableListValue<V>
	{ }
}