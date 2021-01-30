using RocketSystem;
using SA.ScriptableData;
using SA.ScriptableData.Collection;
using System.Linq;
using UnityEngine;

namespace PlayerActions
{
	public class PickRocketPartAction : PlayerAction
	{
		private float maxPickUpDistance = 2f;
		private Vector3Value playerPosition;
		private ListVector3Value rocketPartPositions;
		private RocketPartHolderValue rocketPartHolderValue;

		public PickRocketPartAction(string actionName, float maxPickUpDistance, Vector3Value playerPosition, ListVector3Value rocketPartPositions, RocketPartHolderValue rocketPartHolderValue) : base(actionName)
		{
			this.maxPickUpDistance = maxPickUpDistance;
			this.playerPosition = playerPosition;
			this.rocketPartPositions = rocketPartPositions;
			this.rocketPartHolderValue = rocketPartHolderValue;
		}

		public override bool CanPerformAction => !rocketPartHolderValue.Value.PlayerHoldsPart && rocketPartPositions.Value.Any(rocketPartPosition => Vector3.Distance(rocketPartPosition, playerPosition) < maxPickUpDistance);

		public override void Perform()
		{
			RocketPart rocketPart = RocketPart.AllRocketParts.OrderBy(part => Vector3.Distance(part.transform.position, playerPosition)).First();

			rocketPartHolderValue.Value.PickUpRocketPart(rocketPart);
		}
	}
}