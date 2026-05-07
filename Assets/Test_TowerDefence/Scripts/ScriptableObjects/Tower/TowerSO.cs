using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tower", menuName = "SO/TowerSO/Tower")]
public class TowerSO : ScriptableObject
{
    [field: Header("Tower Description")]
    [field: Space(10)]
    [field: SerializeField] public string TowerName { get; private set; }
    [field: SerializeField] public string TowerDescription { get; private set; }
    [field: SerializeField] public int Cost { get; private set; }
    [field: SerializeField] public GameObject TowerPrefab { get; private set; }
    [field: SerializeField] public Sprite TowerSprite { get; private set; }

    [field: Header("Tower Upgrades")]
    [field: Space(10)]
    [field: SerializeField] public TowerUpgradeLevelSO CurrentUpgradeLevel { get; set; }
    [field: SerializeField] public TowerUpgradeLevelsSO UpgradeLevelsSO { get; private set; }

    [field: Header("Passive Skills")]
    [field: Space(10)]
    [field: SerializeField] public List<Sprite> PassivSkilleSprites { get; private set; }

    [field: Space(10)]
    [field: SerializeField, Range(0, 2)] public int PassiveSkillsAmount { get; private set; }

    [field: Space(15)]
    [field: SerializeField, Range(0, 10)] public List<int> PassiveSkillsUnlockLevels { get; private set; }
    [field: SerializeField] public List<float> StatsMaxValues { get; private set; }
    [field: SerializeField] public List<string> PassiveSkillsNames { get; private set; }
    [field: SerializeField] public List<string> PassiveSkillsDescripsions { get; private set; }
    

    [field: Header("Tower Storable Data")]
    [field: Space(10)]
    [field: SerializeField] public TowerData TowerData { get;  set; }
}
