using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class SimpleMenu
{
	[SerializeField] private Transform layoutRoot;
	[SerializeField] private TMP_Button btnTemplate;

	public TMP_Button CreateButton(string text, UnityAction action)
	{
		var button = UnityEngine.Object.Instantiate(btnTemplate, layoutRoot);
		button.SetText(text);
		button.onClick.AddListener(action);
		return button;
	}
}
