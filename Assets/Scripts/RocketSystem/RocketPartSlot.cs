using System.Collections.Generic;
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

		private Material[] originalMaterials;
		private Material[] missingPartMaterials;

		private void Awake()
		{
			++slotsCount;

			originalMaterials = new Material[meshRenderer.materials.Length];
			missingPartMaterials = new Material[meshRenderer.materials.Length];

			for (int i = 0; i < meshRenderer.materials.Length; ++i)
            {
				originalMaterials[i] = meshRenderer.materials[i];
				if(missingPartMaterials == null)
				{
					UnityEngine.Debug.LogError("missingPartMaterials[] is null");
					continue;
				}
				if(GameManager.Instance == null)
				{
					UnityEngine.Debug.LogError("GameManager.Instance is null");
					continue;
				}
				if(GameManager.Instance.MissingPartMaterial == null)
				{
					UnityEngine.Debug.LogError("GameManager.Instance.MissingPartMaterial is null");
					continue;
				}
				missingPartMaterials[i] = GameManager.Instance.MissingPartMaterial;
            }

            if (!PartInSlot)
			{
				meshRenderer.materials = missingPartMaterials;
			}
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

			meshRenderer.materials = originalMaterials; 
			PartInSlot = true;

			meshRenderer.enabled = true;
			GameManager.Instance.OnRocketPartPlaced();
		}

		public void RemoveRocketPart(RocketPart rocketPart)
		{
			if (!PartInSlot) return;
			rocketPart.gameObject.SetActive(true);
			
			meshRenderer.materials = missingPartMaterials; 
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
