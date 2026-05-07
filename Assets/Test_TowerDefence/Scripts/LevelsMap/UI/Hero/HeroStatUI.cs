using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class HeroStatUI : MonoBehaviour
{
    [SerializeField] private Image statIcon;
    [SerializeField] private Image statBG;
    [SerializeField] private Image increaseImage;
    [SerializeField] private TextMeshProUGUI statNameText;
    [SerializeField] private TextMeshProUGUI currentUpgradeValueText;
    [SerializeField] private TextMeshProUGUI nextUpgradeValueText;

    private int id;

    public void SetupSlider(HeroSO heroSO, IconsSO iconsSO, int sliderIndex, HeroUpgradeLevelSO nextUpgradeLevel = null)
    {
        id = sliderIndex;

        if (id == 1)
        {
            statBG.color = new Color(255, 255, 255, 0);
        }

        statIcon.sprite = iconsSO.AllHeroIconsSprites[sliderIndex];
        currentUpgradeValueText.text = heroSO.CurrentUpgradeLevel.StatsValues[sliderIndex].ToString();
        statNameText.text = HeroAllStatsNames.StatsNames[sliderIndex];

        if (nextUpgradeLevel)
        {
            if (nextUpgradeLevel.StatsValues[sliderIndex] > heroSO.CurrentUpgradeLevel.StatsValues[sliderIndex])
            {
                nextUpgradeValueText.text = nextUpgradeLevel.StatsValues[sliderIndex].ToString();
                increaseImage.gameObject.SetActive(true);
            }
            else
            {
                nextUpgradeValueText.text = "";
                increaseImage.gameObject.SetActive(false);
            }
        }
        else
        {
            nextUpgradeValueText.text = "";
            increaseImage.gameObject.SetActive(false);
        }
    }
}
