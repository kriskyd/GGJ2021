using System.Collections.Generic;

namespace SA.ScriptableData.Collection.Editor
{
	public abstract class ScriptableDictionaryValueEditorBase<TKey, TValue> : ScriptableIDictionaryValueEditorBase<ScriptableDictionaryValue<TKey, TValue>, Dictionary<TKey, TValue>, TKey, TValue>
	{ }
}