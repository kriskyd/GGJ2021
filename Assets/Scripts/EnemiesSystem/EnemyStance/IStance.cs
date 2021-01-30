public interface IStance
{
	Stance Stance { get; }
	void InitializeStance(Enemy enemy);
	void PerformStance(Enemy enemy);
	void DeinitializeStance(Enemy enemy);
}
