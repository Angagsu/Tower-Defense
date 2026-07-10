using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardsExchangeSelectedHeroCardUI : MonoBehaviour
{
    public HeroSO HeroSO { get => heroSO; }
    public HeroCardType CardType { get => cardType; }
    public HeroData HeroData { get; private set; }

    [SerializeField] private Image cardImage;
    [SerializeField] private Image selectImage;
    [SerializeField] private Image highlightImage;
    [SerializeField] private TextMeshProUGUI cardsAmountText;
    [SerializeField] private int id;

    private HeroCardType cardType;
    private HeroSO heroSO;
    private CardsExchangeHeroCardUI selectedTowerCard;
    private bool isSecondCardCheaper;

    public void OnHeroDataChange(HeroData heroData)
    {
        HeroData.UpgradeLevel = heroData.UpgradeLevel;
    }

    public void SetSelectedTowerCard(CardsExchangeHeroCardUI selectedHeroCard, bool isSecondCardCheaper = false)
    {
        if (selectedHeroCard == null)
        {
            this.selectedTowerCard = null;
            selectImage.gameObject.SetActive(false);
            cardsAmountText.text = "";
            return;
        }

        heroSO = selectedHeroCard.HeroSO;
        HeroData = selectedHeroCard.HeroSO.HeroData;
        this.selectedTowerCard = selectedHeroCard;
        cardType = selectedHeroCard.CardType;
        selectImage.sprite = selectedHeroCard.HeroSO.Sprite;
        selectImage.gameObject.SetActive(true);

        this.isSecondCardCheaper = isSecondCardCheaper;

        if (id == 0)
        {
            if (!isSecondCardCheaper)
            {
                cardsAmountText.text = HeroData.ReceivedCardsAmount.ToString();
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

                cardsAmountText.text = HeroData.ReceivedCardsAmount.ToString();
            }
            else
            {
                cardsAmountText.text = "1";
            }
        }

    }

    public void UpdateCartsAmountText(int cardsAmount)
    {
        if (id == 0)
        {
            if (isSecondCardCheaper)
            {
                cardsAmountText.text = HeroData.ReceivedCardsAmount.ToString();
            }
            else
            {
                cardsAmountText.text = (HeroData.ReceivedCardsAmount - cardsAmount).ToString();
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
                cardsAmountText.text = (HeroData.ReceivedCardsAmount + cardsAmount).ToString();
            }
        }
    }



    public void Select() => selectImage.gameObject.SetActive(true);

    public void UnSelect() => selectImage.gameObject.SetActive(false);

    public void Highlight() => highlightImage.gameObject.SetActive(true);

    public void UnHighlight() => highlightImage.gameObject.SetActive(false);
}
