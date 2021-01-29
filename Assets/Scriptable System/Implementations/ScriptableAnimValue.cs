using SA.AnimatedValues;

namespace SA.ScriptableData.Animated
{
	public abstract class ScriptableAnimValue<T, V> : ScriptableValue<T>
		where T : AnimValueBase<V>
	{
		public T AnimValue => base.Value;

		public new V Value
		{
			get { return AnimValue.Value; }
			set { AnimValue.Value = value; }
		}

		public V Target
		{
			get { return AnimValue.Target; }
		}

		public new event AnimValueChangedEvent<V> ValueChanged
		{
			add
			{
				AnimValue.OnValueChangedEvent += value;
			}
			remove
			{
				AnimValue.OnValueChangedEvent -= value;
			}
		}

	}
}