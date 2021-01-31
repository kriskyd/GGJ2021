using UnityEngine;
using SA.ScriptableData;
using DG.Tweening;

[CreateAssetMenu(fileName = "AttackPlayerStance", menuName = "Stances/AttackPlayer")]
public class AttackPlayerStance : StanceSO
{
    [SerializeField]
    private Vector3Value playerPosition;
    [SerializeField]
    private float attackDistance = 1.0f;

    private const string attackStateBoolName = "AttackState";
    private const string walkBoolName = "Walk";
    public override Stance Stance => Stance.AttackPlayer;

    public override void InitializeStance(Enemy enemy)
    {
        enemy.Animator.SetBool(attackStateBoolName, true);
    }

    public override void PerformStance(Enemy enemy)
    {
        if (Vector3.Distance(playerPosition.Value, enemy.transform.position) <= attackDistance)
        {
            enemy.Animator.SetBool(walkBoolName, false);
            enemy.transform.DOLookAt(playerPosition.Value, 0.1f, AxisConstraint.Y, Vector3.up);
            enemy.NavMeshAgent.isStopped = true;
            if (Time.time - enemy.LastAttackTime > enemy.AttackCooldown)
            {
                enemy.PerformAttack(GameManager.Instance.PlayerController);
            }
        }
        else
        {
            enemy.NavMeshAgent.isStopped = false;
            enemy.NavMeshAgent.SetDestination(playerPosition);
            enemy.Animator.SetBool(walkBoolName, true);
        }
    }

    public override void DeinitializeStance(Enemy enemy)
    {
        enemy.Animator.SetBool(attackStateBoolName, false);
    }
}
