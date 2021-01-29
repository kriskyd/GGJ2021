using ObjectPooling;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IRestorable
{
	[SerializeField]
	private NavMeshAgent navMeshAgent;

	private void Awake()
	{
		navMeshAgent.enabled = false;
	}

	private void OnEnable()
	{
		navMeshAgent.enabled = true;
	}

	public void Restore()
	{
		navMeshAgent.enabled = false;
	}

	public void FollowPlayer(Vector3 playerPosition)
	{
		navMeshAgent.SetDestination(playerPosition);
	}
}
