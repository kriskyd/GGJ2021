using DG.Tweening;
using SA.ScriptableData;
using UnityEngine;

[CreateAssetMenu(fileName = "StealRocketPartStance", menuName = "Stances/StealRocketPart")]
public class StealRocketPartStance : StanceSO
{
	[SerializeField]
	private Vector3Value rocketPosition;
	[SerializeField]
	private float attackDistance = 1.0f;

	private const string attackStateBoolName = "AttackState";
	private const string walkBoolName = "Walk";

	public override Stance Stance => Stance.StealRocketPart;

	public override void InitializeStance(Enemy enemy)
	{
		//throw new System.NotImplementedException();
	}

	public override void PerformStance(Enemy enemy)
	{
		if(Vector3.Distance(rocketPosition.Value, enemy.transform.position) <= attackDistance)
		{
			enemy.Animator.SetBool(walkBoolName, false);
			enemy.transform.DOLookAt(rocketPosition.Value, 0.1f, AxisConstraint.Y, Vector3.up);
			enemy.NavMeshAgent.isStopped = true;
			if(Time.time - enemy.LastAttackTime > enemy.AttackCooldown)
			{
				enemy.PerformAttack(GameManager.Instance.RocketScript);
			}
		}
		else
		{
			enemy.NavMeshAgent.isStopped = false;
			enemy.NavMeshAgent.SetDestination(rocketPosition);
			enemy.Animator.SetBool(walkBoolName, true);
		}
		//enemy.NavMeshAgent.SetDestination(rocketPosition);
	}

	public override void DeinitializeStance(Enemy enemy)
	{
		//throw new System.NotImplementedException();
	}
}
