using ObjectPooling;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IRestorable
{
	[SerializeField]
	private Collider enemyCollider;
	[SerializeField]
	private Animator animator;
	[SerializeField]
	private NavMeshAgent navMeshAgent;
	[SerializeField]
	private StanceSO attackPlayerStance;
	[SerializeField]
	private StanceSO stealRocketPartStance;
	[SerializeField]
	private StanceSO dieStance;
	[SerializeField]
	private StanceSO idleStance;

	private IStance currentStance = null;

	[SerializeField]
	private int maxHP;
	private int _hp;
	public int HP { get => _hp; }

	public Stance CurrentStance => currentStance != null ? currentStance.Stance : Stance.Idle;
	public NavMeshAgent NavMeshAgent => navMeshAgent;
	public Animator Animator => animator;
	public Collider Collider => enemyCollider;


	private const string hitTriggerName = "Hit";

	private void Awake()
	{
		navMeshAgent.enabled = false;
		_hp = maxHP;
		SetStance(Stance.Idle);
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
		_hp = maxHP;
		SetStance(Stance.Idle);
	}

	public void SetStance(Stance stance)
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
			case Stance.Die:
				currentStance = dieStance;
				break;
			case Stance.Idle:
				currentStance = idleStance;
				break;
			default:
				break;
		}

		currentStance.InitializeStance(this);
	}

	public void GotHit(int damage)
	{
		if (_hp - damage <= 0)
		{
			SetStance(Stance.Die);
		}
		else
		{
			animator.SetTrigger(hitTriggerName);
			_hp -= damage;
		}
	}
}

public enum Stance
{
	Idle, AttackPlayer, StealRocketPart, Die
}
