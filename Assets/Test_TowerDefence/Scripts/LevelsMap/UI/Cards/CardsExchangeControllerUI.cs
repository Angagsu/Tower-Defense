using System.Collections.Generic;
using UnityEngine;


public class CardsExchangeControllerUI : MonoBehaviour
{
    [Header("Toweres Card Setup")]
    [SerializeField] private AllTowersBlueprintSO allTowers;
    [SerializeField] private TowersDataHandler towersDataHandler;
    [SerializeField] private TowerCardExchangeSetup towerCardExchangeSetup;
    [SerializeField] private CardsExchangeTowerCardUI cardsChangerTowerCardPrefab;
    [SerializeField] private RectTransform towerCardsParentTransform;
    [SerializeField] private GameObject towerCardExchangeSetupPanel;

    private List<CardsExchangeTowerCardUI> towerCardsUI = new List<CardsExchangeTowerCardUI>();
    private List<CardsExchangeTowerCardUI> exchangableTowerCardsUI = new List<CardsExchangeTowerCardUI>();

    private CardsExchangeTowerCardUI firstSelectedTowerCard;
    private CardsExchangeTowerCardUI secondSelectedTowerCard;
    private TowersDataToStore towersDataToStore;

    [Header("Heroes Card Setup")]
    [SerializeField] private HeroesFullBlueprintSO allHeroes;
    [SerializeField] private HeroesDataHandler heroesDataHandler;
    [SerializeField] private HeroCardExchangeSetup heroCardExchangeSetup;
    [SerializeField] private CardsExchangeHeroCardUI cardsExchangeHeroCardPrefab;
    [SerializeField] private RectTransform heroCardsParentTransform;
    [SerializeField] private GameObject heroCardExchangeSetupPanel;

    private List<CardsExchangeHeroCardUI> heroCardsUI = new List<CardsExchangeHeroCardUI>();

    private CardsExchangeHeroCardUI firstSelectedHeroCard;
    private CardsExchangeHeroCardUI secondSelectedHeroCard;
    private HeroesDataToStore heroesDataToStore;


    public List<HeroCard> HeroCards;
    public List<TowerCard> TowerCards;


    int costOfChange;
    bool isSecondCardCheaper;




    private void Awake()
    {
        heroesDataToStore = heroesDataHandler.GetStoredHeroesData();
        towersDataToStore = towersDataHandler.GetStoredTowersData();

        towersDataHandler.DataChanged += OnTowerCardDataChanged;
        heroesDataHandler.DataChanged += OnHeroCardDataChanged;
    }
    private void Start()
    {
        CreateHeroCards();
        CreateTowerCards();
    }

    private void OnDestroy()
    {
        towersDataHandler.DataChanged -= OnTowerCardDataChanged;
        heroesDataHandler.DataChanged -= OnHeroCardDataChanged;
    }


    #region Test
    //hero
    private void CreateRandomAmountOfHeroCards(HeroType type)
    {
        int cardsAmount = Random.Range(1, 25);

        for (int i = 0; i < cardsAmount; i++)
        {
            HeroCards.Add(new HeroCard(type));
        }
    }

    [ContextMenu("Add Hero Cards")]
    public void AddHeroCards()
    {
        heroesDataHandler.AddTowerCards(HeroCards);
    }

    //tower
    private void CreatRandomAmountOfTowerCards(TowerType towerType, TowerCardType cardType)
    {
        int cardsAmount = Random.Range(1, 25);

        for (int j = 0; cardsAmount > j; j++)
        {
            TowerCards.Add(new TowerCard(towerType, cardType));
        }
    }

    [ContextMenu("Add Cards")]
    public void AddTowerCards()
    {
        towersDataHandler.AddTowerCards(TowerCards);
    }

    #endregion


