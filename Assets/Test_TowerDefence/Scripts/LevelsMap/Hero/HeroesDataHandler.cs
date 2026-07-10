using System;
using System.Collections.Generic;
using UnityEngine;

public class HeroesDataHandler : MonoBehaviour
{
    public event Action<List<int>> SelectedAcquiredHeroesChanged;
    public event Action DataChanged;
    public List<int> SelectedHeroesByIndex => selectedAcquiredHeroesByIndex;

    [SerializeField] private HeroesFullBlueprintSO AllHeroes;
    [SerializeField] private LevelsMapHero levelsMapHeroPrefab;
    [SerializeField] private RectTransform parentTransform;
    [SerializeField] private HeroesUpgradeUI heroesUpgradeUI;
    [SerializeField] private PreparingToPlayUI preparingToPlayUI;

    private HeroesDataStoreService heroesDataStoreService;
    private HeroesDataToStore heroesDataToStore;

    private List<LevelsMapHero> levelsMapHeros = new();
    private GamePlayerDataHandler gamePlayerDataHandler;
    private LevelsMapHero currentSelectedHero;

    private List<int> selectedAcquiredHeroesByIndex = new();


    private void Awake()
    {
        for (int i = 0; i < AllHeroAndTowerTypes.HeroTypes.Count; i++)
        {
            levelsMapHeros.Add(Instantiate(levelsMapHeroPrefab, parentTransform, true));
            levelsMapHeros[i].SetHeroesDataHandler(this);
            levelsMapHeros[i].SetParams(AllHeroes.Heroes[i], i);
        }

        currentSelectedHero = levelsMapHeros[0];


        // make first two heroes to selected by default
        selectedAcquiredHeroesByIndex.Add(0);
        selectedAcquiredHeroesByIndex.Add(1);
    }

    private void OnDisable()
    {
        heroesDataStoreService.HeroesDataToStoreChanged -= OnHeroesDataToStoreChange;
    }

    [Inject]
    public void Construct(HeroesDataStoreService heroesDataStoreService, GamePlayerDataHandler gamePlayerDataHandler)
    {
        this.heroesDataStoreService = heroesDataStoreService;
        this.gamePlayerDataHandler = gamePlayerDataHandler;

        this.heroesDataStoreService.SetHeroDataHandler(this);
        this.SelectedAcquiredHeroesChanged?.Invoke(selectedAcquiredHeroesByIndex);
        this.heroesDataStoreService.HeroesDataToStoreChanged += OnHeroesDataToStoreChange;
        this.heroesDataStoreService.Load();
    }

    private void OnHeroesDataToStoreChange(HeroesDataToStore heroesDataToStore)
    {
        this.heroesDataToStore = heroesDataToStore;

        AllHeroes.AcquiredHeroes.Clear();

        for (int i = 0; i < AllHeroAndTowerTypes.HeroTypes.Count; i++)
        {
            levelsMapHeros[i].HeroSO.HeroData.UpgradeLevel = heroesDataToStore.AllHeroes[(HeroType)i].UpgradeLevel;
            levelsMapHeros[i].HeroSO.HeroData.HeroType = heroesDataToStore.AllHeroes[(HeroType)i].HeroType;
            levelsMapHeros[i].HeroSO.HeroData.CardType = heroesDataToStore.AllHeroes[(HeroType)i].CardType;
            levelsMapHeros[i].HeroSO.HeroData.IsAcquired = heroesDataToStore.AllHeroes[(HeroType)i].IsAcquired;
            levelsMapHeros[i].HeroSO.HeroData.ReceivedCards = heroesDataToStore.AllHeroes[(HeroType)i].ReceivedCards;

            levelsMapHeros[i].HeroSO.CurrentUpgradeLevel = levelsMapHeros[i].HeroSO.UpgradeLevelsSO.HeroUpgradeLevelSOs[(int)heroesDataToStore.AllHeroes[(HeroType)i].UpgradeLevel - 1];

            levelsMapHeros[i].SetParams(levelsMapHeros[i].HeroSO, i);

            if (heroesDataToStore.AllHeroes[(HeroType)i].IsAcquired)
            {
                AllHeroes.AcquiredHeroes.Add(AllHeroes.Heroes[i]);
            }
        }

        currentSelectedHero.Select();
        DataChanged?.Invoke();
    }

    public List<LevelsMapHero> GetLevelsMapHeroes()
    {
        return levelsMapHeros;
    }

    public void SetSelectedHeroesByIndex(List<int> selectedHeroesByIndex)
    {
        this.selectedAcquiredHeroesByIndex = selectedHeroesByIndex;
    }

    public void SetCurrentSelectedHero(LevelsMapHero currentSelectedHero, int ID)
    {
        this.currentSelectedHero = currentSelectedHero;
        heroesUpgradeUI.SetSelectedHeroStats(currentSelectedHero);
    }

