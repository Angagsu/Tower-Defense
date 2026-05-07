using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerStatSliderUI : MonoBehaviour
{
    [SerializeField] private Image sliderImage;
    [SerializeField] private Image increaseImage;  
    [SerializeField] private TextMeshProUGUI currentUpgradeValueText;
    [SerializeField] private TextMeshProUGUI nextUpgradeValueText;
    [SerializeField] private TextMeshProUGUI statNameText;

    private int id;

    public void SetupSlider(TowerSO currentUpgradeTowerSO, IconsSO iconsSO, int sliderIndex, TowerUpgradeLevelSO nextUpgradeLevel = null)
    {
        id = sliderIndex;

        sliderImage.sprite = iconsSO.AllIconsSprites[sliderIndex];

        currentUpgradeValueText.text = currentUpgradeTowerSO.CurrentUpgradeLevel.StatsValues[sliderIndex].ToString();
        statNameText.text = TowerAllStatsNames.StatsNames[sliderIndex];

        if (nextUpgradeLevel)
        {
            if (nextUpgradeLevel.StatsValues[sliderIndex] != currentUpgradeTowerSO.CurrentUpgradeLevel.StatsValues[sliderIndex])
            {
                nextUpgradeValueText.text = nextUpgradeLevel.StatsValues[sliderIndex].ToString();
                increaseImage.gameObject.SetActive(true);
            }
            else
            {
                nextUpgradeValueText.text = "";
                increaseImage.gameObject.SetActive(false);
            }
            
        }else
        {
            nextUpgradeValueText.text = "";
            increaseImage.gameObject.SetActive(false);
        }
    }
}
