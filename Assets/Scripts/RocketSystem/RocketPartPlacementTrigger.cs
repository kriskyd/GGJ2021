using UnityEngine;

namespace RocketSystem
{
	public class RocketPartPlacementTrigger : MonoBehaviour
	{
		private void OnTriggerEnter(Collider other)
		{
			if(other.gameObject.layer != LayerMask.NameToLayer("RocketPart"))
			{
				return;
			}

			RocketPart rocketPart = other.GetComponent<RocketPart>();

			if(rocketPart == null)
			{
				Debug.LogError("Object on RocketPart Layer does not contain RocketPart Component.");

				return;
			}

			rocketPart.RocketPartData.RocketPartSlot.PlaceRocketPart(rocketPart);
		}
	}
}