using TMPro;
using UnityEngine;

public class UpgradeButtonUI : MonoBehaviour
{
    [Header("Gold Setup")]
    [SerializeField] private RectTransform gold;
    [SerializeField] private TextMeshProUGUI upgradeGoldCostText;
    [Header("Card Setup")]
    [SerializeField] private RectTransform card;
    [SerializeField] private TextMeshProUGUI upgradeCardCostText;
    [Space(5)]
    [SerializeField] private RectTransform[] anchors;



    public void SetParams(int upgradeGoldCost, int upgradeCardCost)
    {
        if(upgradeCardCost == 0)
        {
            card.gameObject.SetActive(false);
            gold.transform.position = anchors[0].transform.position;
            upgradeGoldCostText.text = upgradeGoldCost.ToString();
        }
        else
        {
            card.gameObject.SetActive(true);
            gold.transform.position = anchors[1].position;
            card.transform.position = anchors[2].position;
            upgradeGoldCostText.text = upgradeGoldCost.ToString();
            upgradeCardCostText.text = upgradeCardCost.ToString();
        } 
    } 
}
