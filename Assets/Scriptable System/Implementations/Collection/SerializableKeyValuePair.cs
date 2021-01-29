using System;
using System.Collections.Generic;
using UnityEngine;

namespace SA.ScriptableData
{
	[Serializable]
	public abstract class SerializableKeyValuePair<TKey, TValue>
	{
		[SerializeField] private TKey key;
		[SerializeField] private TValue value;

		public TKey Key { get => key; }
		public TValue Value { get => value; }

		public SerializableKeyValuePair(TKey key, TValue value)
		{
			this.key = key;
			this.value = value;
		}

		public static implicit operator KeyValuePair<TKey, TValue>(SerializableKeyValuePair<TKey, TValue> serializableKeyValuePair)
		{
			return new KeyValuePair<TKey, TValue>(serializableKeyValuePair.Key, serializableKeyValuePair.Value);
		}
	}
}
