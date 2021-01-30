using UnityEngine;

namespace PlayerActions
{
	public abstract class PlayerActionProvider : ScriptableObject
	{
		public abstract PlayerAction Provide();
	}
}