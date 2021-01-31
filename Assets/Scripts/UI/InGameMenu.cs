using TMPro;
using UnityEngine;

public class InGameMenu : TemporalSingleton<InGameMenu>
{
	[SerializeField] private Sprite defeatBackgroundSprite;
	[SerializeField] private Sprite winBackgroundSprite;
	[SerializeField] private GameObject background;
	[SerializeField] private TextMeshProUGUI gameStatusLabel;
	[SerializeField] private GameObject menuPanel;
	[SerializeField] private GameObject controlsPanel;
	[SerializeField] private SimpleMenu menuControl;

	private TMP_Button backToGameButton;

	protected override void Initialize()
	{
		base.Initialize();

		backToGameButton = menuControl.CreateButton("Back To Game", BackToGame);
		menuControl.CreateButton("Restart", RestartGame);
		menuControl.CreateButton("Controls", ShowControls);
		menuControl.CreateButton("Main Menu", BackToMenu);
		menuControl.CreateButton("Exit Game", ExitGame);

		gameObject.SetActive(false);
		background.SetActive(true);
	}

	private void Update()
	{
		if(Input.GetButtonDown("Cancel"))
		{
			if(controlsPanel.activeInHierarchy)
			{
				controlsPanel.SetActive(false);
				menuPanel.SetActive(true);
			}
			else
				BackToGame();
		}
	}

	private void BackToGame()
	{
		// back to game
		gameObject.SetActive(false);
		Time.timeScale = 1f;
	}

	private void RestartGame()
	{
		// reset game
	}

	private void BackToMenu()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene(0);
	}

	private void ShowControls()
	{
		controlsPanel.SetActive(true);
	}

	private void ExitGame()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.ExitPlaymode();
#else
		Application.Quit();
#endif
	}


	public void Show(string text = "Menu", bool disableBackButton = false, bool win = false, bool lose = false)
	{
		gameStatusLabel.text = text;
        backToGameButton.gameObject.SetActive(!disableBackButton);
		if(win)
        {
			if(background.TryGetComponent(out UnityEngine.UI.Image image))
			{
				image.sprite = winBackgroundSprite;
            }
        }
		else if(lose)
        {
			if(background.TryGetComponent(out UnityEngine.UI.Image image))
			{
				image.sprite = defeatBackgroundSprite;
            }
        }
		gameObject.SetActive(true);
		menuPanel.SetActive(true);
		Time.timeScale = 0f;
	}
}