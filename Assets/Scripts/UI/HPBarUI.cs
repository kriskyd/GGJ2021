using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBarUI : MonoBehaviour
{
	[SerializeField] private Transform layoutRoot;
	[SerializeField] private HPIconObject hpTemplate;

	private List<HPIconObject> allHPs = new List<HPIconObject>();

	private IEnumerator Start()
	{
		while(GameManager.Instance?.PlayerController == null)
			yield return null;

		InitUI(GameManager.Instance.PlayerController.HP);
		GameManager.Instance.PlayerController.OnHPChanged += OnHPChanged;
	}

	private void OnDestroy()
	{
		if(GameManager.Instance?.PlayerController != null)
			GameManager.Instance.PlayerController.OnHPChanged -= OnHPChanged;
	}

	private void InitUI(int hp)
	{
		for(int i = 0; i < hp; i++)
		{
			var hpItem = Instantiate(hpTemplate, layoutRoot);
			hpItem.transform.SetAsFirstSibling();
			allHPs.Add(hpItem);
		}
	}

	private void OnHPChanged(int hp)
	{
		UpdateUI(hp);
	}

	private void UpdateUI(int hp)
	{
		for(int i = 0; i < allHPs.Count; i++)
		{
			allHPs[i].SetActive(i < hp);
		}
	}

}
