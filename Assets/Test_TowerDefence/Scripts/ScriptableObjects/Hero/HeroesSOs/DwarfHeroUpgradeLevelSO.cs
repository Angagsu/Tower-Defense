using UnityEngine;


[CreateAssetMenu(fileName = "DwarfUpgradeLevel", menuName = "SO/HeroSO/HeroUpgradeLevel/Dwarf")]
public class DwarfHeroUpgradeLevelSO : HeroUpgradeLevelSO
{
    [field: Space(20)]
    [field: SerializeField] public float PhysicalPenetrationValue { get; private set; }
    [field: SerializeField] public float PhysicalPenetrationRadius { get; private set; }
}