    #region TowerCard
    private void CreateTowerCards()
    {
        for (int i = 0; towersDataToStore.AllTowers.Count > i; i++)
        {
            for (int j = 0; towersDataToStore.AllTowers[(TowerType)i].TowerUpgrades.Count > j; j++)
            {
                var tc = Instantiate(cardsChangerTowerCardPrefab, towerCardsParentTransform, true);

                tc.SetCardsController(this);
                tc.SetParams(allTowers.AllTowers[i].TowerUpgrades[j], j);

                CreatRandomAmountOfTowerCards(allTowers.AllTowers[i].TowerUpgrades[j].TowerData.TowerType, (TowerCardType)j);

                towerCardsUI.Add(tc);
            }
        }
    }

    public void SetSelectedTower(CardsExchangeTowerCardUI cardsChangerTowerCardUI)
    {
        if (firstSelectedTowerCard == null && IsTowerCardsReceived(cardsChangerTowerCardUI))
        {
            firstSelectedTowerCard = cardsChangerTowerCardUI;
            firstSelectedTowerCard.Select();

            towerCardExchangeSetupPanel.SetActive(true);
            towerCardExchangeSetup.SetFirstSelectedCard(firstSelectedTowerCard);

            SetHeroCardsInteractable(false);
            heroCardExchangeSetupPanel.SetActive(false);

            CalculateExchangableTowersCards();
        }
        else if (firstSelectedTowerCard == cardsChangerTowerCardUI)
        {
            firstSelectedTowerCard.UnSelect();
            firstSelectedTowerCard = null;
            towerCardExchangeSetup.SetFirstSelectedCard(null);

            SetHeroCardsInteractable(true);
            HideChangableTowerCards();
            costOfChange = 0;
        }
        else if (firstSelectedTowerCard && firstSelectedTowerCard != cardsChangerTowerCardUI && CanBeExchanged(cardsChangerTowerCardUI))
        {
            secondSelectedTowerCard = cardsChangerTowerCardUI;
            secondSelectedTowerCard.Select();

            DetermineTowerCardExchangeCost();
            towerCardExchangeSetup.SetSecondSelectedCard(secondSelectedTowerCard, costOfChange, isSecondCardCheaper);

            StartTowerCardsExchangeProccess();
        }
        else
        {
            if (firstSelectedTowerCard)
            {
                firstSelectedTowerCard.UnSelect();
                firstSelectedTowerCard = null;
                towerCardExchangeSetup.SetFirstSelectedCard(null);
            }

            if (secondSelectedTowerCard)
            {
                secondSelectedTowerCard.UnSelect();
                secondSelectedTowerCard = null;
                towerCardExchangeSetup.SetSecondSelectedCard(null);
            }

            HideChangableTowerCards();
            SetHeroCardsInteractable(true);
            costOfChange = 0;
        }
    }

    private void CalculateExchangableTowersCards()
    {
        for (int i = 0; towerCardsUI.Count > i; i++)
        {
            if (towerCardsUI[i].CardType == firstSelectedTowerCard.CardType)
            {
                exchangableTowerCardsUI.Add(towerCardsUI[i]);
                towerCardsUI[i].Highlight();
            }

            if (towerCardsUI[i].TowerData.TowerType == firstSelectedTowerCard.TowerData.TowerType)
            {
                if (firstSelectedTowerCard.CardType > towerCardsUI[i].CardType)
                {
                    if (firstSelectedTowerCard.TowerData.ReceivedCardsAmount > 0)
                    {
                        exchangableTowerCardsUI.Add(towerCardsUI[i]);
                        towerCardsUI[i].Highlight();
                    }
                }
                else
                {
                    costOfChange = towerCardsUI[i].TowerData.CostOfChange - firstSelectedTowerCard.TowerData.CostOfChange;

                    if (firstSelectedTowerCard.TowerData.ReceivedCardsAmount >= costOfChange)
                    {
                        exchangableTowerCardsUI.Add(towerCardsUI[i]);
                        towerCardsUI[i].Highlight();
                    }
                }
            }
        }
    }

