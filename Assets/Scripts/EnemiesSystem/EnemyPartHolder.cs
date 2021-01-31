using RocketSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPartHolder : MonoBehaviour
{
	[SerializeField]
	private RocketPart rocketPart;

	public bool HoldsPart => rocketPart != null;

	public bool TryPickPart(RocketPart rocketPart)
	{
		if(HoldsPart)
		{
			return false;
		}

		this.rocketPart = rocketPart;

		rocketPart.transform.SetParent(this.transform);
		rocketPart.transform.localPosition = Vector3.zero;

		return true;
	}

	public bool TryDropPart()
	{
		if(!HoldsPart)
		{
			return false;
		}

		rocketPart.transform.SetParent(null);
		rocketPart = null;

		return true;
	}

	public bool TryPlacePart(JunkPile junkPile)
	{
		if(!HoldsPart)
		{
			return false;
		}

		junkPile.PlaceRocketPart(rocketPart);

		rocketPart = null;

		return true;
	}

}
