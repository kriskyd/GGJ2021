using UnityEngine;

namespace PlayerActions.Providers
{
	[CreateAssetMenu(fileName = "DropRocketPartActionProvider", menuName = "PlayerActions/Providers/DropRocketPartActionProvider")]
	public class DropRocketPartActionProvider : PlayerActionProvider
	{
		public override PlayerAction Provide()
		{
			return new DropRocketPartAction();
		}
	}
}