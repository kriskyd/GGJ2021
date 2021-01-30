using UnityEngine;

public abstract class StanceSO : ScriptableObject, IStance
{
	public abstract Stance Stance { get; }

	public abstract void DeinitializeStance(Enemy enemy);
	public abstract void InitializeStance(Enemy enemy);
	public abstract void PerformStance(Enemy enemy);
}
