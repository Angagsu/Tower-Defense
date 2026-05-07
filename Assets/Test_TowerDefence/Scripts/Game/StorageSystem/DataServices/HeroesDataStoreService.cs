using System.Collections.Generic;
using System;
using UnityEngine;

public class HeroesDataStoreService : MonoBehaviour, IService
{
    public event Action<HeroesDataToStore> HeroesDataToStoreChanged;
    public List<int> SelectedAcquiredHeroesByIndex => selectedAcquiredHeroesByIndex;
    [field: SerializeField] public HeroesFullBlueprintSO AllHeroes { get; private set; }

    private const string HEROES_DATA_KEY = "HeroesData.Json";

    [SerializeField] private StorageService storageService;
    [SerializeField] private HeroesDataToStore heroesDataToStore = new HeroesDataToStore();

    private HeroesDataHandler heroesDataHandler;
    private HeroCardData heroCardData;
    private List<int> selectedAcquiredHeroesByIndex = new List<int> { 0, 1 };


    public void SetHeroDataHandler(HeroesDataHandler heroesDataHandler)
    {
        this.heroesDataHandler = heroesDataHandler;
        this.heroesDataHandler.SelectedAcquiredHeroesChanged += OnSelectedAcquiredHeroesChange;
    }

    private void OnSelectedAcquiredHeroesChange(List<int> selectedAcquiredHeroesbyIndex)
    {
        this.selectedAcquiredHeroesByIndex = selectedAcquiredHeroesbyIndex;
        heroesDataToStore.SelectedAcquiredHeroesByIndex = this.selectedAcquiredHeroesByIndex;
    }

    private void Save()
    {
        storageService.JsonSaver.Save(HEROES_DATA_KEY, heroesDataToStore, OnSave);
    }

    public void Load()
    {
        try
        {
            storageService.JsonSaver.Load<HeroesDataToStore>(HEROES_DATA_KEY, OnLoad);
        }
        catch (Exception e)
        {
            Debug.Log(e);
            Save();
        }
    }

    private void OnLoad(HeroesDataToStore heroesDataToStore)
    {
        this.heroesDataToStore = heroesDataToStore;
        this.selectedAcquiredHeroesByIndex = heroesDataToStore.SelectedAcquiredHeroesByIndex;

        if (this.heroesDataToStore.AllHeroes.Count < AllHeroAndTowerTypes.HeroTypes.Count)
        {
            Debug.Log("Eheeey");
            for (int i = 0; i < AllHeroAndTowerTypes.HeroTypes.Count; i++)
            {
                if (this.heroesDataToStore.AllHeroes.ContainsKey(AllHeroAndTowerTypes.HeroTypes[i]))
                {
                    continue;
                }
                else
                {
                    this.heroesDataToStore.AllHeroes.Add((HeroType)i, new HeroData(AllHeroAndTowerTypes.HeroTypes[i], HeroUpgradeLevel.Level_1, AllHeroAndTowerTypes.HeroCards[i], isAcquired: false, price: default));

                    this.heroesDataToStore.AllCards.Add((HeroType)i, new HeroCardData(AllHeroAndTowerTypes.HeroCards[i]));
                }
            }
        }

        HeroesDataToStoreChanged?.Invoke(this.heroesDataToStore);
        heroesDataHandler.SetSelectedHeroesByIndex(this.selectedAcquiredHeroesByIndex);
    }

    private void OnSave(bool isSaved)
    {
        HeroesDataToStoreChanged?.Invoke(heroesDataToStore);
    }

    public void StoreChangedHeroData(HeroData changedHeroData, List<int> selectedAcquirdHeroesByIndex)
    {
        if (heroesDataToStore.AllHeroes.ContainsKey(changedHeroData.HeroType))
        {
            heroesDataToStore.AllHeroes[changedHeroData.HeroType] = changedHeroData;
            heroesDataToStore.SelectedAcquiredHeroesByIndex = selectedAcquirdHeroesByIndex;
            Save();
        }
    }

    public void StoreSelectedHeroes()
    {
        Save();
    }

    public void AddCard(List<HeroCard> cards)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            if (heroesDataToStore.AllCards.ContainsKey(cards[i].CardType))
            {
                heroesDataToStore.AllCards[cards[i].CardType].ReceivedCards.Add(cards[i]);
                heroesDataToStore.AllHeroes[cards[i].CardType].ReseivedCards.Add(cards[i]);
            }
        }
        
        Save();
    }

    public void ReduceCard(HeroData heroData, int cards)
    {
        heroesDataToStore.AllCards[heroData.HeroType].ReceivedCards.RemoveRange(0, cards);
    }

    public HeroCardData HeroCardData { get { return heroCardData; } }
}
