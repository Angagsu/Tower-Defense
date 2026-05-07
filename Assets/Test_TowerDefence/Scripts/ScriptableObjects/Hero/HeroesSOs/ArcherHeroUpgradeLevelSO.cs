using UnityEngine;


[CreateAssetMenu(fileName = "ArcherUpgradeLevel", menuName = "SO/HeroSO/HeroUpgradeLevel/Archer")]
public class ArcherHeroUpgradeLevelSO : HeroUpgradeLevelSO
{
    [field: Space(20)]
    [field: SerializeField] public float CriticalDamage { get; private set; }
    [field: SerializeField] public float CriticalDamageChance { get; private set; }
}
