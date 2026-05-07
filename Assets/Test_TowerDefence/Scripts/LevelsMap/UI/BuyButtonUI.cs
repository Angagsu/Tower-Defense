using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BuyButtonUI : MonoBehaviour
{
    [Header("Gold Setup")]
    [SerializeField] private RectTransform gold;
    [SerializeField] private Sprite goldSprite;
    [SerializeField] private TextMeshProUGUI buyGoldCostText;
    [Header("Diamond Setup")]
    [SerializeField] private RectTransform diamond;
    [SerializeField] private Sprite diamondSprite;
    [SerializeField] private TextMeshProUGUI buyDiamondText;
    [Space(5)]
    [SerializeField] private Image resourceImage;
    [SerializeField] private TextMeshProUGUI towerCostText;
    [SerializeField] private RectTransform[] anchors;

    public void SetParams(bool isAcquird, int cost)
    {
        if (isAcquird)
        {
            resourceImage.sprite = goldSprite; 
        }
        else
        {
            resourceImage.sprite = diamondSprite;
        }

        towerCostText.text = cost.ToString();
    }
}
