using UnityEngine;

namespace PlayerActions.Providers
{
	[CreateAssetMenu(fileName = "DropRocketPartActionProvider", menuName = "PlayerActions/Providers/DropRocketPartActionProvider")]
	public class DropRocketPartActionProvider : PlayerActionProvider
	{
		[SerializeField]
		private RocketPartHolderValue rocketPartHolderValue;

		public override PlayerAction Provide()
		{
			return new DropRocketPartAction(rocketPartHolderValue);
		}
	}
}