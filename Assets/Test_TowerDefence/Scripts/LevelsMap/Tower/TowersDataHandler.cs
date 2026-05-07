using System;
using System.Collections.Generic;
using UnityEngine;

public class TowersDataHandler : MonoBehaviour
{
    public event Action<List<int>> SelectedAcquiredTowersChanged;
    public List<int> SelectedTowersByIndex => selectedAcquiredTowersByIndex;

    [SerializeField] private AllTowersBlueprintSO AllTowers;
    [SerializeField] private LevelsMapTower levelsMapHeroPrefab;
    [SerializeField] private RectTransform parentTransform;
    [SerializeField] private TowersUpgradeUI towerUpgradeUI;
    [SerializeField] private PreparingToPlayUI preparingToPlayUI;

    private TowersDataStoreService towersDataStoreService;

    private List<LevelsMapTower> levelsMapTowers = new();
    private GamePlayerDataHandler gamePlayerDataHandler;
    private LevelsMapTower currentSelectedTower;
    private LevelsMapTowerUpgrade currentSelectedTowerUpgrade;

    private List<int> selectedAcquiredTowersByIndex = new();


    private void Awake()
    {
        for (int i = 0; i < AllHeroAndTowerTypes.TowerTypes.Count; i++)
        {
            levelsMapTowers.Add(Instantiate(levelsMapHeroPrefab, parentTransform, true));
            levelsMapTowers[i].SetTowersDataHandler(this);
            levelsMapTowers[i].SetParams(AllTowers.AllTowers[i], i);
        }

        currentSelectedTower = levelsMapTowers[0];

        // make first fore towers to selected by default
        selectedAcquiredTowersByIndex.Add(0);
        selectedAcquiredTowersByIndex.Add(1);
        selectedAcquiredTowersByIndex.Add(2);
        selectedAcquiredTowersByIndex.Add(3);
    }

    private void OnDisable()
    {
        towersDataStoreService.TowerDataChanged -= OnTowerDataChange;
    }

    [Inject]
    public void Construct(TowersDataStoreService towersDataStoreService, GamePlayerDataHandler gamePlayerDataHandler)
    {
        this.towersDataStoreService = towersDataStoreService;
        this.gamePlayerDataHandler = gamePlayerDataHandler;

        this.towersDataStoreService.SetTowersDataHandler(this);
        this.SelectedAcquiredTowersChanged?.Invoke(selectedAcquiredTowersByIndex);
        this.towersDataStoreService.TowerDataChanged += OnTowerDataChange;
        this.towersDataStoreService.Load();
    }

    private void OnTowerDataChange(TowersDataToStore towerDataToStore)
    {
        AllTowers.AcquiredTowers.Clear();

        for (int i = 0; i < AllHeroAndTowerTypes.TowerTypes.Count; i++)
        {
            for(int j = 0; j < AllHeroAndTowerTypes.TowerCards.Count; j++)
            {
                levelsMapTowers[i].TowerFullBlueprintSO.TowerUpgrades[j].TowerData.TowerType = towerDataToStore.AllTowers[(TowerType)i].TowerUpgrades[j].TowerType;
                levelsMapTowers[i].TowerFullBlueprintSO.TowerUpgrades[j].TowerData.UpgradeLevel = towerDataToStore.AllTowers[(TowerType)i].TowerUpgrades[j].UpgradeLevel;
                levelsMapTowers[i].TowerFullBlueprintSO.TowerUpgrades[j].TowerData.CardType = towerDataToStore.AllTowers[(TowerType)i].TowerUpgrades[j].CardType;
                levelsMapTowers[i].TowerFullBlueprintSO.TowerUpgrades[j].TowerData.IsAcquired = towerDataToStore.AllTowers[(TowerType)i].TowerUpgrades[j].IsAcquired;
                levelsMapTowers[i].TowerFullBlueprintSO.TowerUpgrades[j].CurrentUpgradeLevel =
                    levelsMapTowers[i].TowerFullBlueprintSO.TowerUpgrades[j].UpgradeLevelsSO.TowerUpgradeLevelSOs[(int)towerDataToStore.AllTowers[(TowerType)i].TowerUpgrades[j].UpgradeLevel - 1];
                levelsMapTowers[i].TowerFullBlueprintSO.TowerUpgrades[j].TowerData.ReseivedCards = towerDataToStore.AllTowers[(TowerType)i].TowerUpgrades[j].ReseivedCards;
            }

            levelsMapTowers[i].SetParams(levelsMapTowers[i].TowerFullBlueprintSO, i);

            if (towerDataToStore.AllTowers[(TowerType)i].TowerUpgrades[0].IsAcquired)
            {
                AllTowers.AcquiredTowers.Add(AllTowers.AllTowers[i]);
            }
        }

        currentSelectedTower.Select();
    }

    public List<LevelsMapTower> GetLevelsMapTowers()
    {
        return levelsMapTowers;
    }

    public void SetSelectedTowersByIndex(List<int> selectedHeroesByIndex)
    {
        this.selectedAcquiredTowersByIndex = selectedHeroesByIndex;
    }

    public void SetCurrentSelectedTower(LevelsMapTower currentSelectedTower, int ID)
    {
        this.currentSelectedTower = currentSelectedTower;
        towerUpgradeUI.SetSelectedTowerStats(currentSelectedTower);
    }

    public void SetCurrentSelectedTowerUpgrade(LevelsMapTowerUpgrade currentSelectedTowerUpgrade)
    {
        this.currentSelectedTowerUpgrade = currentSelectedTowerUpgrade;
    }

