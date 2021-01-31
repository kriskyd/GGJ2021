using System.Collections;
using UnityEngine;

namespace SA.Coroutines
{
	public class Coroutines
	{
		private const string COROUTINE_OBJECT_HANDLER_NAME = "Coroutines Handler";

		#region MEMBERS

		private MonoBehaviour coroutineObjectHandler;

		#endregion

		#region PROPERTIES

		public MonoBehaviour CoroutineObjectHandler
		{
			get
			{
				if(coroutineObjectHandler == null)
				{
					Debug.LogError(COROUTINE_OBJECT_HANDLER_NAME + " has been destroyed. Initializing new object.");
					InitCoroutinesHandler();
				}

				return coroutineObjectHandler;
			}
		}

		#endregion

		#region METHODS

		public Coroutines()
		{
			InitCoroutinesHandler();
		}

		public Coroutine StartCoroutine(IEnumerator routine)
		{
			if(routine == null)
			{
				Debug.LogException(new System.ArgumentNullException("routine"));
				return null;
			}

			return CoroutineObjectHandler.StartCoroutine(routine);
		}

		public void StopCoroutine(Coroutine routine)
		{
			if(routine == null)
			{
				Debug.LogException(new System.ArgumentNullException("routine"));
				return;
			}

			CoroutineObjectHandler.StopCoroutine(routine);
		}

		public Coroutine StartWaitForSeconds(float waitDuration, System.Action action)
		{
			return StartCoroutine(WaitForSecondsCoroutine(waitDuration, action));
		}

		public Coroutine StartWaitForSecondsRealtime(float waitDuration, System.Action action)
		{
			return StartCoroutine(WaitForSecondsRealtimeCoroutine(waitDuration, action));
		}

		public Coroutine StartWaitForEndOfFrame(System.Action action)
		{
			return StartCoroutine(WaitForEndOfFrameCoroutine(action));
		}

		public Coroutine StartWaitForNextFrame(System.Action action)
		{
			return StartCoroutine(WaitForNextFrameCoroutine(action));
		}

		public Coroutine StartWaitForFrames(int frameCount, System.Action action)
		{
			return StartCoroutine(WaitForFramesCoroutine(frameCount, action));
		}

		public Coroutine StartWaitForPredicate(System.Func<bool> predicate, System.Action action, float timeBetweenChecks = 0f)
		{
			return StartCoroutine(WaitForPredicateCoroutine(predicate, action, timeBetweenChecks));
		}

		private IEnumerator WaitForSecondsCoroutine(float waitDuration, System.Action action)
		{
			yield return Yielders.WaitForSeconds(waitDuration);
			action();
		}

		private IEnumerator WaitForSecondsRealtimeCoroutine(float waitDuration, System.Action action)
		{
			yield return Yielders.WaitForSecondsRealtime(waitDuration);
			action();
		}

		private IEnumerator WaitForEndOfFrameCoroutine(System.Action action)
		{
			yield return Yielders.WaitForEndOfFrame;
			action();
		}

		private IEnumerator WaitForNextFrameCoroutine(System.Action action)
		{
			yield return null;
			action();
		}

		private IEnumerator WaitForFramesCoroutine(int frameCount, System.Action action)
		{
			if(frameCount >= 0)
			{
				for(int i = 0; i < frameCount; i++)
				{
					yield return null;
				}
				action();
			}
		}

		private IEnumerator WaitForPredicateCoroutine(System.Func<bool> predicate, System.Action action, float timeBetweenChecks)
		{
			while(!predicate())
				yield return Yielders.WaitForSecondsRealtime(timeBetweenChecks);
			action();
		}

		private void InitCoroutinesHandler()
		{
			GameObject gameObject = new GameObject(COROUTINE_OBJECT_HANDLER_NAME);
			coroutineObjectHandler = gameObject.AddComponent<CoroutinesHandler>();
		}

		#endregion
	}
}