    public void UpgradeHero(int upgradeGoldCost, int upgradeCardCost)
    {
        if(upgradeCardCost > 0 && upgradeCardCost <= currentSelectedHero.HeroData.ReceivedCards.Count)
        {
            UpgradeHeroByGoldAndCard(upgradeGoldCost, upgradeCardCost);
        }
        else if(upgradeCardCost == 0)
        {
            UpgradeHeroByGold(upgradeGoldCost);
        }
        else
        {
            Debug.Log("Not enought card to upgrade hero !!!");
        }
    }

    private void UpgradeHeroByGoldAndCard(int upgradeGoldCost, int upgradeCardCost)
    {
        if (gamePlayerDataHandler.TrySpendGold(upgradeGoldCost))
        {
            currentSelectedHero.HeroData.UpgradeLevel = currentSelectedHero.HeroData.UpgradeLevel + 1;
            currentSelectedHero.HeroSO.CurrentUpgradeLevel = currentSelectedHero.HeroSO.UpgradeLevelsSO.HeroUpgradeLevelSOs[(int)currentSelectedHero.HeroData.UpgradeLevel - 1];

            var upgradedHeroData = new HeroData(currentSelectedHero.HeroData.HeroType, currentSelectedHero.HeroData.UpgradeLevel, currentSelectedHero.HeroData.CardType, isAcquired: true, currentSelectedHero.HeroData.Price);

            //currentSelectedHero.HeroData.ReceivedCards.RemoveRange(0, upgradeCardCost);
            
            upgradedHeroData.ReceivedCards = currentSelectedHero.HeroData.ReceivedCards;

            currentSelectedHero.OnHeroDataChange(upgradedHeroData);
            heroesUpgradeUI.SetSelectedHeroStats(currentSelectedHero);

            heroesDataStoreService.ReduceCard(upgradedHeroData, upgradeCardCost);
            heroesDataStoreService.StoreChangedHeroData(upgradedHeroData, selectedAcquiredHeroesByIndex);
        }
        else
        {
            Debug.Log("Not enoght money to upgrade hero !!!");
        }
    }

    private void UpgradeHeroByGold(int upgradeGoldCost)
    {
        if (gamePlayerDataHandler.TrySpendGold(upgradeGoldCost))
        {
            currentSelectedHero.HeroData.UpgradeLevel = currentSelectedHero.HeroData.UpgradeLevel + 1;
            currentSelectedHero.HeroSO.CurrentUpgradeLevel = currentSelectedHero.HeroSO.UpgradeLevelsSO.HeroUpgradeLevelSOs[(int)currentSelectedHero.HeroData.UpgradeLevel - 1];
            heroesUpgradeUI.SetSelectedHeroStats(currentSelectedHero);

            var upgradedHeroData = new HeroData(currentSelectedHero.HeroData.HeroType, currentSelectedHero.HeroData.UpgradeLevel, currentSelectedHero.HeroData.CardType, isAcquired: true, currentSelectedHero.HeroData.Price);
            upgradedHeroData.ReceivedCards = currentSelectedHero.HeroData.ReceivedCards;

            currentSelectedHero.OnHeroDataChange(upgradedHeroData);

            heroesDataStoreService.StoreChangedHeroData(upgradedHeroData, selectedAcquiredHeroesByIndex);
        }
        else
        {
            Debug.Log("Not enoght money to upgrade hero !!!");
        }
    }

    public void SaveSelectedHeroesBeforeStartPlayLevel()
    {
        heroesDataStoreService.StoreSelectedHeroes();
    }

    public bool TryBuyHero()
    {
        if (gamePlayerDataHandler.TrySpendDiamond(currentSelectedHero.HeroData.Price))
        {
            currentSelectedHero.HeroData.IsAcquired = true;
            currentSelectedHero.OnHeroDataChange(currentSelectedHero.HeroData);
            heroesUpgradeUI.SetSelectedHeroStats(currentSelectedHero);
            heroesDataStoreService.StoreChangedHeroData(currentSelectedHero.HeroData, selectedAcquiredHeroesByIndex);

            //AllHeroes.AcquiredHeroes.Add(currentSelectedHero.HeroSO);
            return true;
        }
        else
        {
            Debug.Log("Not Enought Diamonds To Buy");
            return false;
        }
    }

    public void AddTowerCards(List<HeroCard> heroCards)
    {
        heroesDataStoreService.AddCard(heroCards);
    }

    public void ExchangeCards()
    {
        heroesDataStoreService.ExchangeCards();
    }

    public HeroesDataToStore GetStoredHeroesData()
    {
        return heroesDataToStore;
    }
}
