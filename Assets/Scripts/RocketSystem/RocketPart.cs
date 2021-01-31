using SA.ScriptableData.Collection;
using System.Collections.Generic;
using UnityEngine;

namespace RocketSystem
{
	public class RocketPart : MonoBehaviour
	{
		public static List<RocketPart> AllRocketParts = new List<RocketPart>();

		[SerializeField]
		private RocketPartData rocketPartData;
		[SerializeField]
		private Outline outline;
		[SerializeField]
		private ListVector3Value rocketPartPositions;

		private int idx = -1;
		private Vector3 lastPosition;

		public RocketPartData RocketPartData => rocketPartData;

		public bool IsRepaired => !gameObject.activeInHierarchy;

		private void Awake()
		{
			AllRocketParts.Add(this);
		}

		private void OnDestroy()
		{
			AllRocketParts.Remove(this);
		}

		private void Start()
		{
			rocketPartData.RocketPart = this;
			outline.OutlineColor = rocketPartData.OutlineColor;

			idx = rocketPartPositions.Value.Count;
			rocketPartPositions.Value.Add(transform.position);
		}

		private void Update()
		{
			if(lastPosition != transform.position)
			{
				rocketPartPositions[idx] = transform.position;
			}
		}
	}
}
