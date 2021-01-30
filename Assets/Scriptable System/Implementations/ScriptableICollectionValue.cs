using System;
using System.Collections.Generic;

namespace SA.ScriptableData.Collection
{
	public abstract class ScriptableICollectionValue<T, V> : ScriptableValue<T>
		where T : ICollection<V>, new()
	{
		protected T Collection
		{
			get
			{
				return base.Value;
			}
			set
			{
				base.Value = value;
			}
		}

		public virtual T Values
		{
			get
			{
				T newCollection = new T();

				if(Collection == null)
				{
					return newCollection;
				}

				var enumerator = Collection.GetEnumerator();
				while(enumerator.MoveNext())
				{
					newCollection.Add(enumerator.Current);
				}
				return newCollection;
			}
		}

		public virtual int Count => Collection?.Count ?? 0;

		public event Action CollectionChanged;

		public virtual void Add(V item)
		{
			if(item == null)
			{
				throw new ArgumentNullException("item");
			}

			if(Collection == null)
			{
				Collection = new T();
			}

			Collection.Add(item);
			OnCollectionChanged();
		}

		public virtual void Clear()
		{
			if(Collection == null)
			{
				Collection = new T();
			}
			else
			{
				bool collectionChanged = Collection.Count != 0;
				
				Collection.Clear();

				if(collectionChanged)
				{
					OnCollectionChanged();
				}
			}
		}

		public virtual bool Contains(V item)
		{
			if(item == null)
			{
				throw new ArgumentNullException("item");
			}

			if(Collection == null)
			{
				return false;
			}

			var enumerator = Collection.GetEnumerator();
			while(enumerator.MoveNext())
			{
				if(item.Equals(enumerator.Current))
				{
					return true;
				}
			}
			return false;
		}

		public virtual int IndexOf(V item)
		{
			if(item == null)
			{
				throw new ArgumentNullException("item");
			}

			if(Collection == null)
			{
				return -1;
			}

			var enumerator = Collection.GetEnumerator();
			int i = 0;
			while(enumerator.MoveNext())
			{
				if(item.Equals(enumerator.Current))
				{
					return i;
				}
				i++;
			}

			return -1;
		}

		public virtual void Insert(V item, int index)
		{
			if(item == null)
			{
				throw new ArgumentNullException("item");
			}

			if(Collection == null)
			{
				Collection = new T();
			}

			if(index < 0 || index >= Collection.Count)
			{
				throw new IndexOutOfRangeException($"Index out of range: {index}");
			}

			var enumerator = Collection.GetEnumerator();
			T newCollection = new T();
			int i = 0;
			while(enumerator.MoveNext())
			{
				if(index == i)
				{
					newCollection.Add(item);
				}
				newCollection.Add(enumerator.Current);
				i++;
			}

			Collection = newCollection;
			OnCollectionChanged();
		}

		public virtual bool Remove(V item)
		{
			if(item == null)
			{
				throw new ArgumentNullException("value");
			}

			if(Collection == null)
			{
				return false;
			}

			bool result = Collection.Remove(item);
			if(result)
			{
				OnCollectionChanged();
			}

			return result;
		}

		public virtual void OnCollectionChanged()
		{
			CollectionChanged?.Invoke();
		}

		public virtual IEnumerator<V> GetEnumerator()
		{
			return Collection.GetEnumerator();
		}

		public static implicit operator T(ScriptableICollectionValue<T, V> scriptableICollectionValue)
		{
			return scriptableICollectionValue.Collection;
		}
	}
}