using System.Collections;
using UnityEngine;

public static class ScriptableValueUtils
{
	public static class Coroutines
	{
		private const string COROUTINE_OBJECT_HANDLER_NAME = "Scriptable Value Coroutine Handler";

		private static MonoBehaviour coroutineObjectHandler;

		public static MonoBehaviour CoroutineObjectHandler
		{
			get
			{
				if(coroutineObjectHandler == null)
				{
					GameObject gameObject = new GameObject(COROUTINE_OBJECT_HANDLER_NAME);
					coroutineObjectHandler = gameObject.AddComponent<ScriptableValueCoroutineHandler>();
				}

				return coroutineObjectHandler;
			}
		}

		public static Coroutine StartCoroutine(IEnumerator routine)
		{
			if(routine == null)
			{
				Debug.LogException(new System.ArgumentNullException("routine"));
				return null;
			}

			return CoroutineObjectHandler?.StartCoroutine(routine);
		}

		public static void StopCoroutine(Coroutine routine)
		{
			if(routine == null)
			{
				Debug.LogException(new System.ArgumentNullException("routine"));
				return;
			}

			CoroutineObjectHandler?.StopCoroutine(routine);
		}

	}
}
