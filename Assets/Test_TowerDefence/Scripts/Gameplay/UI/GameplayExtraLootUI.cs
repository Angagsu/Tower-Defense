using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameplayExtraLootUI : MonoBehaviour
{
    [SerializeField] private GameOverHandler gameOverHandler;
    [Header("Gold Setup")]
    [SerializeField] private RectTransform goldTransform;
    [SerializeField] private TextMeshProUGUI goldText;
    [Header("Energy Setup")]
    [SerializeField] private RectTransform energyTransform;
    [SerializeField] private TextMeshProUGUI energyText;
    [Header("Card Setup")]
    [SerializeField] private HeroCardUI heroCardPrefab;
    [SerializeField] private TowerCardUI towerCardPrefab;
    [SerializeField] private RectTransform cardsContainer;
    [SerializeField] private RectTransform[] cardsInstantiatePoints;

    private HeroesDataStoreService heroesDataStoreService;
    private TowersDataStoreService towersDataStoreService;
    private int[] loot;

    private int cardIndex;

    private void OnEnable()
    {
        gameOverHandler.ExtraLootReceived += OnExtraLootRecieve;
    }

    [Inject]
    public void Construct(HeroesDataStoreService heroesDataStoreService, TowersDataStoreService towersDataStoreService)
    {
        this.heroesDataStoreService = heroesDataStoreService;
        this.towersDataStoreService = towersDataStoreService;
    }

    private void OnExtraLootRecieve(int[] loots, List<TowerCard> towerCard, List<HeroCard> heroCards)
    {
        this.loot = loots;

        if (towerCard != null)
        {
            CreatTowerCards(towerCard);
        }

        if (heroCards != null)
        {
            CreatHeroCards(heroCards);
        }

        if (loots.Length == 2)
        {
            //cardsContainer.gameObject.SetActive(false);

            goldText.text = loots[0].ToString();
            energyText.text = loots[1].ToString();

            //goldTransform.localPosition = new Vector3(goldTransform.localPosition.x, goldTransform.localPosition.y - 25, goldTransform.localPosition.z);
            //energyTransform.localPosition = new Vector3(energyTransform.localPosition.x, energyTransform.localPosition.y - 25, goldTransform.localPosition.z);
        }
        else
        {
            goldTransform.gameObject.SetActive(false);
            cardsContainer.gameObject.SetActive(false);

            energyText.text = loots[0].ToString();
            energyTransform.localPosition = new Vector3(cardsContainer.localPosition.x, energyTransform.localPosition.y - 25, cardsContainer.localPosition.z);
        }
    }

    private void CreatTowerCards(List<TowerCard> TowerCards)
    {
        for (int i = 0; i < TowerCards.Count; i++)
        {
            var cardUI = Instantiate(towerCardPrefab, cardsInstantiatePoints[i].transform.position, Quaternion.identity, cardsContainer);
            TowerSO cardConfig = towersDataStoreService.AllTowers.AllTowers[(int)TowerCards[i].TowerType].TowerUpgrades[(int)TowerCards[i].CardType];
            cardUI.SetParams(cardConfig);

            cardIndex = i + 1;
        }
    }

    private void CreatHeroCards(List<HeroCard> heroCards)
    {
        for (int i = 0; i < heroCards.Count; i++)
        {
            var cardUI = Instantiate(heroCardPrefab, cardsInstantiatePoints[i + cardIndex].transform.position, Quaternion.identity, cardsContainer);
            HeroSO cardConfig = heroesDataStoreService.AllHeroes.Heroes[(int)heroCards[i].CardType];
            cardUI.SetCardSprite(cardConfig);
        }
    }

    private void OnDisable()
    {
        gameOverHandler.ExtraLootReceived -= OnExtraLootRecieve;
    }
}
