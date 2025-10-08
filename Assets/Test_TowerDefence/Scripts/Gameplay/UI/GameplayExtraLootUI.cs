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
    [SerializeField] private CardUI cardPrefab;
    [SerializeField] private RectTransform cardsContainer;
    [SerializeField] private RectTransform[] cardsInstantiatePoints;

    private HeroesDataStoreService heroesDataStoreService;
    private int[] loot;



    private void OnEnable()
    {
        gameOverHandler.ExtraLootReceived += OnExtraLootRecieve;
    }

    [Inject]
    public void Construct(HeroesDataStoreService heroesDataStoreService)
    {
        this.heroesDataStoreService = heroesDataStoreService;
    }

    private void OnExtraLootRecieve(int[] loots, List<Card> cards)
    {
        this.loot = loots;

        if (cards != null)
        {
            CreatCards(cards);
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

    private void CreatCards(List<Card> cards)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            var cardUI = Instantiate(cardPrefab, cardsInstantiatePoints[i].transform.position, Quaternion.identity, cardsContainer);
            HeroSO cardConfig = heroesDataStoreService.AllHeroes.Heroes[(int)cards[i].CardType];
            cardUI.SetParams(cardConfig);
        }
    }

    private void OnDisable()
    {
        gameOverHandler.ExtraLootReceived -= OnExtraLootRecieve;
    }
}
