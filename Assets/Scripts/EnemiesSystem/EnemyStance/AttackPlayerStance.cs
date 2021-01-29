using SA.ScriptableData;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackPlayerStance", menuName = "Stances/AttackPlayer")]
public class AttackPlayerStance : StanceSO
{
	[SerializeField]
	private Vector3Value playerPosition;

	public override Stance Stance => Stance.AttackPlayer;

	public override void InitializeStance(Enemy enemy)
	{
		//throw new System.NotImplementedException();
	}

	public override void PerformStance(Enemy enemy)
	{
		enemy.NavMeshAgent.SetDestination(playerPosition);
	}

	public override void DeinitializeStance(Enemy enemy)
	{
		//throw new System.NotImplementedException();
	}
}
