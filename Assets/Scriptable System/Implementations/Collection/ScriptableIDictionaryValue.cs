using System;
using System.Collections.Generic;
using UnityEngine;

namespace SA.ScriptableData.Collection
{
	public abstract class ScriptableIDictionaryValue<T, TKey, TValue> : ScriptableICollectionValue<T, KeyValuePair<TKey, TValue>>, ISerializationCallbackReceiver
		where T : IDictionary<TKey, TValue>, new()
	{
		[SerializeField] protected List<TKey> keys;
		[SerializeField] protected List<TValue> values;

		protected T Dictionary
		{
			get => Collection;
			set => Collection = value;
		}

		public TValue this[TKey key]
		{
			get => Dictionary[key];
			set
			{
				Dictionary[key] = value;
				OnCollectionChanged();
			}
		}

		public bool IsReadOnly => Dictionary.IsReadOnly;

		public ICollection<TKey> Keys => Dictionary.Keys;

		public new ICollection<TValue> Values => Dictionary.Values;

		public virtual void Add(TKey key, TValue value)
		{
			Dictionary.Add(key, value);
			OnCollectionChanged();
		}

		public override void Add(KeyValuePair<TKey, TValue> item)
		{
			Dictionary.Add(item);
			OnCollectionChanged();
		}

		public virtual bool TryAdd(TKey key, TValue value)
		{
			if(Dictionary.ContainsKey(key))
				return false;

			Dictionary.Add(key, value);
			OnCollectionChanged();
			return true;
		}

		public virtual bool ContainsKey(TKey key)
		{
			return Dictionary?.ContainsKey(key) ?? false;
		}

		public override bool Contains(KeyValuePair<TKey, TValue> item)
		{
			return Dictionary?.Contains(item) ?? false;
		}

		public override void Clear()
		{
			base.Clear();
		}

		public virtual bool Remove(TKey key)
		{
			bool result = Dictionary.Remove(key);

			if(result)
			{
				OnCollectionChanged();
			}

			return result;
		}

		public override bool Remove(KeyValuePair<TKey, TValue> item)
		{
			bool result = Dictionary.Remove(item);

			if(result)
			{
				OnCollectionChanged();
			}

			return result;
		}

		public virtual bool TryGetValue(TKey key, out TValue value)
		{
			return Dictionary.TryGetValue(key, out value);
		}

		public override IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return Dictionary.GetEnumerator();
		}

		public void OnBeforeSerialize() { }

		public void OnAfterDeserialize()
		{
			// In editor keep both <keys and values> and <dictionary> for debugging and editing.
			Dictionary = new T();
			for(int i = 0; i != Math.Min(keys.Count, values.Count); i++)
			{
				TryAdd(keys[i], values[i]);
			}

#if !UNITY_EDITOR
			keys.Clear();
			values.Clear();
#endif
		}

		public static implicit operator T(ScriptableIDictionaryValue<T, TKey, TValue> scriptableIDictionaryValue)
		{
			return scriptableIDictionaryValue.Dictionary;
		}
	}
}