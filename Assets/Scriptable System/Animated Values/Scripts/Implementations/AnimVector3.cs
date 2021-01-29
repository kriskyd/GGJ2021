using System;
using UnityEngine;
using UnityEngine.Events;

namespace SA.AnimatedValues
{
	[Serializable]
	public class AnimVector3 : AnimValueBase<Vector3>
	{
		[SerializeField]
		private Vector3 m_Value;

		public AnimVector3() : base(Vector3.zero) { }

		public AnimVector3(Vector3 value) : base(value) { }

		public AnimVector3(UnityAction callback) : base(Vector3.zero, callback) { }

		public AnimVector3(Vector3 value, UnityAction callback) : base(value, callback) { }

		protected override Vector3 GetValue()
		{
			m_Value = Vector3.Lerp(Start, Target, SmoothLerpValue);
			return m_Value;
		}
	}
}