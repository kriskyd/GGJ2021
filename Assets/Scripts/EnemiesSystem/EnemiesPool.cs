using ObjectPooling;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemiesPool : MonoBehaviour
{
	[SerializeField]
	private ObjectPool enemiesPool;

	[SerializeField]
	private int minEnemiesToSpawn = 3;
	[SerializeField]
	private int maxEnemiesToSpawn = 15;

	public void SpawnStartingEnemies(Bounds spawnBounds, Bounds excludedSpawnBounds)
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

		EnemiesManager.Instance.SpawnedEnemies.Add(new EnemiesManager.EnemyPoolData(enemy, enemiesPool));

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
}
