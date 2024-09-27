using UnityEngine.UI;
using UnityEngine;
using System.Collections;


public class UIManager : MonoBehaviour
{
    [SerializeField] private Text moneyText, waveText, livesText;
    [SerializeField] private Text arrowTowerCostText, golemTowerCostText, thunderTowerCostText;
    [SerializeField] private Text defenderTowerCostText, fireTowerCostText, iceTowerCostText;
    [SerializeField] private Text waveSurvivedText, waveSurvivedTextOnLevelCompleted;

    [SerializeField] private GameObject gameOverPanel, pausePanel, levelCompetePanel;
    [SerializeField] private Sprite fastTimeRunActiveSprite, fastTimeRunDeactivatedSprite;
    [SerializeField] private Button timeRewindButton;
    public bool IsTimeRewind;
    

    public GameObject GameOverPanel { get { return gameOverPanel; } }
    public GameObject PausePanel { get { return pausePanel; } }
    public GameObject LevelCompletePanel { get { return levelCompetePanel; } }

    private TowerOnBuy towerCost;
    [SerializeField]  FactoriesService factoriesService;

    private void Awake()
    {
        factoriesService.WaveComplited += FactoriesService_WaveComplited;
    }

    void Start()
    {      
        towerCost = GetComponent<TowerOnBuy>();
        FactoriesService_WaveComplited();
        InvokeRepeating("TowerCostsConvetToStringOnUI", 0f, 1f);
    }

    private void OnDisable()
    {
        factoriesService.WaveComplited -= FactoriesService_WaveComplited;
    }

    void Update()
    {
        FloatConvetToStringOnUI();
    }

    private void FactoriesService_WaveComplited()
    {
        waveText.text = "Wave  - " + (factoriesService.WaveIndex + 1) + '/' + factoriesService.Wayes[0].Waves.Length;
    }

    private void FloatConvetToStringOnUI()
    {
       //if (factoriesService.WaveIndex != factoriesService.waypoints_1Waves.Length)
       //{
       //    waveText.text = "Wave  - " + (factoriesService.WaveIndex + 1) + '/' + factoriesService.waypoints_1Waves.Length;
       //}
        
        moneyText.text = "Money - $" + PlayerStats.Money.ToString();
        livesText.text = "Lives   - " + PlayerStats.Lives.ToString();
        
        if (GameController.IsGameOver)
        {
            waveSurvivedText.text = PlayerStats.Waves.ToString();
        }
    }

    private void TowerCostsConvetToStringOnUI()
    {
        arrowTowerCostText.text = "$ " + towerCost.ArrowTower.Cost.ToString();
        golemTowerCostText.text = "$ " + towerCost.GolemTower.Cost.ToString();
        thunderTowerCostText.text = "$ " + towerCost.ThunderTower.Cost.ToString();
        defenderTowerCostText.text = "$ " + towerCost.DefenderTower.Cost.ToString();
        fireTowerCostText.text = "$ " + towerCost.FireTower.Cost.ToString();
        iceTowerCostText.text = "$ " + towerCost.IceTower.Cost.ToString();
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
