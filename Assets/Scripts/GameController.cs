using UnityEngine.SceneManagement;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static bool IsGameOver;
    private UIManager uiManager;

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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void RetryButtonOnGameOverPanel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MenuButton()
    {
        Debug.Log("Menu");
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
