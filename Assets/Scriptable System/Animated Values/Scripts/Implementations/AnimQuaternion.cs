using System;
using UnityEngine;
using UnityEngine.Events;

namespace SA.AnimatedValues
{
	[Serializable]
	public class AnimQuaternion : AnimValueBase<Quaternion>
	{
		[SerializeField]
		private Quaternion m_Value;

		public AnimQuaternion() : base(Quaternion.identity) { }

		public AnimQuaternion(Quaternion value) : base(value) { }

		public AnimQuaternion(UnityAction callback) : base(Quaternion.identity, callback) { }

		public AnimQuaternion(Quaternion value, UnityAction callback) : base(value, callback) { }

		protected override Quaternion GetValue()
		{
			m_Value = Quaternion.Slerp(Start, Target, SmoothLerpValue);
			return m_Value;
		}
	}
}