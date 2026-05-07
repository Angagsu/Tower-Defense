using System.Collections.Generic;
using UnityEngine;


public abstract class TowerUpgradeLevelSO : ScriptableObject
{
    //[field: Space(10)][field: SerializeField] public TowerUpgradeLevel TowerUpgradeLevel {  get; private set; }   
    [field: SerializeField] public AttackStats AttackStats { get; private set; }
    [field: Space(10)]
    [field: SerializeField] public float AttackRange { get; private set; }
    [field: SerializeField] public float AttackRate { get; private set; }
    [field: SerializeField] public float BuyCost { get; private set; }


    [field: Space(10)]
    [field: SerializeField] public int UpgradeCostByGold { get; private set; }
    [field: SerializeField] public int UpgradeCostByCard { get; private set; }

    [field: Space(10)]
    [field: SerializeField] public List<float> StatsValues { get; private set; }
    [field: SerializeField] public List<float> PassiveSkillsValues { get; private set; }
    [field: SerializeField] public List<bool> PassiveSkillsAccessibility { get; private set; }
}
