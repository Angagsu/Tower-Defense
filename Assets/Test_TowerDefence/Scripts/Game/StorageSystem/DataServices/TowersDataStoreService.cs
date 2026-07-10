using System;
using System.Collections.Generic;
using UnityEngine;


public class TowersDataStoreService : MonoBehaviour, IService
{
    public event Action<TowersDataToStore> TowerDataChanged;
    public List<int> SelectedAcquiredTowerByIndex => selectedAcquiredTowerByIndex;
    public TowersDataToStore TowersDataToStore => towersDataToStore;
    [field: SerializeField] public AllTowersBlueprintSO AllTowers { get; private set; }

    private const string TOWERS_DATA_KEY = "TowersData.Json";

    [SerializeField] private StorageService storageService;
    private TowersDataToStore towersDataToStore = new TowersDataToStore();

    private TowersDataHandler towersDataHandler;
    private List<int> selectedAcquiredTowerByIndex = new();


    public void SetTowersDataHandler(TowersDataHandler towersDataHandler)
    {
        this.towersDataHandler = towersDataHandler;
        this.towersDataHandler.SelectedAcquiredTowersChanged += OnSelectedAcquiredTowerChange;
    }

    private void OnSelectedAcquiredTowerChange(List<int> selectedAcquiredHeroesbyIndex)
    {
        this.selectedAcquiredTowerByIndex = selectedAcquiredHeroesbyIndex;
        towersDataToStore.SelectedAcquiredTowersByIndex = this.selectedAcquiredTowerByIndex;
    }

    private void Save()
    {
        storageService.JsonSaver.Save(TOWERS_DATA_KEY, towersDataToStore, OnSave);
    }

    public void Load()
    {
        try
        {
            storageService.JsonSaver.Load<TowersDataToStore>(TOWERS_DATA_KEY, OnLoad);
        }
        catch (Exception e)
        {
            Debug.Log(e);
            Save();
        }
    }

    private void OnLoad(TowersDataToStore towersDataToStore)
    {
        this.towersDataToStore = towersDataToStore;
        this.selectedAcquiredTowerByIndex = towersDataToStore.SelectedAcquiredTowersByIndex;


        if(this.towersDataToStore.AllTowers.Count < AllHeroAndTowerTypes.TowerTypes.Count)
        {
            for(int i = 0; i < AllHeroAndTowerTypes.TowerTypes.Count; i++)
            {
                if (this.towersDataToStore.AllTowers.ContainsKey(AllHeroAndTowerTypes.TowerTypes[i]))
                {
                    continue;
                }
                else
                {
                    TowerUpgradesData towerUpgradesData = new TowerUpgradesData();

                    for (int j = 0; j < AllHeroAndTowerTypes.TowerCards.Count; j++)
                    {
                        towerUpgradesData.AddTowerData(new TowerData(AllHeroAndTowerTypes.TowerTypes[i], TowerUpgradeLevel.Level_1, AllHeroAndTowerTypes.TowerCards[j], isAcquired: false, price: default));
                    }

                    this.towersDataToStore.AllTowers.Add(AllHeroAndTowerTypes.TowerTypes[i], towerUpgradesData);
                } 
            }
        }

        TowerDataChanged?.Invoke(towersDataToStore);
        towersDataHandler.SetSelectedTowersByIndex(this.selectedAcquiredTowerByIndex);
    }

    private void OnSave(bool isSaved)
    {
        TowerDataChanged?.Invoke(towersDataToStore);
    }

    public void StoreChangedTowerData(TowerData changedTowerData, int towerDataIndex, List<int> selectedAcquirdHeroesByIndex)
    {
        if (towersDataToStore.AllTowers.ContainsKey(changedTowerData.TowerType))
        {
            towersDataToStore.AllTowers[changedTowerData.TowerType].TowerUpgrades[towerDataIndex] = changedTowerData;
            towersDataToStore.SelectedAcquiredTowersByIndex = selectedAcquirdHeroesByIndex;
            Save();
        }
    }

    public void StoreSelectedTowers()
    {
        Save();
    }

    public void AddCard(List<TowerCard> cards)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            if (towersDataToStore.AllTowers.ContainsKey(cards[i].TowerType))
            {
                towersDataToStore.AllTowers[cards[i].TowerType].TowerUpgrades[(int)cards[i].CardType].ReceivedCards.Add(cards[i]);
            }
        }

        Save();
    }

    public void ReduceCard(TowerData towerData, int cards, int index)
    {
        towersDataToStore.AllTowers[towerData.TowerType].TowerUpgrades[index].ReceivedCards.RemoveRange(0, cards);
    }

    public void ExchangeCards()
    {
        Save();
    }
}
