using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardsExchangeHeroCardUI : MonoBehaviour
{
    public HeroSO HeroSO { get => heroSO; }
    public HeroCardType CardType { get => cardType; }
    [field: SerializeField] public HeroData HeroData { get; private set; }

    [SerializeField] private Image cardImage;
    [SerializeField] private Button button;
    [SerializeField] private Image selectImage;
    [SerializeField] private Image highlightImage;
    [SerializeField] private TextMeshProUGUI cardsAmountText;
    private HeroCardType cardType;

    private CardsExchangeControllerUI cardsController;
    private HeroSO heroSO;
    private int iD;


    private void Awake()
    {
        button.onClick.AddListener(OnClick);
    }

    public void SetCardsController(CardsExchangeControllerUI cardsController)
    {
        this.cardsController = cardsController;
    }

    public void SetParams(HeroSO heroSO, int iD)
    {
        this.heroSO = heroSO;
        this.iD = iD;

        HeroData = heroSO.HeroData;
        cardImage.sprite = heroSO.Sprite;
        cardType = heroSO.HeroData.CardType;
        cardsAmountText.text = heroSO.HeroData.ReceivedCards.Count.ToString();
        UnSelect();
    }

    private void OnClick()
    {
        cardsController.SetSelectedHero(this);
    }

    public void UpdateCardsAmount()
    {
        cardsAmountText.text = HeroData.ReceivedCardsAmount.ToString();
    }

    public void DisableButton() => button.enabled = false;
    public void EnableButton() => button.enabled = true;

    public void Select() => selectImage.gameObject.SetActive(true);
    public void UnSelect() => selectImage.gameObject.SetActive(false);

    public void Highlight(bool tof) => highlightImage.gameObject.SetActive(tof);
    public void UnHighlight() => highlightImage.gameObject.SetActive(false);

}
