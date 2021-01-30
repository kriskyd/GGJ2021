using UnityEngine;

namespace PlayerActions
{
	public abstract class PlayerActionProvider : ScriptableObject
	{
		[SerializeField]
		private string actionName;

		public string ActionName => actionName;
		public abstract PlayerAction Provide();
	}
}