using UnityEngine;

namespace PlayerActions.Providers
{
	[CreateAssetMenu(fileName = "MountRocketPartActionProvider", menuName = "PlayerActions/Providers/MountRocketPartActionProvider")]
	public class MountRocketPartActionProvider : PlayerActionProvider
	{
		public override PlayerAction Provide()
		{
			return new MountRocketPartAction();
		}
	}
}