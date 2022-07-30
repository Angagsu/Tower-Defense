using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text moneyText, timerText, livesText;
    [SerializeField] private Text standardTowerCostText, missileLauncherTowerCostText, laserTowerCostText;
    [SerializeField] private Text waveSurvivedText;
    [SerializeField] private GameObject gameOverPanel;
    public GameObject GameOverPanel{ get { return gameOverPanel; } }

    private Shop towerCost;
    private WaveSpawner waveSpawner;
    void Start()
    {
        waveSpawner = GetComponent<WaveSpawner>();
        towerCost = FindObjectOfType<Shop>();

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
        standardTowerCostText.text = "$ " + towerCost.standardTower.cost.ToString();
        missileLauncherTowerCostText.text = "$ " + towerCost.missileLauncherTower.cost.ToString();
        laserTowerCostText.text = "$ " + towerCost.laserTower.cost.ToString();
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
    }

    public void Menu()
    {

    }
}
