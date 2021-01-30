using TMPro;
using UnityEngine;

public class InGameMenu : TemporalSingleton<InGameMenu>
{
	[SerializeField] private GameObject background;
	[SerializeField] private TextMeshProUGUI gameStatusLabel;
	[SerializeField] private GameObject menuPanel;
	[SerializeField] private GameObject controlsPanel;
	[SerializeField] private SimpleMenu menuControl;

	protected override void Initialize()
	{
		base.Initialize();

		menuControl.CreateButton("Back To Game", BackToGame);
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
		// load new scene and start game
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


	public void Show()
	{
		gameObject.SetActive(true);
		menuPanel.SetActive(true);
	}
}