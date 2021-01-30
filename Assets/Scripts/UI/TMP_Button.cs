using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TMP_Button : Button
{
	[SerializeField] protected TextMeshProUGUI targetText = default;
	[SerializeField] private ColorBlock textColors = new ColorBlock()
	{
		normalColor = Color.white,
		highlightedColor = Color.white,
		pressedColor = Color.white,
		disabledColor = Color.grey,
		selectedColor = Color.white,
		colorMultiplier = 1f,
		fadeDuration = 0f
	};
	[SerializeField] private Transition textTransition = Transition.ColorTint;

	public TextMeshProUGUI TextComponent { get { return targetText; } protected set { targetText = value; } }
	public ColorBlock TextColors { get { return textColors; } set { textColors = value; } }
	public Color TextBaseColor { get { return textColors.normalColor; } set { textColors.normalColor = value; } }
	public Transition TextTransition { get { return textTransition; } private set { textTransition = value; } }

	protected override void InstantClearState()
	{
		base.InstantClearState();

		switch(TextTransition)
		{
			case Transition.ColorTint:
				StartColorTween(Color.white, true);
				break;
			default:
				break;
		}
	}

	protected override void DoStateTransition(SelectionState state, bool instant)
	{
		base.DoStateTransition(state, instant);

		Color tintColor;

		switch(state)
		{
			case SelectionState.Normal:
				tintColor = TextColors.normalColor;
				break;
			case SelectionState.Highlighted:
				tintColor = TextColors.highlightedColor;
				break;
			case SelectionState.Pressed:
				tintColor = TextColors.pressedColor;
				break;
			case SelectionState.Disabled:
				tintColor = TextColors.disabledColor;
				break;
			case SelectionState.Selected:
				tintColor = TextColors.selectedColor;
				break;
			default:
				tintColor = Color.black;
				break;
		}

		if(gameObject.activeInHierarchy)
		{
			switch(textTransition)
			{
				case Transition.ColorTint:
					StartColorTween(tintColor * TextColors.colorMultiplier, instant);
					break;
				default:
					break;
			}
		}
	}

	protected void StartColorTween(Color targetColor, bool instant)
	{
		if(TextComponent == null)
			return;

		TextComponent.CrossFadeColor(targetColor, instant ? 0f : TextColors.fadeDuration, true, true);
	}

	public void SetText(string text)
	{
		targetText.text = text;
	}
}
