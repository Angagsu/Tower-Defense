using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerCardExchangeSetup : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button increaseButton;
    [SerializeField] private Button decreaseButton;
    [SerializeField] private Button maxButton;
    [SerializeField] private Button acceptExchangeButton;
    [SerializeField] private Button cancelExchangeButton;
    [SerializeField] private TextMeshProUGUI cardAmountText;

    [SerializeField] private CardsExchangeSelectedTowerCardUI firstSelectedTowerCardsUI;
    [SerializeField] private CardsExchangeSelectedTowerCardUI secondSelectedTowerCardsUI;
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

    public void SetFirstSelectedCard(CardsExchangeTowerCardUI cardsChangerTowerCardUI)
    {
        firstSelectedTowerCardsUI.SetSelectedTowerCard(cardsChangerTowerCardUI);
    }

    public void SetSecondSelectedCard(CardsExchangeTowerCardUI cardsChangerTowerCardUI, int costOfChange = default, bool isSecondCardCheaper = false)
    {
        this.costOfChange = costOfChange;
        this.isSecondCardCheaper = isSecondCardCheaper;

        secondSelectedTowerCardsUI.SetSelectedTowerCard(cardsChangerTowerCardUI);

        cardAmountText.text = "0";

        firstCardExchangeThreshold = firstSelectedTowerCardsUI.TowerData.ReceivedCardsAmount;
        secondCardExchangeThreshold = secondSelectedTowerCardsUI.TowerData.ReceivedCardsAmount;
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
        else if(costOfChange == 1)
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

        secondSelectedTowerCardsUI.UpdateCartsAmountText(cardsAmount: secondCardToExchangeAmount);
        firstSelectedTowerCardsUI.UpdateCartsAmountText(cardsAmount: firstCardToExchangeAmount);
    }

    private void OnDecreaseButtonClick()
    {
        if(firstCardToExchangeAmount <= 0)
        {
            return;
        }

        if (!isSecondCardCheaper)
        {
            firstCardToExchangeAmount -= costOfChange; 
            secondCardToExchangeAmount--;
            cardAmountText.text = secondCardToExchangeAmount.ToString();

            secondSelectedTowerCardsUI.UpdateCartsAmountText(cardsAmount: secondCardToExchangeAmount);
            firstSelectedTowerCardsUI.UpdateCartsAmountText(cardsAmount: firstCardToExchangeAmount);
        }
        else
        {
            firstCardToExchangeAmount--;
            secondCardToExchangeAmount -= costOfChange;
            cardAmountText.text = secondCardToExchangeAmount.ToString();

            secondSelectedTowerCardsUI.UpdateCartsAmountText(cardsAmount: secondCardToExchangeAmount);
            firstSelectedTowerCardsUI.UpdateCartsAmountText(cardsAmount: firstCardToExchangeAmount);
        }
    }

    private void OnIncreaseButtonClick()
    {
        if(firstCardExchangeThreshold <= firstCardToExchangeAmount)
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

            secondSelectedTowerCardsUI.UpdateCartsAmountText(cardsAmount: secondCardToExchangeAmount);
            firstSelectedTowerCardsUI.UpdateCartsAmountText(cardsAmount: firstCardToExchangeAmount);
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

            secondSelectedTowerCardsUI.UpdateCartsAmountText(cardsAmount: secondCardToExchangeAmount);
            firstSelectedTowerCardsUI.UpdateCartsAmountText(cardsAmount: firstCardToExchangeAmount);
        }
    }

    private void OnCancelExchangeButtonClick()
    {
        firstCardToExchangeAmount = 0;
        costOfChange = 0;
        secondCardToExchangeAmount = 0;

        cardAmountText.text = firstCardToExchangeAmount.ToString();
        DisableButtons();
        cardsController.CancelTowerCardsExchangeProccess();
    }

    private void OnAcceptExchangeButtonClick()
    {
        firstSelectedTowerCardsUI.TowerData.ReceivedCards.RemoveRange(0, firstCardToExchangeAmount);

        List<TowerCard> cards = new();

        for(int i = 0; i < secondCardToExchangeAmount; i++)
        {
            TowerCard card = new(secondSelectedTowerCardsUI.TowerData.TowerType, secondSelectedTowerCardsUI.CardType);
            cards.Add(card);
        }

        secondSelectedTowerCardsUI.TowerData.ReceivedCards.AddRange(cards);

        cardsController.AcceptTowerExchangeProccess();
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
