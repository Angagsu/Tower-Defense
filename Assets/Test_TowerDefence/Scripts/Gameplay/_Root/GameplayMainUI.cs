using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using TMPro;


public class GameplayMainUI : MonoBehaviour
{
    public GameObject GameOverPanel { get { return gameOverPanel; } }
    public GameObject PausePanel { get { return pausePanel; } }
    public GameObject LevelCompletePanel { get { return levelCompetePanel; } }

    private bool IsTimeRewind;


    [SerializeField] private GameplayPlayerDataHandler playerDataHandler;  
    [SerializeField] private GameplayStates gameplayStates;
    [SerializeField] private MonstersFactoriesService monstersFactoriesService;
    [Space(10)]
    [SerializeField] private TextMeshProUGUI moneyText, waveText, livesText;
    [SerializeField] private TextMeshProUGUI waveSurvivedText, waveSurvivedTextOnLevelComplete;

    [SerializeField] private GameObject gameOverPanel, pausePanel, levelCompetePanel;
    [SerializeField] private Sprite fastTimeRunActiveSprite, fastTimeRunDeactivatedSprite;
    [SerializeField] private Button timeRewindButton;
    
    


    private void Awake()
    {      
        monstersFactoriesService.WaveComplited += FactoriesService_WaveComplited;
        playerDataHandler.MoneyAmountChanged += PlayerDataHandler_OnMoneyAmountChanged;
        playerDataHandler.LivesAmountChanged += PlayerDataHandler_OnLivesAmountChanged;
    }

    private void OnEnable()
    {
        gameplayStates.Paused += OnPause;
    }

    private void OnPause()
    {
        if (gameplayStates.State == GameplayState.Defeat)
        {
            waveSurvivedText.text = playerDataHandler.Waves.ToString();
        }
    }

    private void PlayerDataHandler_OnLivesAmountChanged(int livesAmount)
    {
        livesText.text = livesAmount.ToString();
    }

    private void PlayerDataHandler_OnMoneyAmountChanged(int moneyAmount)
    {
        moneyText.text = moneyAmount.ToString();
    }

    void Start()
    {      
        FactoriesService_WaveComplited();
    }

    private void OnDisable()
    {
        gameplayStates.Paused -= OnPause;
        monstersFactoriesService.WaveComplited -= FactoriesService_WaveComplited;
        playerDataHandler.MoneyAmountChanged -= PlayerDataHandler_OnMoneyAmountChanged;
        playerDataHandler.LivesAmountChanged -= PlayerDataHandler_OnLivesAmountChanged;
    }

    private void FactoriesService_WaveComplited()
    {
        waveText.text = $"{monstersFactoriesService.WaveIndex + 1} / {monstersFactoriesService.Wayes[0].Waves.Count}";
    }

    public IEnumerator AnimateWaveSurvivedText()
    {
        waveSurvivedTextOnLevelComplete.text = "0";
        int wave = 0;

        yield return new WaitForSeconds(0.5f);

        while (wave < playerDataHandler.Waves)
        {
            wave++;
            waveSurvivedTextOnLevelComplete.text = wave.ToString();
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void TimeRewindButton()
    {
        IsTimeRewind = !IsTimeRewind;
        if (IsTimeRewind)
        {
            gameplayStates.SetGameplaySpeed(timeScale: 2);
            timeRewindButton.image.sprite = fastTimeRunActiveSprite;
        }
        else
        {
            gameplayStates.SetGameplaySpeed(timeScale: 1);
            timeRewindButton.image.sprite = fastTimeRunDeactivatedSprite;
        }
    }

    public void PauseOrContinueGameButton()
    {
        PausePanel.SetActive(!PausePanel.activeSelf);

        if (gameplayStates.State == GameplayState.Pause)
        {
            gameplayStates.SetStatePlay();
        }
        else if (gameplayStates.State == GameplayState.Play)
        {
            gameplayStates.SetStatePause();
        }
        else if (gameplayStates.State == GameplayState.Start)
        {
            gameplayStates.SetStateStart();
        }
    }
}
