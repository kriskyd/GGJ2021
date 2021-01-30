using System.Collections.Generic;

namespace SA.ScriptableData.Collection
{
	public abstract class ScriptableIListValue<T, V> : ScriptableICollectionValue<T, V>
		where T : IList<V>, new()
	{
		public T List
		{
			get => Collection;
			protected set => Collection = value;
		}

		public override T Values => base.Values;

		public virtual V this[int index]
		{
			get => List[index];
			set
			{
				List[index] = value;
				OnCollectionChanged();
			}
		}

		public override void Add(V item)
		{
			List.Add(item);
			OnCollectionChanged();
		}

		public override void Clear()
		{
			List.Clear();
			OnCollectionChanged();
		}

		public override bool Contains(V item)
		{
			return List.Contains(item);
		}

		public override int IndexOf(V item)
		{
			return List.IndexOf(item);
		}

		public override void Insert(V item, int index)
		{
			List.Insert(index, item);
			OnCollectionChanged();
		}

		public override bool Remove(V item)
		{
			bool result = List.Remove(item);
			if(result)
			{
				OnCollectionChanged();
			}

			return result;
		}

		public virtual void RemoveAt(int index)
		{
			List.RemoveAt(index);
			OnCollectionChanged();
		}

		public override void OnCollectionChanged()
		{
			base.OnCollectionChanged();
		}

		public override IEnumerator<V> GetEnumerator()
		{
			return List.GetEnumerator();
		}
	}
}