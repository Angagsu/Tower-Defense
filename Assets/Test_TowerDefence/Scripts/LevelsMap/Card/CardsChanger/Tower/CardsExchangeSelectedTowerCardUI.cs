using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardsExchangeSelectedTowerCardUI : MonoBehaviour
{
    public TowerSO TowerSO { get => towerSO; }
    public TowerCardType CardType { get => cardType; }
    public TowerData TowerData { get; private set; }

    [SerializeField] private Image cardImage;
    [SerializeField] private Image selectImage;
    [SerializeField] private Image highlightImage;
    [SerializeField] private TextMeshProUGUI cardsAmountText;
    [SerializeField] private int id;

    private TowerCardType cardType;
    private TowerSO towerSO;
    private CardsExchangeTowerCardUI selectedTowerCard;
    private bool isSecondCardCheaper;

    public void OnHeroDataChange(TowerData towerData)
    {
        TowerData.UpgradeLevel = towerData.UpgradeLevel;
    }

    public void SetSelectedTowerCard(CardsExchangeTowerCardUI selectedTowerCard, bool isSecondCardCheaper = false)
    {
        if (selectedTowerCard == null)
        {
            this.selectedTowerCard = null;
            selectImage.gameObject.SetActive(false);
            cardsAmountText.text = "";
            return;
        }

        towerSO = selectedTowerCard.TowerSO;
        TowerData = selectedTowerCard.TowerSO.TowerData;
        this.selectedTowerCard = selectedTowerCard;
        cardType = selectedTowerCard.CardType;
        selectImage.sprite = selectedTowerCard.TowerSO.TowerSprite;
        selectImage.gameObject.SetActive(true);

        this.isSecondCardCheaper = isSecondCardCheaper;

        if(id == 0)
        {
            if (!isSecondCardCheaper)
            {
                cardsAmountText.text = TowerData.ReceivedCardsAmount.ToString();
            }
            else
            {
                cardsAmountText.text = "1";
            }
        }
        else
        {
            if (!isSecondCardCheaper)
            {
                
                cardsAmountText.text = TowerData.ReceivedCardsAmount.ToString();
            }
            else
            {
                cardsAmountText.text = "1";
            }
        }

    }

    public void UpdateCartsAmountText(int cardsAmount)
    {
        if(id == 0)
        {
            if (isSecondCardCheaper)
            {
                cardsAmountText.text = TowerData.ReceivedCardsAmount.ToString();
            }
            else
            {
                cardsAmountText.text = (TowerData.ReceivedCardsAmount - cardsAmount).ToString();
            }
        }
        else
        {
            if (isSecondCardCheaper)
            {
                
                cardsAmountText.text = cardsAmount.ToString();
            }
            else
            {
                cardsAmountText.text = (TowerData.ReceivedCardsAmount + cardsAmount).ToString();
            }
        }
    }



    public void Select() => selectImage.gameObject.SetActive(true);

    public void UnSelect() => selectImage.gameObject.SetActive(false);

    public void Highlight() => highlightImage.gameObject.SetActive(true);

    public void UnHighlight() => highlightImage.gameObject.SetActive(false);
}
