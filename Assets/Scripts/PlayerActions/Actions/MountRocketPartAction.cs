using SA.ScriptableData;
using UnityEngine;

namespace PlayerActions
{
	public class MountRocketPartAction : PlayerAction
	{
		private float maxMountDistance;
		private RocketPartHolderValue rocketPartHolderValue;
		private RocketValue rocketValue;
		private Vector3Value playerPosition;
		private RocketSystem.TempRocketScript rocketScript;

		public MountRocketPartAction(string actionName, float maxMountDistance, RocketPartHolderValue rocketPartHolderValue, RocketValue rocketValue, Vector3Value playerPosition) : base(actionName)
		{
			this.maxMountDistance = maxMountDistance;
			this.rocketPartHolderValue = rocketPartHolderValue;
			this.rocketValue = rocketValue;
			this.playerPosition = playerPosition;
			rocketScript = /*this.rocketValue.Value ??*/ GameManager.Instance.RocketScript;
		}

		public override bool CanPerformAction => rocketPartHolderValue.Value.PlayerHoldsPart && (Vector3.Distance(playerPosition, rocketScript.transform.position) < maxMountDistance);

		public override void Perform()
		{
			rocketValue.Value.MountRocketPart(rocketPartHolderValue.Value.DropRocketPart());
		}
	}
}