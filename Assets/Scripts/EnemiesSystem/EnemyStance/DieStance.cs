using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "DieStance", menuName = "Stances/Die")]
public class DieStance : StanceSO
{
    private const string deadBoolName = "Dead";
    public override Stance Stance => Stance.Die;

    public override void InitializeStance(Enemy enemy)
    {
        enemy.Collider.enabled = false;
        enemy.NavMeshAgent.isStopped = true;
        enemy.NavMeshAgent.enabled = false;
        enemy.Animator.SetBool(deadBoolName, true);
    }

    public override void PerformStance(Enemy enemy)
    {
        if (!enemy.IsCollapsingTweenSet)
        {
            enemy.SetCollapsingTween();
        }
    }

    public override void DeinitializeStance(Enemy enemy)
    {
        enemy.NavMeshAgent.enabled = true;
        enemy.Animator.SetBool(deadBoolName, false);
        enemy.Collider.enabled = true;
    }
}
