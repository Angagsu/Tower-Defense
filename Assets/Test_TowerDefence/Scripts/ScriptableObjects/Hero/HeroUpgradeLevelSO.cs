using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HeroUpgradeLevel", menuName = "SO/HeroSO/HeroUpgradeLevel")]
public class HeroUpgradeLevelSO : ScriptableObject
{  
    [field: SerializeField] public int UpgradeCostByGold { get; private set; }
    [field: SerializeField] public int UpgradeCostByCard { get; private set; }

    [field: Space(20)]
    [field: SerializeField] public AttackStats AttackStats { get; private set; }

    [field: Space(20)]
    [field: SerializeField] public float MoveSpeed { get; private set; }

    [field: Space(20)]
    [field: SerializeField] public float ActiveSkillValue { get; private set; }
    [field: SerializeField] public bool ActiveSkillAccessibility { get; private set; }

    [field: Space(10)]
    [field: SerializeField] public List<float> PassiveSkillsValues { get; private set; }
    [field: SerializeField] public List<bool> PassiveSkillsAccessibility { get; private set; }

    [field: Space(10)]
    [field: SerializeField] public List<float> StatsValues { get; private set; }
}
