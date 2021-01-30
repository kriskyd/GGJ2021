using System.Collections.Generic;
using UnityEngine;

namespace ObjectPooling
{
	public class ObjectPoolManager : TemporalSingleton<ObjectPoolManager>
	{
		private Dictionary<string, ObjectPool> allPools = new Dictionary<string, ObjectPool>();

		public void RegisterPool(string poolID, ObjectPool objectPool)
		{
			if(allPools.ContainsKey(poolID))
			{
				Debug.LogError($"Object pool with ID {poolID} already exists");
			}
			else
			{
				allPools.Add(poolID, objectPool);
			}
		}

		/// <summary>
		/// Get reference to object pool with <paramref name="poolID"/>
		/// </summary>
		/// <param name="poolID"></param>
		/// <returns> Returns object pool with <paramref name="poolID"/> if it's registered, otherwise returns null.</returns>
		public ObjectPool GetPool(string poolID)
		{
			if(allPools.TryGetValue(poolID, out var objectPool))
			{
				return objectPool;
			}
			else
			{
				return null;
			}
		}
	}
}