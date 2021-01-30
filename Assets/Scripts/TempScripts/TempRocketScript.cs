using SA.ScriptableData;
using System.Collections.Generic;
using UnityEngine;

namespace RocketSystem
{
	public class TempRocketScript : MonoBehaviour
	{
		[SerializeField]
		private Vector3Value rocketPosition;
		[SerializeField]
		private List<RocketPartSlot> partsPlacementPositions = new List<RocketPartSlot>();

		private Vector3 lastPosition;

		private void Awake()
		{
			rocketPosition.Value = transform.position;
		}
	}
}
