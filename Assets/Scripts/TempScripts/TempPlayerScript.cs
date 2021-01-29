using SA.ScriptableData;
using UnityEngine;

public class TempPlayerScript : MonoBehaviour
{
	[SerializeField]
	private Vector3Value playerPosition;
	[SerializeField]
	private Vector3Value rocketPosition;

	private Vector3 lastPosition;

	private void Awake()
	{
		lastPosition = transform.position;
	}

	private void Update()
	{
		if(lastPosition != transform.position)
		{
			playerPosition.Value = lastPosition = transform.position;
		}
	}

}
