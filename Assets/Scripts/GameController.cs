using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;


public class GameController : MonoBehaviour 
{
    
    public static bool IsGameOver;

    private UIManager uiManager;
    private string menuSceneName = "MainMenu";
    [SerializeField] private SceneFader sceneFader;

    [Header("Optional settings. Dont Touch")]
    [SerializeField] private int levelToUnlock;
    [SerializeField] private string nextLevel;

    
    private void Awake()
    {
        IsGameOver = false;
    }
    private void Start()
    {
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

    public void LevelComplete()
    {
        IsGameOver = true;
        uiManager.LevelCompletePanel.SetActive(true);
        if (PlayerPrefs.GetInt("levelReached") < levelToUnlock)
        {
            PlayerPrefs.SetInt("levelReached", levelToUnlock);
        }
        uiManager.StartCoroutine("AnimateWaveSurvivedText");
       // sceneFader.FadeTo(nextLevel);
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

    public void RetryButtonOnLevelCompletePanel()
    {
        sceneFader.FadeTo(SceneManager.GetActiveScene().name);
    }

    public void MenuButtonOnLevelComletePanel()
    {
        sceneFader.FadeTo(menuSceneName);
    }

    public void ContinueButtonOnLevelCompletePanel()
    {
        sceneFader.FadeTo(nextLevel);
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
