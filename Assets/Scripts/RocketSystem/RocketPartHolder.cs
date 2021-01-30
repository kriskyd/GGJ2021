using RocketSystem;
using UnityEngine;

public class RocketPartHolder : MonoBehaviour
{
	[SerializeField]
	private RocketPartHolderValue rocketPartHolderValue;

	public bool PlayerHoldsPart => holdedRocketPart != null;

	private RocketPart holdedRocketPart;

	private void Start()
	{
		rocketPartHolderValue.Value = this;
	}

	public void PickUpRocketPart(RocketPart rocketPart)
	{
		holdedRocketPart = rocketPart;
		holdedRocketPart.transform.SetParent(this.transform);
		holdedRocketPart.transform.localPosition = Vector3.zero;
	}

	public void DropRocketPart()
	{
		holdedRocketPart.transform.SetParent(null);
		holdedRocketPart = null;
	}
}
