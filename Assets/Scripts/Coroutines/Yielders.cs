using System.Collections.Generic;
using UnityEngine;

namespace SA.Coroutines
{
	public static class Yielders
	{
		#region MEMBERS

		private static Dictionary<float, WaitForSeconds> waitForSecondsDict = new Dictionary<float, WaitForSeconds>();

		private static Dictionary<float, WaitForSecondsRealtime> waitForSecondsRealtimeDict = new Dictionary<float, WaitForSecondsRealtime>();

		#endregion

		#region PROPERTIES

		public static WaitForEndOfFrame WaitForEndOfFrame { get; } = new WaitForEndOfFrame();

		public static WaitForFixedUpdate WaitForFixedUpdate { get; } = new WaitForFixedUpdate();

		#endregion

		#region METHODS

		public static WaitForSeconds WaitForSeconds(float duration)
		{
			if(waitForSecondsDict.TryGetValue(duration, out var yielder))
				return yielder;
			else
			{
				var newYielder = new WaitForSeconds(duration);
				waitForSecondsDict.Add(duration, newYielder);
				return newYielder;
			}
		}

		public static WaitForSecondsRealtime WaitForSecondsRealtime(float duration)
		{
			if(waitForSecondsRealtimeDict.TryGetValue(duration, out var yielder))
				return yielder;
			else
			{
				var newYielder = new WaitForSecondsRealtime(duration);
				waitForSecondsRealtimeDict.Add(duration, newYielder);
				return newYielder;
			}
		}

		#endregion
	}
}