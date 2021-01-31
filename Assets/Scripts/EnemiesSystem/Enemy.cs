using DG.Tweening;
using ObjectPooling;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IRestorable
{
	[SerializeField]
	private int damageDealtToPlayer;
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

	[SerializeField]
	private int maxHP;

	public float LastAttackTime { get; private set; }
	public float AttackCooldown { get; private set; } = 2.0f;
	public int DamageDealtToPlayer { get => damageDealtToPlayer; }
	public EnemiesManager EnemiesManager { private get; set; }

	private IStance currentStance = null;

	private Tween collapsingTween;


	private int _hp;

	public int HP { get => _hp; }

	public Stance CurrentStance => currentStance != null ? currentStance.Stance : Stance.Idle;
	public NavMeshAgent NavMeshAgent => navMeshAgent;
	public Animator Animator => animator;
	public Collider Collider => enemyCollider;

	public bool IsCollapsingTweenSet => collapsingTween != null;

	private const string hitTriggerName = "Hit";
	private const string attackTriggerName = "Attack";

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
        if (collapsingTween != null)
        {
			collapsingTween.Kill();
			collapsingTween = null;
		}
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

	public void DespawnEnemy()
    {
		if (EnemiesManager != null) EnemiesManager.DespawnEnemy(this);
    }

	public void SetCollapsingTween()
    {
		if (collapsingTween != null) collapsingTween.Kill();
		collapsingTween = transform.DOMoveY(-50.0f, 10.0f);
		collapsingTween.SetDelay(3.0f);
		collapsingTween.onComplete = () => { DespawnEnemy(); };
	}

	public void PerformAttack()
	{
		Animator.SetTrigger(attackTriggerName);
		GameManager.Instance.PlayerController.GotHit(DamageDealtToPlayer);
		LastAttackTime = Time.time; 
	}
}

public enum Stance
{
	Idle, AttackPlayer, StealRocketPart, Die
}
