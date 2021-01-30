using UnityEngine;

namespace RocketSystem
{
	public class RocketPart : MonoBehaviour
	{
		[SerializeField]
		private RocketPartData rocketPartData;
		[SerializeField]
		private Outline outline;

		public RocketPartData RocketPartData => rocketPartData;

		private void Start()
		{
			rocketPartData.RocketPart = this;
			outline.OutlineColor = rocketPartData.OutlineColor;
		}
	}
}
