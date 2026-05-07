using UnityEngine;
using UnityEngine.UI;

public class TowerCardUI : MonoBehaviour
{
    [SerializeField] private Image cardImage;

    private TowerCardType cardType;

    public void SetParams(TowerSO towerSO)
    {
        cardImage.sprite = towerSO.TowerSprite;
        cardType = towerSO.TowerData.CardType;
    }
}
