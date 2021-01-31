using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JunkPilesManager : MonoBehaviour
{
	[SerializeField]
	private List<JunkPile> junkPiles = new List<JunkPile>();

	private Dictionary<Enemy, JunkPile> enemyJunkPilePairs = new Dictionary<Enemy, JunkPile>();

	public static JunkPilesManager Instance;

	private void Awake()
	{
		Instance = this;
	}

	public JunkPile GetJunkPileForEnemy(Enemy enemy)
	{
		if(enemyJunkPilePairs.ContainsKey(enemy))
		{
			return enemyJunkPilePairs[enemy];
		}

		IEnumerable<JunkPile> pilesWithFreeSlot = junkPiles.Where(pile => pile.FreeSlot);

		if(pilesWithFreeSlot == null || pilesWithFreeSlot.Count() == 0)
		{
			return null;
		}

		JunkPile junkPile = pilesWithFreeSlot.ElementAt(Random.Range(0, pilesWithFreeSlot.Count()));
		enemyJunkPilePairs.Add(enemy, junkPile);

		enemy.Died += Enemy_Died;
		junkPile.RocketPartPlaced += JunkPile_RocketPartPlaced;

		return junkPile;
	}

	private void JunkPile_RocketPartPlaced(JunkPile junkPile)
	{
		var pairs = enemyJunkPilePairs.Where(pair => pair.Value == junkPile);

		List<Enemy> keys = new List<Enemy>();

		foreach(var pair in pairs)
		{
			keys.Add(pair.Key);
		}

		foreach(var key in keys)
		{
			RemoveJunkPileForEnemy(key);
		}
	}

	public void RemoveJunkPileForEnemy(Enemy enemy)
	{
		if(enemyJunkPilePairs.ContainsKey(enemy))
		{
			enemyJunkPilePairs.Remove(enemy);
		}
	}

	private void Enemy_Died(Enemy enemy)
	{
		RemoveJunkPileForEnemy(enemy);
	}
}

