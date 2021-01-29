using UnityEngine;

public abstract class TemporalSingleton<T> : MonoBehaviour where T : Component
{
	public static T Instance { get; private set; }

	private void Awake()
	{
		Instance = GetComponent<T>();
		Initialize();
	}

	protected virtual void Initialize()
	{
	}

	private void OnDestroy()
	{
		if(this == Instance)
		{
			OnSingletonDestroy();
			Instance = null;
		}
	}

	protected virtual void OnSingletonDestroy()
	{
	}
}