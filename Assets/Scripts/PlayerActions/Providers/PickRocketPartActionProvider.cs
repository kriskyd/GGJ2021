using SA.ScriptableData;
using SA.ScriptableData.Collection;
using UnityEngine;

namespace PlayerActions.Providers
{
	[CreateAssetMenu(fileName = "PickRocketPartActionProvider", menuName = "PlayerActions/Providers/PickRocketPartActionProvider")]
	public class PickRocketPartActionProvider : PlayerActionProvider
	{
		[SerializeField]
		private float maxPickUpDistance = 2f;
		[SerializeField]
		private Vector3Value playerPosition;
		[SerializeField]
		private ListVector3Value rocketPartPositions;
		[SerializeField]
		private RocketPartHolderValue rocketPartHolderValue;

		public override PlayerAction Provide()
		{
			return new PickRocketPartAction(maxPickUpDistance, playerPosition, rocketPartPositions, rocketPartHolderValue);
		}
	}
}