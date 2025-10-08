using TMPro;
using UnityEngine;

public class HeroesUpgradeButtonUI : MonoBehaviour
{
    [Header("Gold Setup")]
    [SerializeField] private RectTransform gold;
    [SerializeField] private TextMeshProUGUI upgradeGoldCostText;
    [Header("Card Setup")]
    [SerializeField] private RectTransform card;
    [SerializeField] private TextMeshProUGUI upgradeCardCostText;
    [Space(5)]
    [SerializeField] private RectTransform[] anchors;



    public void SetParams(int upgradeGoldCost)
    {
        card.gameObject.SetActive(false);
        gold.transform.position = anchors[0].transform.position;
        upgradeGoldCostText.text = upgradeGoldCost.ToString();
    } 
}