    private void DetermineTowerCardExchangeCost()
    {
        if (firstSelectedTowerCard.CardType == secondSelectedTowerCard.CardType)
        {
            costOfChange = 1;
        }
        else if (firstSelectedTowerCard.CardType > secondSelectedTowerCard.CardType)
        {
            costOfChange = firstSelectedTowerCard.TowerData.CostOfChange - secondSelectedTowerCard.TowerData.CostOfChange;
            isSecondCardCheaper = true;
        }
        else
        {
            costOfChange = secondSelectedTowerCard.TowerData.CostOfChange - firstSelectedTowerCard.TowerData.CostOfChange;
            isSecondCardCheaper = false;
        }
    }

    private void HideChangableTowerCards()
    {
        for (int i = 0; exchangableTowerCardsUI.Count > i; i++)
        {
            exchangableTowerCardsUI[i].UnHighlight();
        }

        exchangableTowerCardsUI.Clear();
    }

    private void StartTowerCardsExchangeProccess()
    {
        HideChangableTowerCards();

        SetTowerCardsInteractable(false);
        SetHeroCardsInteractable(false);

        towerCardExchangeSetup.StartExchangeProccess();
    }

    public void AcceptTowerExchangeProccess()
    {
        firstSelectedTowerCard.UpdateCardsAmount();
        secondSelectedTowerCard.UpdateCardsAmount();
        towersDataHandler.ExchangeCards();
    }

    public void CancelTowerCardsExchangeProccess()
    {
        SetHeroCardsInteractable(true);
        SetTowerCardsInteractable(true);

        firstSelectedTowerCard.UnSelect();
        secondSelectedTowerCard.UnSelect();

        firstSelectedTowerCard = null;
        secondSelectedTowerCard = null;

        towerCardExchangeSetup.SetFirstSelectedCard(null);
        towerCardExchangeSetup.SetSecondSelectedCard(null);

        costOfChange = 0;
        isSecondCardCheaper = false;
    }

    private void SetTowerCardsInteractable(bool tof)
    {
        if (tof)
        {
            for (int i = 0; towerCardsUI.Count > i; i++)
            {
                towerCardsUI[i].EnableButton();
            }
        }
        else
        {
            for (int i = 0; towerCardsUI.Count > i; i++)
            {
                towerCardsUI[i].DisableButton();
            }
        }
    }

    private bool CanBeExchanged(CardsExchangeTowerCardUI cardsChangerTowerCardUI)
    {
        for (int i = 0; exchangableTowerCardsUI.Count > i; i++)
        {
            if (exchangableTowerCardsUI[i] == cardsChangerTowerCardUI)
            {
                return true;
            }
        }
        return false;
    }

    private bool IsTowerCardsReceived(CardsExchangeTowerCardUI cardsChangerTowerCardUI) => cardsChangerTowerCardUI.TowerData.ReceivedCards.Count > 0;

    private void OnTowerCardDataChanged()
    {
        for (int i = 0; towerCardsUI.Count > i; i++)
        {
            towerCardsUI[i].UpdateCardsAmount();
        }
    }

    #endregion


    #region HeroCard

    private void CreateHeroCards()
    {
        for (int i = 0; i < heroesDataToStore.AllHeroes.Count; i++)
        {
            var hc = Instantiate(cardsExchangeHeroCardPrefab, heroCardsParentTransform, true);

            hc.SetCardsController(this);
            hc.SetParams(allHeroes.Heroes[i], i);

            CreateRandomAmountOfHeroCards(allHeroes.Heroes[i].HeroData.HeroType);

            heroCardsUI.Add(hc);
        }
    }

