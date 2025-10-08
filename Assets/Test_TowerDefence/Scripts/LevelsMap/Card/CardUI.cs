using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
    [SerializeField] private Image cardImage;

    private HeroCardType cardType;

    public void SetParams(HeroSO heroSO)
    {
        cardImage.sprite = heroSO.Sprite;
        cardType = heroSO.HeroData.CardType;
    }
}
