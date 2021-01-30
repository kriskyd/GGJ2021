using ObjectPooling;
using SA.ScriptableData;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemiesManager : MonoBehaviour
{
	[SerializeField]
	private List<ObjectPool> enemiesPools = new List<ObjectPool>();
	[SerializeField]
	private Vector3Value playerPosition;
	[SerializeField]
	private Vector3Value rocketPosition;

	[SerializeField]
	private List<EnemyPoolData> spawnedEnemies = new List<EnemyPoolData>();

	[Space()]
	[Header("Debug values")]
	[SerializeField]
	private int minEnemiesToSpawn = 3;
	[SerializeField]
	private int maxEnemiesToSpawn = 15;
	[SerializeField]
	private Bounds spawnBounds = new Bounds(Vector3.zero, Vector3.up * 2);
	[SerializeField]
	private Bounds excludedSpawnBounds = new Bounds(Vector3.zero, Vector3.up);

	public Vector3 PlayerPosition => playerPosition.Value;
	public Vector3 RocketPosition => rocketPosition.Value;

	private void Start()
	{
		playerPosition.ValueChanged += PlayerPosition_ValueChanged;

		SpawnRandomly();
	}

	private bool TrySpawnEnemy(ObjectPool enemiesPool, Vector3 position, out Enemy enemy)
	{
		enemy = null;

		if(enemiesPool.UsedObjectCount == enemiesPool.MaxPoolSize)
		{
			return false;
		}

		position = GetValidPosition(position);

		GameObject gameObject = enemiesPool.Spawn(position);

		enemy = gameObject.GetComponent<Enemy>();
		enemy.EnemiesManager = this;

		spawnedEnemies.Add(new EnemyPoolData(enemy, enemiesPool));

		return true;
	}

	private Vector3 GetValidPosition(Vector3 position)
	{
		float range = 0.05f;
		NavMeshHit hit = new NavMeshHit();

		while(!NavMesh.SamplePosition(position, out hit, range, NavMesh.AllAreas))
		{
			range += 0.05f;
		}

		return hit.position;
	}

	public void DespawnEnemy(Enemy enemy)
	{
		int idx = spawnedEnemies.FindIndex(data => data.Enemy == enemy);

		spawnedEnemies.ElementAt(idx).ObjectPool.Despawn(enemy.gameObject);

		spawnedEnemies.RemoveAt(idx);
	}

	private void PlayerPosition_ValueChanged()
	{
		foreach(var spawnedEnemy in spawnedEnemies)
		{
			if (spawnedEnemy.Enemy.CurrentStance != Stance.Die)
			{
				if (Vector3.Distance(spawnedEnemy.Enemy.transform.position, rocketPosition) < Vector3.Distance(spawnedEnemy.Enemy.transform.position, playerPosition))
				{
					spawnedEnemy.Enemy.SetStance(Stance.StealRocketPart);
				}
				else
				{
					spawnedEnemy.Enemy.SetStance(Stance.AttackPlayer);
				}
			}
		}
	}

	private struct EnemyPoolData
	{
		public Enemy Enemy { private set; get; }
		public ObjectPool ObjectPool { private set; get; }

		public EnemyPoolData(Enemy enemy, ObjectPool objectPool) : this()
		{
			Enemy = enemy;
			ObjectPool = objectPool;
		}
	}

	#region Debug
	public void SpawnRandomly()
	{
		foreach(var enemiesPool in enemiesPools)
		{
			int random = Random.Range(minEnemiesToSpawn, maxEnemiesToSpawn + 1);

			for(int i = 0; i < random; i++)
			{
				Vector3 randomPosition = Vector3.zero;

				while(excludedSpawnBounds.Contains(randomPosition))
				{
					float x = Random.Range(spawnBounds.center.x - spawnBounds.extents.x, spawnBounds.center.x + spawnBounds.extents.x);
					float z = Random.Range(spawnBounds.center.z - spawnBounds.extents.z, spawnBounds.center.z + spawnBounds.extents.z);
					randomPosition = Vector3.right * x + Vector3.forward * z;
				}

				TrySpawnEnemy(enemiesPool, randomPosition, out Enemy enemy);
			}
		}
	}

#if UNITY_EDITOR
	[ExecuteInEditMode()]
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube(excludedSpawnBounds.center, excludedSpawnBounds.size);
		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(spawnBounds.center, spawnBounds.size);
	}

#endif
	#endregion Debug
}

