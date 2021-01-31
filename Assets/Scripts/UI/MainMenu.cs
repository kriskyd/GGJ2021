using UnityEngine;
using UnityEngine.UI;

public class MainMenu : TemporalSingleton<MainMenu>
{
	[SerializeField] private GameObject menuPanel;
	[SerializeField] private GameObject controlsPanel;
	[SerializeField] private GameObject creditsPanel;
	[SerializeField] private SimpleMenu menuController;
	[SerializeField] private Button button1;
	[SerializeField] private Button button2;
	[SerializeField] private Button button3;
	[SerializeField] private Button button4;

	protected override void Initialize()
	{
		base.Initialize();
		button1.onClick.AddListener(PlayGame);
		button2.onClick.AddListener(ShowControls);
		button3.onClick.AddListener(ShowCredits);
		button4.onClick.AddListener(ExitGame);
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
		Time.timeScale = 1f;
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
