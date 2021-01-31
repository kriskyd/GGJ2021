using System.Collections;
using System.Collections.Generic;
using RocketSystem;
using SA.AnimatedValues;
using UnityEngine;
using UnityEngine.UI;

public class ObjectMarkerUI : MonoBehaviour
{
	[SerializeField] private RocketPart target;
	[SerializeField] private Image icon;

	private AnimBool visible = new AnimBool(false);

	private void Update()
	{
		UpdateOnScreenPosition();
		UpdateFadeValue();
	}

	private void UpdateFadeValue()
	{
		var viewPoint = CameraController.Instance.Camera.WorldToViewportPoint(target.transform.position);
		visible.Target = !target.IsRepaired && (viewPoint.x < 0 || viewPoint.x > 1 || viewPoint.y < 0 || viewPoint.y > 1);

		var color = icon.color;
		color.a = visible.FadedValue;
		icon.color = color;
	}

	private void UpdateOnScreenPosition()
	{
		Vector2 targetScreenPosition = CameraController.Instance.Camera.WorldToViewportPoint(target.transform.position);
		Vector2 screenCenter = new Vector2(0.5f, 0.5f);
		Vector2 direction = targetScreenPosition - screenCenter;
		if(direction != Vector2.zero)
			direction /= Mathf.Max(Mathf.Abs(direction.x), Mathf.Abs(direction.y));
		Vector2 position = Screen.safeArea.size * 0.5f + Vector2.Scale(Screen.safeArea.size, direction * 0.5f);

		transform.position = position;
	}
}
