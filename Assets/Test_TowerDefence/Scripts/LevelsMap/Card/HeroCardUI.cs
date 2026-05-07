using UnityEngine;
using UnityEngine.UI;

public class HeroCardUI : MonoBehaviour
{
    [SerializeField] private Image cardImage;

    private HeroCardType cardType;

    public void SetParams(HeroSO heroSO)
    {
        cardImage.sprite = heroSO.CardSprite;
        cardType = heroSO.HeroData.CardType;
    }

    public void SetCardSprite(HeroSO heroSO)
    {
        cardImage.sprite = heroSO.Sprite;
    }
}
