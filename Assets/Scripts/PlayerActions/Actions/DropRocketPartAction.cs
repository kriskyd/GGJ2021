namespace PlayerActions
{
	public class DropRocketPartAction : PlayerAction
	{
		private RocketPartHolderValue rocketPartHolderValue;

		public DropRocketPartAction(RocketPartHolderValue rocketPartHolderValue)
		{
			this.rocketPartHolderValue = rocketPartHolderValue;
		}

		public override bool CanPerformAction => rocketPartHolderValue.Value.PlayerHoldsPart;

		public override void Perform()
		{
			rocketPartHolderValue.Value.DropRocketPart();
		}
	}
}