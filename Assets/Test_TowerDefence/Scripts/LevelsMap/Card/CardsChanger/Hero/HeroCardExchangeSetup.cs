using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroCardExchangeSetup : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button increaseButton;
    [SerializeField] private Button decreaseButton;
    [SerializeField] private Button maxButton;
    [SerializeField] private Button acceptExchangeButton;
    [SerializeField] private Button cancelExchangeButton;
    [SerializeField] private TextMeshProUGUI cardAmountText;

    [SerializeField] private CardsExchangeSelectedHeroCardUI firstSelectedHeroCardsUI;
    [SerializeField] private CardsExchangeSelectedHeroCardUI secondSelectedHeroCardsUI;
    [SerializeField] private CardsExchangeControllerUI cardsController;

    private int firstCardToExchangeAmount;
    private int costOfChange;
    private int secondCardToExchangeAmount;
    private bool isSecondCardCheaper;

    private int firstCardExchangeThreshold;
    private int secondCardExchangeThreshold;

    private void Awake()
    {
        increaseButton.onClick.AddListener(OnIncreaseButtonClick);
        decreaseButton.onClick.AddListener(OnDecreaseButtonClick);
        maxButton.onClick.AddListener(OnMaxButtonClick);
        acceptExchangeButton.onClick.AddListener(OnAcceptExchangeButtonClick);
        cancelExchangeButton.onClick.AddListener(OnCancelExchangeButtonClick);

        DisableButtons();
    }

    public void SetFirstSelectedCard(CardsExchangeHeroCardUI cardsChangerTowerCardUI)
    {
        firstSelectedHeroCardsUI.SetSelectedTowerCard(cardsChangerTowerCardUI);
    }

    public void SetSecondSelectedCard(CardsExchangeHeroCardUI cardsChangerTowerCardUI, int costOfChange = default, bool isSecondCardCheaper = false)
    {
        this.costOfChange = costOfChange;
        this.isSecondCardCheaper = isSecondCardCheaper;

        secondSelectedHeroCardsUI.SetSelectedTowerCard(cardsChangerTowerCardUI);

        cardAmountText.text = "0";

        firstCardExchangeThreshold = firstSelectedHeroCardsUI.HeroData.ReceivedCardsAmount;
        secondCardExchangeThreshold = secondSelectedHeroCardsUI.HeroData.ReceivedCardsAmount;
    }


    public void StartExchangeProccess()
    {
        EnableButtons();
    }

    private void OnMaxButtonClick()
    {
        if (!isSecondCardCheaper)
        {
            firstCardToExchangeAmount = firstCardExchangeThreshold - (firstCardExchangeThreshold % costOfChange);
            secondCardToExchangeAmount = firstCardToExchangeAmount / costOfChange;
            cardAmountText.text = secondCardToExchangeAmount.ToString();
        }
        else if (costOfChange == 1)
        {
            firstCardToExchangeAmount = firstCardExchangeThreshold;
            secondCardToExchangeAmount = firstCardToExchangeAmount;
            cardAmountText.text = firstCardToExchangeAmount.ToString();
        }
        else
        {
            firstCardToExchangeAmount = firstCardExchangeThreshold;
            secondCardToExchangeAmount = firstCardToExchangeAmount * costOfChange;
            cardAmountText.text = secondCardToExchangeAmount.ToString();
        }

        secondSelectedHeroCardsUI.UpdateCartsAmountText(cardsAmount: secondCardToExchangeAmount);
        firstSelectedHeroCardsUI.UpdateCartsAmountText(cardsAmount: firstCardToExchangeAmount);
    }

    private void OnDecreaseButtonClick()
    {
        if (firstCardToExchangeAmount <= 0)
        {
            return;
        }

        if (!isSecondCardCheaper)
        {
            firstCardToExchangeAmount -= costOfChange;
            secondCardToExchangeAmount--;
            cardAmountText.text = secondCardToExchangeAmount.ToString();

            secondSelectedHeroCardsUI.UpdateCartsAmountText(cardsAmount: secondCardToExchangeAmount);
            firstSelectedHeroCardsUI.UpdateCartsAmountText(cardsAmount: firstCardToExchangeAmount);
        }
        else
        {
            firstCardToExchangeAmount--;
            secondCardToExchangeAmount -= costOfChange;
            cardAmountText.text = secondCardToExchangeAmount.ToString();

            secondSelectedHeroCardsUI.UpdateCartsAmountText(cardsAmount: secondCardToExchangeAmount);
            firstSelectedHeroCardsUI.UpdateCartsAmountText(cardsAmount: firstCardToExchangeAmount);
        }
    }

    private void OnIncreaseButtonClick()
    {
        if (firstCardExchangeThreshold <= firstCardToExchangeAmount)
        {
            return;
        }

        if (!isSecondCardCheaper)
        {
            firstCardToExchangeAmount += costOfChange;

            if (firstCardToExchangeAmount > firstCardExchangeThreshold)
            {
                firstCardToExchangeAmount -= costOfChange;
                return;
            }

            secondCardToExchangeAmount++;
            cardAmountText.text = secondCardToExchangeAmount.ToString();

            secondSelectedHeroCardsUI.UpdateCartsAmountText(cardsAmount: secondCardToExchangeAmount);
            firstSelectedHeroCardsUI.UpdateCartsAmountText(cardsAmount: firstCardToExchangeAmount);
        }
        else
        {
            firstCardToExchangeAmount++;

            if (firstCardToExchangeAmount > firstCardExchangeThreshold)
            {
                firstCardToExchangeAmount--;
                return;
            }

            secondCardToExchangeAmount += costOfChange;
            cardAmountText.text = secondCardToExchangeAmount.ToString();

            secondSelectedHeroCardsUI.UpdateCartsAmountText(cardsAmount: secondCardToExchangeAmount);
            firstSelectedHeroCardsUI.UpdateCartsAmountText(cardsAmount: firstCardToExchangeAmount);
        }
    }

    private void OnCancelExchangeButtonClick()
    {
        firstCardToExchangeAmount = 0;
        costOfChange = 0;
        secondCardToExchangeAmount = 0;

        cardAmountText.text = firstCardToExchangeAmount.ToString();
        DisableButtons();
        cardsController.CancelHeroCardsExchangeProccess();
    }

    private void OnAcceptExchangeButtonClick()
    {
        firstSelectedHeroCardsUI.HeroData.ReceivedCards.RemoveRange(0, firstCardToExchangeAmount);

        List<HeroCard> cards = new();

        for (int i = 0; i < secondCardToExchangeAmount; i++)
        {
            HeroCard card = new(secondSelectedHeroCardsUI.HeroData.HeroType);
            cards.Add(card);
        }

        secondSelectedHeroCardsUI.HeroData.ReceivedCards.AddRange(cards);

        cardsController.AcceptHeroCardExchangeProcces();
        OnCancelExchangeButtonClick();
    }

    private void EnableButtons()
    {
        increaseButton.interactable = true;
        decreaseButton.interactable = true;
        maxButton.interactable = true;
        acceptExchangeButton.interactable = true;
        cancelExchangeButton.interactable = true;

        cardAmountText.enabled = true;
    }

    private void DisableButtons()
    {
        increaseButton.interactable = false;
        decreaseButton.interactable = false;
        maxButton.interactable = false;
        acceptExchangeButton.interactable = false;
        cancelExchangeButton.interactable = false;

        cardAmountText.enabled = false;
    }

    private void OnDestroy()
    {
        increaseButton.onClick.RemoveListener(OnIncreaseButtonClick);
        decreaseButton.onClick.RemoveListener(OnDecreaseButtonClick);
        maxButton.onClick.RemoveListener(OnMaxButtonClick);
        acceptExchangeButton.onClick.RemoveListener(OnAcceptExchangeButtonClick);
        cancelExchangeButton.onClick.RemoveListener(OnCancelExchangeButtonClick);
    }
}
