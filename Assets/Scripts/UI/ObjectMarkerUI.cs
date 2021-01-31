using System.Collections;
using System.Collections.Generic;
using RocketSystem;
using SA.AnimatedValues;
using UnityEngine;
using UnityEngine.UI;

public interface IScreenMarkable
{
	bool ShowMarkerOnScreen { get; }
}

public class ObjectMarkerUI : MonoBehaviour
{
	[SerializeField] private RocketPart target;
	[SerializeField] private CanvasGroup canvasGroup;

	private AnimBool visible = new AnimBool(false);

	private void Update()
	{
		UpdateOnScreenPosition();
		UpdateFadeValue();
	}

	private void UpdateFadeValue()
	{
		var viewPoint = CameraController.Instance.Camera.WorldToViewportPoint(target.transform.position);
		visible.Target = !target.ShowMarkerOnScreen && (viewPoint.x < 0 || viewPoint.x > 1 || viewPoint.y < 0 || viewPoint.y > 1);
		canvasGroup.alpha = visible.FadedValue;
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
		float angle = Vector3.Angle(Vector2.up, direction);
		if(targetScreenPosition.x > 0f)
			angle *= -1f;
		transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, angle);
	}
}
