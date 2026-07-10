using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class TowersUpgradeUI : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button buyButton;
    [SerializeField] private Button upgradeButton;
    [Space(10)]
    [SerializeField] private Slider reachedUpgradeLevelBar;
    [SerializeField] private TextMeshProUGUI heroBuyCostText;
    [SerializeField] private TextMeshProUGUI reachedUpgradeLevelText;
    [SerializeField] private TextMeshProUGUI reachedUpgradeLevelProgressText;
    [Header("Cards")]
    [SerializeField] private TextMeshProUGUI receivedCardsText;
    [SerializeField] private TowerCardUI cardUI;
    [Space(10)]
    [SerializeField] private RectTransform parentTransform;
    [SerializeField] private LevelsMapTowerUpgrade towerUpgradPrefab;
    [Space(10)]
    [SerializeField] private UpgradeButtonUI upgradeButtonUI;
    [SerializeField] private BuyButtonUI buyButtonUI;
    [SerializeField] private TowersDataHandler towersDataHandler;
    [SerializeField] private TowersUpgradesStatsHandlerUI upgradeStatsHandlerUI;

    private List<LevelsMapTowerUpgrade> levelsMapTowerUpgrads = new();
    private List<LevelsMapTower> levelsMapTowers;
    private LevelsMapTowerUpgrade currentSelectedTowerUpgrade;
    private LevelsMapTower currentSelectedTower;
    private TowerUpgradeLevelSO nextUpgradeLevel;

    private int currentUpgradeGoldCost;
    private int currentUpgradeCardCost;



    private void Awake()
    {
        levelsMapTowers = towersDataHandler.GetLevelsMapTowers();

        currentSelectedTower = levelsMapTowers[0];

        for (int i = 0; i < 4; i++)
        {
            levelsMapTowerUpgrads.Add(Instantiate(towerUpgradPrefab, parentTransform, true));
            levelsMapTowerUpgrads[i].SetTowersUpgradeUI(this);
            levelsMapTowerUpgrads[i].SetParams(currentSelectedTower.TowerFullBlueprintSO.TowerUpgrades[i], i);
        }

        currentSelectedTowerUpgrade = levelsMapTowerUpgrads[0];

        SetSelectedTowerStats(currentSelectedTower);

        towersDataHandler.DataChanged += OnDataChange;

        buyButton.onClick.AddListener(OnBuy);
        upgradeButton.onClick.AddListener(OnUpgrade);
    }

    private void OnDestroy()
    {
        towersDataHandler.DataChanged -= OnDataChange;
    }

    public void SetSelectedTowerStats(LevelsMapTower currentSelectedTower)
    {
        if (this.currentSelectedTower && this.currentSelectedTower != currentSelectedTower)
        {
            this.currentSelectedTower.UnSelect();
        }

        this.currentSelectedTower = currentSelectedTower;
        this.currentSelectedTower.Select();

        for (int i = 0; i < levelsMapTowerUpgrads.Count; i++)
        {
            levelsMapTowerUpgrads[i].SetParams(currentSelectedTower.TowerFullBlueprintSO.TowerUpgrades[i], i);
        }

        currentSelectedTowerUpgrade = levelsMapTowerUpgrads[0];
        SetSelectedTowerUpgradeStats(currentSelectedTowerUpgrade);
    }

    public void SetSelectedTowerUpgradeStats(LevelsMapTowerUpgrade currentSelectedTowerUpgrade)
    {
        if(this.currentSelectedTowerUpgrade && this.currentSelectedTowerUpgrade != currentSelectedTowerUpgrade)
        {
            this.currentSelectedTowerUpgrade.UnSelect();
        }

        nextUpgradeLevel = null;

        towersDataHandler.SetCurrentSelectedTowerUpgrade(currentSelectedTowerUpgrade);
        this.currentSelectedTowerUpgrade = currentSelectedTowerUpgrade;
        this.currentSelectedTowerUpgrade.Select();

        SetupCardParams();
        SetupButtons();
        SetupTexts();
        SetupSliders();
    }

    public void OnDataChange()
    {
        SetupCardParams();
        SetupButtons();
        SetupTexts();
        SetupSliders();
    }

    private void SetupCardParams()
    {
        cardUI.SetParams(currentSelectedTowerUpgrade.TowerSO);
        receivedCardsText.text = currentSelectedTowerUpgrade.TowerSO.TowerData.ReceivedCards.Count.ToString();
    }

    private void SetupTexts()
    {
        reachedUpgradeLevelProgressText.text = $"{(int)currentSelectedTowerUpgrade.TowerSO.TowerData.UpgradeLevel} / {currentSelectedTowerUpgrade.TowerSO.UpgradeLevelsSO.TowerUpgradeLevelSOs.Count}";
        reachedUpgradeLevelText.text = $"{(int)currentSelectedTowerUpgrade.TowerSO.TowerData.UpgradeLevel}";
    }

    private void SetupSliders()
    {
        float x = (float)currentSelectedTowerUpgrade.TowerSO.TowerData.UpgradeLevel;
        float y = currentSelectedTowerUpgrade.TowerSO.UpgradeLevelsSO.TowerUpgradeLevelSOs.Count;
        float value = x / y;
        
        reachedUpgradeLevelBar.value = value;

        upgradeStatsHandlerUI.UpdateTowerStats(currentSelectedTowerUpgrade.TowerSO, nextUpgradeLevel);
    }

    private void SetupButtons()
    {
        if (currentSelectedTower.TowerData.IsAcquired)
        {
            if (currentSelectedTowerUpgrade.TowerData.IsAcquired)
            {
                buyButton.gameObject.SetActive(false);
                upgradeButton.gameObject.SetActive(true);

                DetermineUpgradeCost(currentSelectedTowerUpgrade.TowerSO);
                upgradeButtonUI.SetParams(currentUpgradeGoldCost, currentUpgradeCardCost);
            }
            else if (currentSelectedTower.TowerFullBlueprintSO.TowerUpgrades[currentSelectedTowerUpgrade.ID - 1] != null && !currentSelectedTower.TowerFullBlueprintSO.TowerUpgrades[currentSelectedTowerUpgrade.ID - 1].TowerData.IsAcquired)
            {
                upgradeButton.gameObject.SetActive(false);
                buyButton.gameObject.SetActive(false);
            }
            else
            {
                upgradeButton.gameObject.SetActive(false);
                buyButton.gameObject.SetActive(true);
                buyButtonUI.SetParams(isAcquird: true, currentSelectedTowerUpgrade.TowerData.Price);
            }
        }
        else
        {
            if (currentSelectedTowerUpgrade.ID == 0)
            {
                buyButton.gameObject.SetActive(true);
                upgradeButton.gameObject.SetActive(false);

                buyButtonUI.SetParams(isAcquird: false, currentSelectedTowerUpgrade.TowerData.Price);
            }
            else
            {
                buyButton.gameObject.SetActive(false);
                upgradeButton.gameObject.SetActive(false);
            }
        }
    }

    private void DetermineUpgradeCost(TowerSO towerSO)
    {
        var currentUpgradeLevle = towerSO.TowerData.UpgradeLevel;

        if ((int)currentUpgradeLevle < towerSO.UpgradeLevelsSO.TowerUpgradeLevelSOs.Count)
        {
            nextUpgradeLevel = towerSO.UpgradeLevelsSO.TowerUpgradeLevelSOs[(int)currentUpgradeLevle];
            currentUpgradeGoldCost = nextUpgradeLevel.UpgradeCostByGold;
            currentUpgradeCardCost = nextUpgradeLevel.UpgradeCostByCard;
        }
        else
        {
            nextUpgradeLevel = null;
            upgradeButton.gameObject.SetActive(false);
        }
    }

    private void OnUpgrade()
    {
        towersDataHandler.UpgradeTower(currentSelectedTowerUpgrade, currentUpgradeGoldCost, currentUpgradeCardCost);
    }

    private void OnBuy()
    {
        if(currentSelectedTowerUpgrade.ID == 0)
        {
            if (towersDataHandler.TryBuyTower())
            {
                upgradeButton.gameObject.SetActive(true);
                buyButton.gameObject.SetActive(false);
            }
        }
        else if(towersDataHandler.TryBuyTowerUpgrade())
        {
            upgradeButton.gameObject.SetActive(true);
            buyButton.gameObject.SetActive(false);
        } 
    }
}


