using ObjectPooling;
using SA.ScriptableData;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
	[SerializeField]
	private TMP_Text nextWaveTimerText;
	[SerializeField]
	private int spawnDelayInSeconds = 30;
	[SerializeField]
	private List<EnemiesPool> enemiesPools = new List<EnemiesPool>();
	[SerializeField]
	private Vector3Value playerPosition;
	[SerializeField]
	private Vector3Value rocketPosition;

	[SerializeField]
	private List<EnemyPoolData> spawnedEnemies = new List<EnemyPoolData>();

	[Space()]
	[Header("Debug values")]
	[SerializeField]
	private Bounds spawnBounds = new Bounds(Vector3.zero, Vector3.up * 2);
	[SerializeField]
	private Bounds excludedSpawnBounds = new Bounds(Vector3.zero, Vector3.up);

	public Vector3 PlayerPosition => playerPosition.Value;
	public Vector3 RocketPosition => rocketPosition.Value;
	public List<EnemyPoolData> SpawnedEnemies => spawnedEnemies;

	public static EnemiesManager Instance { get; private set; }

	private Vector3 lastPlayerPosition;

	private void Awake()
	{
		Instance = this;
		lastPlayerPosition = Vector3.one * int.MaxValue;
	}

	private void Start()
	{
		playerPosition.ValueChanged += PlayerPosition_ValueChanged;
		PlayerPosition_ValueChanged();

		SpawnStartingEnemies();
		StartCoroutine(WavesSpawning());
	}

	private void OnDestroy()
	{
		playerPosition.ValueChanged -= PlayerPosition_ValueChanged;
	}

	private IEnumerator WavesSpawning()
	{
		float awaitingTime = spawnDelayInSeconds;

		while(true)
		{
			while(awaitingTime > 0f)
			{
				awaitingTime -= Time.deltaTime;
				int min = (int)(awaitingTime / 60);
				int sec = (int)(awaitingTime % 60);
				nextWaveTimerText.text = string.Format("{0:00}:{1:00}", min, sec);

				yield return null;
			}

			SpawnStartingEnemies();
			awaitingTime += spawnDelayInSeconds;
		}
	}

	public void DespawnEnemy(Enemy enemy)
	{
		int idx = spawnedEnemies.FindIndex(data => data.Enemy == enemy);

		spawnedEnemies.ElementAt(idx).ObjectPool.Despawn(enemy.gameObject);

		spawnedEnemies.RemoveAt(idx);
	}

	private void PlayerPosition_ValueChanged()
	{
		if(Vector3.Distance(lastPlayerPosition, PlayerPosition) < 2f)
		{
			return;
		}

		lastPlayerPosition = PlayerPosition;
		foreach(var spawnedEnemy in spawnedEnemies)
		{
			if(spawnedEnemy.Enemy.CurrentStance != Stance.Die)
			{
				if(spawnedEnemy.Enemy.HoldPart)
				{
					spawnedEnemy.Enemy.SetStance(Stance.PlacePartInJunkPile);
				}
				else if(Vector3.Distance(spawnedEnemy.Enemy.transform.position, rocketPosition) < Vector3.Distance(spawnedEnemy.Enemy.transform.position, playerPosition) && AnyPartAttachedToRocket())
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

	private bool AnyPartAttachedToRocket()
	{
		return GameManager.Instance.RocketScript.MountedPartsCount > 0;
	}

	public struct EnemyPoolData
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
	public void SpawnStartingEnemies()
	{
		foreach(var enemiesPool in enemiesPools)
		{
			enemiesPool.SpawnStartingEnemies(spawnBounds, excludedSpawnBounds);
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

