using UnityEngine;

[CreateAssetMenu(fileName = "IdleStance", menuName = "Stances/Idle")]
public class IdleStance : StanceSO
{
    private const string deadBoolName = "Dead";
    public override Stance Stance => Stance.Idle;

    public override void InitializeStance(Enemy enemy)
    {
    }

    public override void PerformStance(Enemy enemy)
    {
    }

    public override void DeinitializeStance(Enemy enemy)
    {
    }
}