    public void UpgradeTower(LevelsMapTowerUpgrade levelsMapTowerUpgrade, int upgradeGoldCost, int upgradeCardCost)
    {
        this.currentSelectedTowerUpgrade = levelsMapTowerUpgrade;
        if (upgradeCardCost > 0 && upgradeCardCost <= currentSelectedTowerUpgrade.TowerData.ReseivedCards.Count)
        {
            UpgradeTowerByGoldAndCard(upgradeGoldCost, upgradeCardCost, currentSelectedTowerUpgrade.ID);
        }
        else if (upgradeCardCost == 0)
        {
            UpgradeTowerByGold(upgradeGoldCost);
        }
        else
        {
            Debug.Log("Not enought card to upgrade hero !!!");
        }
    }

    private void UpgradeTowerByGoldAndCard(int upgradeGoldCost, int upgradeCardCost, int index)
    {
        if (gamePlayerDataHandler.TrySpendGold(upgradeGoldCost))
        {
            currentSelectedTowerUpgrade.TowerData.UpgradeLevel = currentSelectedTowerUpgrade.TowerData.UpgradeLevel + 1;
            currentSelectedTowerUpgrade.TowerSO.CurrentUpgradeLevel = currentSelectedTowerUpgrade.TowerSO.UpgradeLevelsSO.TowerUpgradeLevelSOs[(int)currentSelectedTowerUpgrade.TowerData.UpgradeLevel - 1];

            var upgradedHeroData = new TowerData(currentSelectedTowerUpgrade.TowerData.TowerType, currentSelectedTowerUpgrade.TowerData.UpgradeLevel, currentSelectedTowerUpgrade.TowerData.CardType, isAcquired: true, currentSelectedTowerUpgrade.TowerData.Price);

            currentSelectedTowerUpgrade.TowerData.ReseivedCards.RemoveRange(0, upgradeCardCost);

            upgradedHeroData.ReseivedCards = currentSelectedTowerUpgrade.TowerData.ReseivedCards;

            towerUpgradeUI.SetSelectedTowerUpgradeStats(currentSelectedTowerUpgrade);

            towersDataStoreService.ReduceCard(upgradedHeroData, upgradeCardCost, currentSelectedTowerUpgrade.ID);
            towersDataStoreService.StoreChangedTowerData(upgradedHeroData, currentSelectedTowerUpgrade.ID, selectedAcquiredTowersByIndex);
        }
        else
        {
            Debug.Log("Not enoght money to upgrade hero !!!");
        }
    }

    private void UpgradeTowerByGold(int upgradeGoldCost)
    {
        if (gamePlayerDataHandler.TrySpendGold(upgradeGoldCost))
        {
            currentSelectedTowerUpgrade.TowerData.UpgradeLevel = currentSelectedTowerUpgrade.TowerData.UpgradeLevel + 1;
            currentSelectedTowerUpgrade.TowerSO.CurrentUpgradeLevel = currentSelectedTowerUpgrade.TowerSO.UpgradeLevelsSO.TowerUpgradeLevelSOs[(int)currentSelectedTowerUpgrade.TowerData.UpgradeLevel - 1];

            towerUpgradeUI.SetSelectedTowerUpgradeStats(currentSelectedTowerUpgrade);

            var upgradedTowerData = new TowerData(currentSelectedTowerUpgrade.TowerData.TowerType, currentSelectedTowerUpgrade.TowerData.UpgradeLevel, currentSelectedTowerUpgrade.TowerData.CardType, isAcquired: true, currentSelectedTowerUpgrade.TowerData.Price);
            upgradedTowerData.ReseivedCards = currentSelectedTowerUpgrade.TowerData.ReseivedCards;

            towersDataStoreService.StoreChangedTowerData(upgradedTowerData, currentSelectedTowerUpgrade.ID, selectedAcquiredTowersByIndex);
        }
        else
        {
            Debug.Log("Not enoght money to upgrade hero !!!");
        }
    }

    public void SaveSelectedTowersBeforeStartPlayLevel()
    {
        towersDataStoreService.StoreSelectedTowers();
    }

    public bool TryBuyTower()
    {
        if (gamePlayerDataHandler.TrySpendDiamond(currentSelectedTower.TowerData.Price))
        {
            currentSelectedTower.TowerData.IsAcquired = true;
            currentSelectedTower.OnTowerDataChange(currentSelectedTower.TowerData);
            towerUpgradeUI.SetSelectedTowerStats(currentSelectedTower);
            towersDataStoreService.StoreChangedTowerData(currentSelectedTower.TowerData, currentSelectedTowerUpgrade.ID, selectedAcquiredTowersByIndex);

            //AllTowers.AcquiredTowers.Add(currentSelectedTower.TowerFullBlueprintSO);
            return true;
        }
        else
        {
            Debug.Log("Not Enought Diamonds To Buy");
            return false;
        }
    }

    public bool TryBuyTowerUpgrade()
    {
        if (gamePlayerDataHandler.TrySpendGold(currentSelectedTowerUpgrade.TowerData.Price))
        {
            currentSelectedTowerUpgrade.TowerData.IsAcquired = true;
            currentSelectedTowerUpgrade.OnTowerDataChange(currentSelectedTowerUpgrade.TowerData);
            towerUpgradeUI.SetSelectedTowerUpgradeStats(currentSelectedTowerUpgrade);
            towersDataStoreService.StoreChangedTowerData(currentSelectedTowerUpgrade.TowerData,currentSelectedTowerUpgrade.ID, selectedAcquiredTowersByIndex);

            return true;
        }
        else
        {
            Debug.Log("Not Enought Gold To Buy");
            return false;
        }
    }
}
