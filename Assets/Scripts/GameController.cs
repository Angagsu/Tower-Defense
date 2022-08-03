using UnityEngine.SceneManagement;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static bool IsGameOver;
    private UIManager uiManager;
    private string menuSceneName = "MainMenu";
    [SerializeField] private SceneFader sceneFader;

    private void Start()
    {
        IsGameOver = false;
        uiManager = GetComponent<UIManager>();
    }
    void Update()
    {
        if (IsGameOver)
        {
            return;
        }
        if (PlayerStats.Lives <= 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        IsGameOver = true;
        uiManager.GameOverPanel.SetActive(true);
        
    }

    public void RetryButtonOnPausePanel()
    {
        PauseOrContinueTheGame();
        sceneFader.FadeTo(SceneManager.GetActiveScene().name);
    }

    public void RetryButtonOnGameOverPanel()
    {
        sceneFader.FadeTo(SceneManager.GetActiveScene().name);
    }

    public void MenuButtonOnPausePanel()
    {
        PauseOrContinueTheGame();
        sceneFader.FadeTo(menuSceneName);
    }

    public void MenuButtonOnGameOverPanel()
    {
        sceneFader.FadeTo(menuSceneName);
    }

    public void PauseOrContinueTheGame()
    {
        uiManager.PausePanel.SetActive(!uiManager.PausePanel.activeSelf);
        if (uiManager.PausePanel.activeSelf)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

}
