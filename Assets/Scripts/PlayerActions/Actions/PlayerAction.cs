namespace PlayerActions
{
	public abstract class PlayerAction
	{
		public abstract bool CanPerformAction { get; }
		public abstract void Perform();
	}
}