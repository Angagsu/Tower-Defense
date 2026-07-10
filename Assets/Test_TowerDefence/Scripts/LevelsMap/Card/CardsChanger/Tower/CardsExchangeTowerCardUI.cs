using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardsExchangeTowerCardUI : MonoBehaviour
{
    public TowerSO TowerSO { get => towerSO; }
    public TowerCardType CardType { get => cardType; }
    [field: SerializeField] public TowerData TowerData { get; private set; }

    [SerializeField] private Image cardImage;
    [SerializeField] private Button button;
    [SerializeField] private Image selectImage;
    [SerializeField] private Image highlightImage;
    [SerializeField] private TextMeshProUGUI cardsAmountText;
    private TowerCardType cardType;

    private CardsExchangeControllerUI cardsController;
    private TowerSO towerSO;
    private int iD;


    private void Awake()
    {
        button.onClick.AddListener(OnClick);
    }

    public void SetCardsController(CardsExchangeControllerUI cardsController)
    {
        this.cardsController = cardsController;
    }

    public void SetParams(TowerSO towerSO, int iD)
    {
        this.towerSO = towerSO;
        this.iD = iD;

        TowerData = towerSO.TowerData;
        cardImage.sprite = towerSO.TowerSprite;
        cardType = towerSO.TowerData.CardType;
        cardsAmountText.text = towerSO.TowerData.ReceivedCards.Count.ToString();
        UnSelect();
    }

    private void OnClick()
    {
        cardsController.SetSelectedTower(this);
    }

    public void UpdateCardsAmount()
    {
        cardsAmountText.text = TowerData.ReceivedCardsAmount.ToString();
    }

    public void DisableButton () => button.enabled = false;
    public void EnableButton() => button.enabled = true;

    public void Select() => selectImage.gameObject.SetActive(true);
    public void UnSelect() => selectImage.gameObject.SetActive(false);

    public void Highlight() => highlightImage.gameObject.SetActive(true);
    public void UnHighlight() => highlightImage.gameObject.SetActive(false);

    
}
