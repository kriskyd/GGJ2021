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

		public override bool CanPerformAction => !rocketPartHolderValue.Value.PlayerHoldsPart && RocketPart.AllRocketParts.Any(part => Vector3.Distance(rocketPartPositions[part.Idx], playerPosition) < maxPickUpDistance && part.gameObject.activeInHierarchy);

		public override void Perform()
		{
			var parts = RocketPart.AllRocketParts.Where((rpart) => rpart.gameObject.activeInHierarchy).OrderBy(part => Vector3.Distance(part.transform.position, playerPosition));

			if (parts != null && parts.Count() > 0)
			{
				RocketPart rocketPart = parts.First();
				rocketPartHolderValue.Value.PickUpRocketPart(rocketPart);
			}
		}
	}
}