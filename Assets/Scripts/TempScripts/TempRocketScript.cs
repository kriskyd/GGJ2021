using SA.ScriptableData;
using UnityEngine;

public class TempRocketScript : MonoBehaviour
{
	[SerializeField]
	private Vector3Value rocketPosition;

	private Vector3 lastPosition;

	private void Awake()
	{
		rocketPosition.Value = transform.position;
	}
}
