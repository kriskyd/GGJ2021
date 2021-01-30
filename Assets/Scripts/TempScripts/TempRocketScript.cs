using SA.ScriptableData;
using System.Collections.Generic;
using UnityEngine;

namespace RocketSystem
{
	public class TempRocketScript : MonoBehaviour
	{
		[SerializeField]
		private RocketValue rocketValue;
		[SerializeField]
		private Vector3Value rocketPosition;

		private Vector3 lastPosition;

		private void Awake()
		{
			rocketValue.Value = this;
			rocketPosition.Value = transform.position;
		}

		internal void MountRocketPart(RocketPart rocketPart)
		{
			rocketPart.gameObject.SetActive(false);
			rocketPart.RocketPartData.RocketPartSlot.PlaceRocketPart(rocketPart);
		}
	}
}
