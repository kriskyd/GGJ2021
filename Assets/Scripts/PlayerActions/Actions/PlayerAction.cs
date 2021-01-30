namespace PlayerActions
{
	public abstract class PlayerAction
	{
		public string ActionName { get; private set; }

		public PlayerAction(string actionName)
		{
			ActionName = actionName;
		}

		public abstract bool CanPerformAction { get; }
		public abstract void Perform();
	}
}