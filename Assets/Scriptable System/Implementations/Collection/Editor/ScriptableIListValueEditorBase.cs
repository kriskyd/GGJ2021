using System.Collections.Generic;

namespace SA.ScriptableData.Collection.Editor
{
	public abstract class ScriptableIListValueEditorBase<T, V, U> : ScriptableICollectionValueEditorBase<T, V, U>
		where T : ScriptableIListValue<V, U>
		where V : IList<U>, new()
	{ }
}