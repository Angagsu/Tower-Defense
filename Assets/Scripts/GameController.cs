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
    [Space(10f)]
    [SerializeField] private int levelToUnlock;
    [SerializeField] private string nextLevel;
    [SerializeField] private Button timeRewindButton;


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

    public void RetryButton()
    {
        sceneFader.FadeTo(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

    public void MenuButton()
    {
        sceneFader.FadeTo(menuSceneName);
        Time.timeScale = 1;
    }

    public void ContinueButtonOnLevelCompletePanel()
    {
        sceneFader.FadeTo(nextLevel);
        Time.timeScale = 1;
    }

    public void PauseOrContinueTheGame()
    {
        uiManager.PausePanel.SetActive(!uiManager.PausePanel.activeSelf);
        

        if (uiManager.PausePanel.activeSelf)
        {
            Time.timeScale = 0f;
        }
        else if (uiManager.IsTimeRewind && !IsGameOver)
        {
            Time.timeScale = 2f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

}
