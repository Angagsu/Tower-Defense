using System.Collections;
using System.Collections.Generic;
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
}
