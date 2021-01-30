using UnityEngine;
using UnityEngine.UI;

public class HPIconObject : MonoBehaviour
{
	[SerializeField] private Image activeHP;

	public void SetActive(bool active)
	{
		activeHP.enabled = active;
	}
}