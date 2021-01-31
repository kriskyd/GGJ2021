using RocketSystem;
using UnityEngine;

public class JunkPile : MonoBehaviour
{
	[SerializeField]
	private RocketPart rocketPart;

	private void Awake()
	{
		if(rocketPart != null)
		{
			PlaceRocketPart(rocketPart);
		}
	}

	public void PlaceRocketPart(RocketPart rocketPart)
	{
		rocketPart.transform.SetParent(this.transform);
		rocketPart.transform.localPosition = Vector3.zero;

		rocketPart.PickedUp += RocketPart_PickedUp;
	}

	private void RocketPart_PickedUp(RocketPart rocketPart)
	{
		rocketPart = null;
	}
}
