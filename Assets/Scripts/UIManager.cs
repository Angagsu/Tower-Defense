using UnityEngine.UI;
using UnityEngine;


public class UIManager : MonoBehaviour
{
    [SerializeField] private Text moneyText, timerText, livesText;
    [SerializeField] private Text standardTowerCostText, missileLauncherTowerCostText, laserTowerCostText;
    [SerializeField] private Text waveSurvivedText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject pausePanel;
    public GameObject GameOverPanel{ get { return gameOverPanel; } }
    public GameObject PausePanel { get { return pausePanel; } }

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

}
