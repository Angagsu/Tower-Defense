using UnityEngine;


[CreateAssetMenu(fileName = "MageUpgradeLevel", menuName = "SO/HeroSO/HeroUpgradeLevel/Mage")]
public class MageHeroUpgradeLevelSO : HeroUpgradeLevelSO
{
    [field: Space(20)]
    [field: SerializeField] public float PeriodicDamage { get; private set; }
    [field: SerializeField] public float PeriodicDamageDuration { get; private set; }
}
