using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelsMapMainUI : MonoBehaviour
{

    [Header("Player Stats")]
    [Space(5)]
    [SerializeField] private TextMeshProUGUI diamondsText;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI starsText;
    [SerializeField] private TextMeshProUGUI energyText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private Button quitButton;

    [Header("Hero Upgrade Panel")]
    [Space(5)]
    [SerializeField] private Button heroUpgradButton;
    [SerializeField] private GameObject heroUpgradPanel;

    [Header("Tower Upgrade Panel")]
    [Space(5)]
    [SerializeField] private Button towerUpgradeButton;
    [SerializeField] private GameObject towerUpgradePanel;

    [Header("Shop Panel")]
    [Space(5)]
    [SerializeField] private Button shopButton;
    [SerializeField] private GameObject shopPanel;

    [Header("Cards Library Panel")]
    [SerializeField] private Button cardsLibraryButton;
    [SerializeField] private GameObject cardsLibraryPanel;

    [Header("Monsters Book Panel")]
    [SerializeField] private Button monstersBookButton;
    [SerializeField] private GameObject monstersBookPanel;

    [Header("Settings Panel")]
    [SerializeField] private Button settingsButton;
    [SerializeField] private GameObject settingsPanel;

    [Header("Preparing To Play Panel")]
    [Space(5)]
    [SerializeField] private GameObject preparingToPlayPanel;

    [Space(15)]
    [SerializeField] private LevelsMapDataHandler levelsDataHandler;

    private GamePlayerDataHandler playerDataHandler;

    private int currentEnergy;
    private int maxEnergy = 40;


    private void Awake()
    { 
        heroUpgradButton.onClick.AddListener(() => { heroUpgradPanel.SetActive(true); quitButton.gameObject.SetActive(true); });
        towerUpgradeButton.onClick.AddListener(() => { towerUpgradePanel.SetActive(true); quitButton.gameObject.SetActive(true); });
        shopButton.onClick.AddListener(() => { shopPanel.SetActive(true); quitButton.gameObject.SetActive(true); });
        cardsLibraryButton.onClick.AddListener(() => { cardsLibraryPanel.SetActive(true); quitButton.gameObject.SetActive(true); });
        monstersBookButton.onClick.AddListener(() => { monstersBookPanel.SetActive(true); quitButton.gameObject.SetActive(true); });
        settingsButton.onClick.AddListener(() => { settingsPanel.SetActive(true); quitButton.gameObject.SetActive(true); });
        quitButton.onClick.AddListener(HideAllPanels);
    }

    [Inject]
    public void Construct(GamePlayerDataHandler playerDataHandler)
    {
        this.playerDataHandler = playerDataHandler;

        currentEnergy = playerDataHandler.Energy;
        SetTextsValues();

        SubscrabOnTextsValuesChange();
    }

    private void SubscrabOnTextsValuesChange()
    {
        playerDataHandler.PlayerDataChanged += OnPlayerDataChange;
        playerDataHandler.EnergyTimerTicked += OnEnergyTimerTick;
    }

    private void OnEnergyTimerTick(TimeSpan ts)
    {
        if (playerDataHandler.Energy == maxEnergy)
        {
            timerText.text = "";
            return;
        }

        timerText.text = $"{ts.Minutes:D2}:{ts.Seconds:D2}";
    }

    private void OnPlayerDataChange(GamePlayerData playerData)
    {
        diamondsText.text = playerData.Diamonds.ToString();
        goldText.text = playerData.Gold.ToString();

        currentEnergy = playerData.Energy;
        energyText.text = $"{currentEnergy} / {maxEnergy}";
    }

    private void SetTextsValues()
    {
        diamondsText.text = playerDataHandler.Diamonds.ToString();
        goldText.text = playerDataHandler.Gold.ToString();
        starsText.text = levelsDataHandler.ReceivedStars.ToString();
        energyText.text = $"{currentEnergy} / {maxEnergy}";
        timerText.text = "";
    }

    private void HideAllPanels()
    {
        heroUpgradPanel.SetActive(false);
        towerUpgradePanel.SetActive(false);
        shopPanel.SetActive(false);
        cardsLibraryPanel.SetActive(false);
        monstersBookPanel.SetActive(false);
        settingsPanel.SetActive(false);
        quitButton.gameObject.SetActive(false);
    }

    public void ShowPreparingToPlayPanel()
    {
        preparingToPlayPanel.SetActive(true);
    }

    private void OnDisable()
    {
        playerDataHandler.PlayerDataChanged -= OnPlayerDataChange;
        playerDataHandler.EnergyTimerTicked -= OnEnergyTimerTick;
    }
}
