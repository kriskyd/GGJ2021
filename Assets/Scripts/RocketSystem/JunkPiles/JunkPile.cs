using RocketSystem;
using System;
using UnityEngine;

public class JunkPile : MonoBehaviour
{
	public event Action<JunkPile> RocketPartPlaced;

	[SerializeField]
	private RocketPart rocketPart;

	public bool FreeSlot => rocketPart == null;

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
		RocketPartPlaced?.Invoke(this);
	}

	private void RocketPart_PickedUp(RocketPart rocketPart)
	{
		this.rocketPart = null;
	}
}
