using System.Collections.Generic;
using UnityEngine;

public class TowersUpgradesStatsHandlerUI : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private TowerStatSliderUI sliderPrefab;
    [SerializeField] private RectTransform statsSlidersContainer;
    [SerializeField] private RectTransform[] statsSlidersTransforms;
    [Header("Passive Skills")]
    [SerializeField] private TowerPassiveSkillSliderUI passiveSkillSliderPrefab;
    [SerializeField] private RectTransform passiveSklillSlidersContainer;
    [SerializeField] private RectTransform[] passiveSkillsSlidersTransforms;
    [Header("Attack And Defense Stats")]
    [SerializeField] private TowerAttackAndDefenseStatsUI attackAndDefenseStatsPrefab;
    [SerializeField] private RectTransform attackAndDefenseStatsContainer;
    [SerializeField] private RectTransform[] attackAndDefenseStatsTransforms;
    [Space(10)]
    [SerializeField] private TowerPassiveSkillInformPopupUI informPopupUI;
    [SerializeField] private IconsSO iconsSO;

    private List<TowerStatSliderUI> sliders = new();
    private List<TowerPassiveSkillSliderUI> passiveSkillSliders = new();
    private List<TowerAttackAndDefenseStatsUI> attackAndDefenseStats = new();

    public void UpdateTowerStats(TowerSO cureentUpgradeTowerSO, TowerUpgradeLevelSO nextUpgradeTowerSO = null)
    {
        CleareSliders();

        if (cureentUpgradeTowerSO.PassiveSkillsAmount != 0)
        {
            CreatePassiveSkillSliders(cureentUpgradeTowerSO, nextUpgradeTowerSO);
        }

        CreateAttackAndDefenseStats(cureentUpgradeTowerSO, nextUpgradeTowerSO);

        if (cureentUpgradeTowerSO.TowerData.TowerType != TowerType.Defender)
        {
            CreateStatSliders(cureentUpgradeTowerSO, nextUpgradeTowerSO);
        }
        else
        {
            CreateDefenderTowerStatsSlider(cureentUpgradeTowerSO, nextUpgradeTowerSO);
        }  
    }

    private void CreatePassiveSkillSliders(TowerSO towerSO, TowerUpgradeLevelSO nextUpgradeLevel = null)
    {
        for (int i = 0; i < towerSO.PassiveSkillsAmount; i++)
        {
            passiveSkillSliders.Add(Instantiate(passiveSkillSliderPrefab, passiveSkillsSlidersTransforms[i].position, passiveSkillsSlidersTransforms[i].rotation, passiveSklillSlidersContainer));
            passiveSkillSliders[i].SetupSlider(informPopupUI, towerSO, i, nextUpgradeLevel);
        }
    }

    private void CreateAttackAndDefenseStats(TowerSO cureentUpgradeTowerSO, TowerUpgradeLevelSO nextUpgradeLevel = null)
    {
        for (int i = 0; i < 2; i++)
        {
            attackAndDefenseStats.Add(Instantiate(attackAndDefenseStatsPrefab, attackAndDefenseStatsTransforms[i].position, attackAndDefenseStatsTransforms[i].rotation, attackAndDefenseStatsContainer));
            attackAndDefenseStats[i].SetupSlider(informPopupUI, iconsSO, cureentUpgradeTowerSO, i);
        }
    }

    private void CreateStatSliders(TowerSO cureentUpgradeTowerSO, TowerUpgradeLevelSO nextUpgradeLevel = null)
    {
        for (int i = 0; i < 5; i++)
        {
            sliders.Add(Instantiate(sliderPrefab, statsSlidersTransforms[i].position, statsSlidersTransforms[i].rotation, statsSlidersContainer));
            sliders[i].SetupSlider(cureentUpgradeTowerSO, iconsSO, sliderIndex: i, nextUpgradeLevel);
        }
    }

    private void CreateDefenderTowerStatsSlider(TowerSO cureentUpgradeTowerSO, TowerUpgradeLevelSO nextUpgradeTowerSO = null)
    {
        for (int i = 0; i < 3; i++)
        {
            sliders.Add(Instantiate(sliderPrefab, statsSlidersTransforms[i].position, statsSlidersTransforms[i].rotation, statsSlidersContainer));
            sliders[i].SetupSlider(cureentUpgradeTowerSO, iconsSO, sliderIndex: i, nextUpgradeTowerSO);
        }

        sliders.Add(Instantiate(sliderPrefab, statsSlidersTransforms[3].position, statsSlidersTransforms[3].rotation, statsSlidersContainer));
        sliders[3].SetupSlider(cureentUpgradeTowerSO, iconsSO, sliderIndex: 4, nextUpgradeTowerSO);
        sliders.Add(Instantiate(sliderPrefab, statsSlidersTransforms[4].position, statsSlidersTransforms[4].rotation, statsSlidersContainer));
        sliders[4].SetupSlider(cureentUpgradeTowerSO, iconsSO, sliderIndex: 5, nextUpgradeTowerSO);
    }

    public void CleareSliders()
    {
        foreach (var slider in sliders)
        {
            Destroy(slider.gameObject);
        }

        foreach (var passiveSkillSlider in passiveSkillSliders)
        {
            Destroy(passiveSkillSlider.gameObject);
        }

        foreach (var attackAndDefenseStat in attackAndDefenseStats)
        {
            Destroy(attackAndDefenseStat.gameObject);
        }

        sliders.Clear();
        passiveSkillSliders.Clear();
        attackAndDefenseStats.Clear();
    }

}


