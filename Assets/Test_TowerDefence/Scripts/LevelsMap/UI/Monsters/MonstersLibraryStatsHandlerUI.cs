using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MonstersLibraryStatsHandlerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI monsterNameText;
    [Header("Stats")]
    [SerializeField] private MonsterStatUI statPrefab;
    [SerializeField] private RectTransform statsContainer;
    [SerializeField] private RectTransform[] statsSlidersTransforms;
    [Header("Attack And Defense Stats")]
    [SerializeField] private MonsterAttackAndDefenseStatsUI attackAndDefenseStatsPrefab;
    [SerializeField] private RectTransform attackAndDefenseStatsContainer;
    [SerializeField] private RectTransform[] attackAndDefenseStatsTransforms;
    [Space(10)]
    [SerializeField] private MonsterStatsInformPopupUI informPopupUI;
    [SerializeField] private IconsSO iconsSO;


    private List<MonsterStatUI> sliders = new();
    private List<MonsterAttackAndDefenseStatsUI> attackAndDefenseStats = new();


    public void UpdateStats(MonsterSO monsterSO)
    {
        monsterNameText.text = monsterSO.Name;
        CleareSliders();

        CreateStatSliders(monsterSO);

        CreateAttackAndDefenseStats(monsterSO);
    }

    private void CreateStatSliders(MonsterSO monsterSO)
    {
        for (int i = 0; i < MonsterAllStatsNames.StatsNames.Count; i++)
        {
            sliders.Add(Instantiate(statPrefab, statsSlidersTransforms[i].position, statsSlidersTransforms[i].rotation, statsContainer));
            sliders[i].SetupSlider(monsterSO, iconsSO, sliderIndex: i);
        }
    }

    private void CreateAttackAndDefenseStats(MonsterSO monsterSO)
    {
        for (int i = 0; i < 2; i++)
        {
            attackAndDefenseStats.Add(Instantiate(attackAndDefenseStatsPrefab, attackAndDefenseStatsTransforms[i].position, attackAndDefenseStatsTransforms[i].rotation, attackAndDefenseStatsContainer));
            attackAndDefenseStats[i].SetupSlider(informPopupUI, iconsSO, monsterSO, i);
        }
    }

    public void CleareSliders()
    {
        foreach (var slider in sliders)
        {
            Destroy(slider.gameObject);
        }

        foreach (var attackAndDefenseStat in attackAndDefenseStats)
        {
            Destroy(attackAndDefenseStat.gameObject);
        }

        attackAndDefenseStats.Clear();
        sliders.Clear();
    }
}
