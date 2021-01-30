using System;
using UnityEngine;
using UnityEngine.Events;

namespace SA.AnimatedValues
{
	[Serializable]
	public class AnimFloat : AnimValueBase<float>
	{
		[SerializeField]
		private float m_Value;

		public AnimFloat() : base(0f) { }

		public AnimFloat(float value) : base(value) { }

		public AnimFloat(UnityAction callback) : base(0f, callback) { }

		public AnimFloat(float value, UnityAction callback) : base(value, callback) { }

		protected override float GetValue()
		{
			m_Value = Mathf.Lerp(Start, Target, SmoothLerpValue);
			return m_Value;
		}
	}
}