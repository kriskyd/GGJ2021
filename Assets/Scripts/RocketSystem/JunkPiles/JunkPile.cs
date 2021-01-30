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
			rocketPart.transform.SetParent(this.transform);
			rocketPart.transform.localPosition = Vector3.zero;
		}
	}
}
