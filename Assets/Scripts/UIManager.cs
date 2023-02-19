using UnityEngine.UI;
using UnityEngine;
using System.Collections;


public class UIManager : MonoBehaviour
{
    [SerializeField] private Text moneyText, timerText, livesText;
    [SerializeField] private Text standardTowerCostText, missileLauncherTowerCostText, laserTowerCostText;
    [SerializeField] private Text waveSurvivedText, waveSurvivedTextOnLevelCompleted;
    [SerializeField] private GameObject gameOverPanel, pausePanel, levelCompetePanel;
    [SerializeField] private Sprite fastTimeRunActiveSprite, fastTimeRunDeactivatedSprite;
    [SerializeField] private Button timeRewindButton;
    public bool IsTimeRewind;
    

    public GameObject GameOverPanel { get { return gameOverPanel; } }
    public GameObject PausePanel { get { return pausePanel; } }
    public GameObject LevelCompletePanel { get { return levelCompetePanel; } }

    private TowerOnBuy towerCost;
    private WaveSpawner waveSpawner;
    void Start()
    {
        waveSpawner = GetComponent<WaveSpawner>();
        towerCost = GetComponent<TowerOnBuy>();

        InvokeRepeating("TowerCostsConvetToStringOnUI", 0f, 1f);
    }

    void Update()
    {
        FloatConvetToStringOnUI();
    }

    private void FloatConvetToStringOnUI()
    {
        timerText.text = "Timer  - " + string.Format("{0:00.00}", waveSpawner.CountDown);
        moneyText.text = "Money - $" + PlayerStats.Money.ToString();
        livesText.text = "Lives   - " + PlayerStats.Lives.ToString();
        
        if (GameController.IsGameOver)
        {
            waveSurvivedText.text = PlayerStats.Waves.ToString();
        }
    }

    private void TowerCostsConvetToStringOnUI()
    {
        standardTowerCostText.text = "$ " + towerCost.standardTower.Cost.ToString();
        missileLauncherTowerCostText.text = "$ " + towerCost.missileLauncherTower.Cost.ToString();
        laserTowerCostText.text = "$ " + towerCost.laserTower.Cost.ToString();
    }

    public IEnumerator AnimateWaveSurvivedText()
    {
        waveSurvivedTextOnLevelCompleted.text = "0";
        int wave = 0;

        yield return new WaitForSeconds(0.5f);

        while (wave < PlayerStats.Waves)
        {
            wave++;
            waveSurvivedTextOnLevelCompleted.text = wave.ToString();
            yield return new WaitForSeconds(0.1f);
        }
    }
    public void TimeRewindButton()
    {
        IsTimeRewind = !IsTimeRewind;
        if (IsTimeRewind)
        {
            Time.timeScale = 2f;
            timeRewindButton.image.sprite = fastTimeRunActiveSprite;
        }
        else
        {
            Time.timeScale = 1f;
            timeRewindButton.image.sprite = fastTimeRunDeactivatedSprite;
        }
    }
}
