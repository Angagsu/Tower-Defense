using System.Collections.Generic;
using UnityEngine;

public class HeroStatsHandlerUI : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private HeroStatUI statPrefab;
    [SerializeField] private RectTransform statsContainer;
    [SerializeField] private RectTransform[] statsSlidersTransforms;
    [Header("Passive Skills")]
    [SerializeField] private HeroPassiveSkillUI passiveSkillPrefab;
    [SerializeField] private RectTransform passiveSklillsContainer;
    [SerializeField] private RectTransform[] passiveSkillsSlidersTransforms;
    [Header("Active Skill")]
    [SerializeField] private HeroActiveSkillUI activeSkillPrefab;
    [SerializeField] private RectTransform activeSklillTransform;  
    [Header("Attack And Defense Stats")]
    [SerializeField] private HeroAttackAndDefenseStatsUI attackAndDefenseStatsPrefab;
    [SerializeField] private RectTransform attackAndDefenseStatsContainer;
    [SerializeField] private RectTransform[] attackAndDefenseStatsTransforms;
    [Space(10)]
    [SerializeField] private HeroSkillsInformPopupUI informPopupUI;
    [SerializeField] private IconsSO iconsSO;




    private List<HeroStatUI> sliders = new();
    private List<HeroPassiveSkillUI> passiveSkillSliders = new();
    private List<HeroAttackAndDefenseStatsUI> attackAndDefenseStats = new();
    private HeroActiveSkillUI activeSkillUI;

    public void UpdateStats(HeroSO heroSO, HeroUpgradeLevelSO nextUpgradeHeroSO = null)
    {
        CleareSliders();

        CreateStatSliders(heroSO, nextUpgradeHeroSO);

        if (heroSO.PassiveSkillsAmount != 0)
        {
            CreatePassiveSkillSliders(heroSO, nextUpgradeHeroSO);
        }

        CreatActiveSkillUI(heroSO, nextUpgradeHeroSO);
        CreateAttackAndDefenseStats(heroSO, nextUpgradeHeroSO);
    }

    private void CreatePassiveSkillSliders(HeroSO heroSO, HeroUpgradeLevelSO nextUpgradeLevel = null)
    {
        for (int i = 0; i < heroSO.PassiveSkillsAmount; i++)
        {
            passiveSkillSliders.Add(Instantiate(passiveSkillPrefab, passiveSkillsSlidersTransforms[i].position, passiveSkillsSlidersTransforms[i].rotation, passiveSklillsContainer));
            passiveSkillSliders[i].SetupSlider(informPopupUI, heroSO, i, nextUpgradeLevel);
        }
    }

    private void CreateStatSliders(HeroSO heroSO, HeroUpgradeLevelSO nextUpgradeLevel = null)
    {
        for (int i = 0; i < 3; i++)
        {
            sliders.Add(Instantiate(statPrefab, statsSlidersTransforms[i].position, statsSlidersTransforms[i].rotation, statsContainer));
            sliders[i].SetupSlider(heroSO, iconsSO, sliderIndex: i, nextUpgradeLevel);
        }
    }

    private void CreatActiveSkillUI(HeroSO heroSO, HeroUpgradeLevelSO nextUpgradeLevel = null)
    {
        activeSkillUI = Instantiate(activeSkillPrefab, activeSklillTransform.position, activeSklillTransform.rotation, passiveSklillsContainer);
        activeSkillUI.SetupSlider(informPopupUI, heroSO, 2, nextUpgradeLevel);
    }

    private void CreateAttackAndDefenseStats(HeroSO heroSO, HeroUpgradeLevelSO nextUpgradeLevel = null)
    {
        for (int i = 0; i < 2; i++)
        {
            attackAndDefenseStats.Add(Instantiate(attackAndDefenseStatsPrefab, attackAndDefenseStatsTransforms[i].position, attackAndDefenseStatsTransforms[i].rotation, attackAndDefenseStatsContainer));
            attackAndDefenseStats[i].SetupSlider(informPopupUI, iconsSO, heroSO, i, nextUpgradeLevel);
        }
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

        if(activeSkillUI != null)
        {
            Destroy(activeSkillUI.gameObject);
        }

        foreach (var attackAndDefenseStat in attackAndDefenseStats)
        {
            Destroy(attackAndDefenseStat.gameObject);
        }

        activeSkillUI = null;
        sliders.Clear();
        passiveSkillSliders.Clear();
        attackAndDefenseStats.Clear();
    }
}
