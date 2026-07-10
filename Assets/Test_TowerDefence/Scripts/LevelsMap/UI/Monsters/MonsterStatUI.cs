using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterStatUI : MonoBehaviour
{
    [SerializeField] private Image statIcon;
    [SerializeField] private Image statBG;
    [SerializeField] private TextMeshProUGUI statNameText;
    [SerializeField] private TextMeshProUGUI statValueText;

    private int id;

    public void SetupSlider(MonsterSO monsterSO, IconsSO iconsSO, int sliderIndex)
    {
        id = sliderIndex;

        if (id == 0 || id == 2)
        {
            statBG.color = new Color(255, 255, 255, 0);
        }

        statIcon.sprite = iconsSO.MonstersSprites[sliderIndex];
        statNameText.text = MonsterAllStatsNames.StatsNames[sliderIndex];
        statValueText.text = monsterSO.StatsValues[sliderIndex].ToString();
    }
}
