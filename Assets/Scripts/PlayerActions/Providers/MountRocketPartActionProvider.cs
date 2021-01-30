using SA.ScriptableData;
using UnityEngine;

namespace PlayerActions.Providers
{
	[CreateAssetMenu(fileName = "MountRocketPartActionProvider", menuName = "PlayerActions/Providers/MountRocketPartActionProvider")]
	public class MountRocketPartActionProvider : PlayerActionProvider
	{
		[SerializeField]
		private float maxMountDistance = 2f;
		[SerializeField]
		private RocketPartHolderValue rocketPartHolderValue;
		[SerializeField]
		private RocketValue rocketValue;
		[SerializeField]
		private Vector3Value playerPosition;

		public override PlayerAction Provide()
		{
			return new MountRocketPartAction(ActionName, maxMountDistance, rocketPartHolderValue, rocketValue, playerPosition);
		}
	}
}