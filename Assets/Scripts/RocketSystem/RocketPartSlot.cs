using UnityEngine;

namespace RocketSystem
{
	public class RocketPartSlot : MonoBehaviour
	{
		public static int slotsCount;

		[SerializeField]
		private RocketPartData rocketPartData;
		[SerializeField]
		private MeshRenderer meshRenderer;

		public RocketPartData RocketPartData => rocketPartData;

		private bool PartInSlot = false;

		private void Awake()
		{
			++slotsCount;
			meshRenderer.enabled = PartInSlot;
		}

		private void Start()
		{
			rocketPartData.RocketPartSlot = this;
		}

        private void OnDestroy()
        {
			--slotsCount;
		}

        public void PlaceRocketPart(RocketPart rocketPart)
		{
			if (PartInSlot) return;
			rocketPart.gameObject.SetActive(false);
			PartInSlot = true;
			meshRenderer.enabled = true;
			GameManager.Instance.OnRocketPartPlaced();
		}

		public void RemoveRocketPart(RocketPart rocketPart)
		{
			if (!PartInSlot) return;
			rocketPart.gameObject.SetActive(true);
			PartInSlot = false;
			meshRenderer.enabled = false;
			GameManager.Instance.OnRocketPartRemoved();
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
