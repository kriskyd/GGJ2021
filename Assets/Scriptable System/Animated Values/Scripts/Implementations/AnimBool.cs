using System;
using UnityEngine;
using UnityEngine.Events;

namespace SA.AnimatedValues
{
	[Serializable]
	public class AnimBool : AnimValueBase<bool>
	{
		[SerializeField]
		private float m_Value;

		public AnimBool() : base(false) { }

		public AnimBool(bool value) : base(value) { }

		public AnimBool(UnityAction callback) : base(false, callback) { }

		public AnimBool(bool value, UnityAction callback) : base(value, callback) { }

		public float FadedValue
		{
			get
			{
				GetValue();
				return m_Value;
			}
		}

		protected override bool GetValue()
		{
			float startValue = Target ? 0f : 1f;
			float endValue = 1f - startValue;

			m_Value = Mathf.Lerp(startValue, endValue, SmoothLerpValue);

			return m_Value > .5f;
		}

		public float Fade(float from, float to)
		{
			return Mathf.Lerp(from, to, FadedValue);
		}
	}
}