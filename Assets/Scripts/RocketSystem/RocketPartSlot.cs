using UnityEngine;

namespace RocketSystem
{
	public class RocketPartSlot : MonoBehaviour
	{
		[SerializeField]
		private RocketPartData rocketPartData;
		[SerializeField]
		private MeshRenderer meshRenderer;

		public RocketPartData RocketPartData => rocketPartData;

		private bool PartInSlot = false;

		private void Awake()
		{
			meshRenderer.enabled = PartInSlot;
		}

		private void Start()
		{
			rocketPartData.RocketPartSlot = this;
		}

		public void PlaceRocketPart(RocketPart rocketPart)
		{
			rocketPart.gameObject.SetActive(false);
			PartInSlot = true;
			meshRenderer.enabled = true;
		}

		public void RemoveRocketPart(RocketPart rocketPart)
		{
			rocketPart.gameObject.SetActive(true);
			PartInSlot = false;
			meshRenderer.enabled = false;
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
