using UnityEngine;

namespace RocketSystem
{
	public class RocketPartSlot : MonoBehaviour
	{
		[SerializeField]
		private RocketPartData rocketPartData;

		public RocketPartData RocketPartData => rocketPartData;

		private void Start()
		{
			rocketPartData.RocketPartSlot = this;
		}

		public void PlaceRocketPart(RocketPart rocketPart)
		{
			rocketPart.transform.SetParent(this.transform);
			rocketPart.transform.localPosition = Vector3.zero;
		}

#if UNITY_EDITOR
		[ExecuteInEditMode()]
		private void OnDrawGizmos()
		{
			Gizmos.color = rocketPartData.OutlineColor;
			Gizmos.DrawWireSphere(transform.position, 0.1f);
		}
#endif
	}
}
