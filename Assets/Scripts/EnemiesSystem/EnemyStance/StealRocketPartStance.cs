using SA.ScriptableData;
using UnityEngine;

[CreateAssetMenu(fileName = "StealRocketPartStance", menuName = "Stances/StealRocketPart")]
public class StealRocketPartStance : StanceSO
{
	[SerializeField]
	private Vector3Value rocketPosition;

	public override Stance Stance => Stance.StealRocketPart;

	public override void InitializeStance(Enemy enemy)
	{
		//throw new System.NotImplementedException();
	}

	public override void PerformStance(Enemy enemy)
	{
		enemy.NavMeshAgent.SetDestination(rocketPosition);
	}

	public override void DeinitializeStance(Enemy enemy)
	{
		//throw new System.NotImplementedException();
	}
}
