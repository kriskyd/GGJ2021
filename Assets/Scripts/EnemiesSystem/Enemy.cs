using ObjectPooling;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IRestorable
{
	[SerializeField]
	private NavMeshAgent navMeshAgent;
	[SerializeField]
	private StanceSO attackPlayerStance;
	[SerializeField]
	private StanceSO stealRocketPartStance;

	private IStance currentStance = null;

	public NavMeshAgent NavMeshAgent => navMeshAgent;

	private void Awake()
	{
		navMeshAgent.enabled = false;
	}

	private void OnEnable()
	{
		navMeshAgent.enabled = true;
	}

	private void Update()
	{
		if(currentStance == null)
		{
			return;
		}

		currentStance.PerformStance(this);
	}

	public void Restore()
	{
		navMeshAgent.enabled = false;
	}

	public void SetStance(EnemiesManager enemiesManager, Stance stance)
	{
		if(currentStance != null && currentStance.Stance == stance)
		{
			return;
		}

		currentStance?.DeinitializeStance(this);
		currentStance = null;

		switch(stance)
		{
			case Stance.AttackPlayer:
				currentStance = attackPlayerStance;
				break;
			case Stance.StealRocketPart:
				currentStance = stealRocketPartStance;
				break;
			default:
				break;
		}

		currentStance.InitializeStance(this);
	}
}

public enum Stance
{
	AttackPlayer, StealRocketPart
}
