using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "PlacePartInJunkPileStance", menuName = "Stances/PlacePartInJunkPileStance")]
public class PlacePartInJunkPileStance : StanceSO
{
	[SerializeField]
	private float placeDistance = 2f;

	private const string attackStateBoolName = "AttackState";
	private const string walkBoolName = "Walk";

	public override Stance Stance => Stance.PlacePartInJunkPile;

	public override void InitializeStance(Enemy enemy)
	{
		JunkPilesManager.Instance.GetJunkPileForEnemy(enemy);
	}

	public override void PerformStance(Enemy enemy)
	{
		JunkPile junkPile = JunkPilesManager.Instance.GetJunkPileForEnemy(enemy);

		if(Vector3.Distance(junkPile.transform.position, enemy.transform.position) <= placeDistance)
		{
			enemy.Animator.SetBool(walkBoolName, false);
			enemy.transform.DOLookAt(junkPile.transform.position, 0.1f, AxisConstraint.Y, Vector3.up);
			enemy.NavMeshAgent.isStopped = true;
			enemy.TryPlacePart(junkPile);

		}
		else
		{
			enemy.NavMeshAgent.isStopped = false;
			enemy.NavMeshAgent.SetDestination(junkPile.transform.position);
			enemy.Animator.SetBool(walkBoolName, true);
		}
	}

	public override void DeinitializeStance(Enemy enemy)
	{
		JunkPilesManager.Instance.RemoveJunkPileForEnemy(enemy);
	}
}