    public void SetSelectedHero(CardsExchangeHeroCardUI cardsExchangeHeroCardUI)
    {
        if (firstSelectedHeroCard == null && IsHeroCardsReceived(cardsExchangeHeroCardUI))
        {
            firstSelectedHeroCard = cardsExchangeHeroCardUI;
            firstSelectedHeroCard.Select();

            heroCardExchangeSetupPanel.SetActive(true);
            heroCardExchangeSetup.SetFirstSelectedCard(firstSelectedHeroCard);

            SetTowerCardsInteractable(false);
            towerCardExchangeSetupPanel.SetActive(false);

            ShowOrHideExchangableHeroCards(true);
        }
        else if (firstSelectedHeroCard == cardsExchangeHeroCardUI)
        {
            firstSelectedHeroCard.UnSelect();
            firstSelectedHeroCard = null;
            heroCardExchangeSetup.SetFirstSelectedCard(null);

            SetTowerCardsInteractable(true);
            ShowOrHideExchangableHeroCards(false);
        }
        else if (firstSelectedHeroCard && firstSelectedHeroCard != cardsExchangeHeroCardUI)
        {
            secondSelectedHeroCard = cardsExchangeHeroCardUI;
            secondSelectedHeroCard.Select();

            costOfChange = 1;
            heroCardExchangeSetup.SetSecondSelectedCard(secondSelectedHeroCard, costOfChange);

            StartHeroCardsExchangeProccess();
        }
        else
        {
            if (firstSelectedHeroCard != null)
            {
                firstSelectedHeroCard.UnSelect();
                firstSelectedHeroCard = null;
                heroCardExchangeSetup.SetFirstSelectedCard(null);
            }

            if (secondSelectedHeroCard != null)
            {
                secondSelectedHeroCard.UnSelect();
                secondSelectedHeroCard = null;
                heroCardExchangeSetup.SetSecondSelectedCard(null);
            }

            SetTowerCardsInteractable(true);

            costOfChange = 0;
        }
    }

    private void ShowOrHideExchangableHeroCards(bool tof)
    {
        for (int i = 0; heroCardsUI.Count > i; i++)
        {
            heroCardsUI[i].Highlight(tof);
        }
    }

    private void StartHeroCardsExchangeProccess()
    {
        ShowOrHideExchangableHeroCards(false);

        SetTowerCardsInteractable(false);
        SetHeroCardsInteractable(false);

        heroCardExchangeSetup.StartExchangeProccess();
    }

    public void AcceptHeroCardExchangeProcces()
    {
        firstSelectedHeroCard.UpdateCardsAmount();
        secondSelectedHeroCard.UpdateCardsAmount();
        heroesDataHandler.ExchangeCards();
    }

    public void CancelHeroCardsExchangeProccess()
    {
        SetTowerCardsInteractable(true);
        SetHeroCardsInteractable(true);

        firstSelectedHeroCard.UnSelect();
        secondSelectedHeroCard.UnSelect();

        firstSelectedHeroCard = null;
        secondSelectedHeroCard = null;

        heroCardExchangeSetup.SetFirstSelectedCard(firstSelectedHeroCard);
        heroCardExchangeSetup.SetSecondSelectedCard(secondSelectedHeroCard);

        costOfChange = 0;
    }

    private void SetHeroCardsInteractable(bool tof)
    {
        if (tof)
        {
            for (int i = 0; heroCardsUI.Count > i; i++)
            {
                heroCardsUI[i].EnableButton();
            }
        }
        else
        {
            for (int i = 0; heroCardsUI.Count > i; i++)
            {
                heroCardsUI[i].DisableButton();
            }
        }
    }

    private bool IsHeroCardsReceived(CardsExchangeHeroCardUI cardsChangerHeroCardUI) => cardsChangerHeroCardUI.HeroData.ReceivedCards.Count > 0;

    private void OnHeroCardDataChanged()
    {
        for (int i = 0; heroCardsUI.Count > i; i++)
        {
            heroCardsUI[i].UpdateCardsAmount();
        }
    }

    #endregion

}
