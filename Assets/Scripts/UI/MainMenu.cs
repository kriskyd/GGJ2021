using UnityEngine;

public class MainMenu : TemporalSingleton<MainMenu>
{
	[SerializeField] private GameObject menuPanel;
	[SerializeField] private GameObject controlsPanel;
	[SerializeField] private GameObject creditsPanel;
	[SerializeField] private SimpleMenu menuController;

	protected override void Initialize()
	{
		base.Initialize();

		menuController.CreateButton("Play Game", PlayGame);
		menuController.CreateButton("Controls", ShowControls);
		menuController.CreateButton("Credits", ShowCredits);
		menuController.CreateButton("Exit Game", ExitGame);
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
			else if(creditsPanel.activeInHierarchy)
			{
				creditsPanel.SetActive(false);
				menuPanel.SetActive(true);
			}
		}
	}


	private void PlayGame()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene(1);
	}

	private void ShowCredits()
	{
		menuPanel.SetActive(false);
		creditsPanel.SetActive(true);
	}

	private void ShowControls()
	{
		menuPanel.SetActive(false);
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

}
