using UnityEngine;



[CreateAssetMenu(fileName = "KnightUpgradeLevel", menuName = "SO/HeroSO/HeroUpgradeLevel/Knight")]
public class KnightHeroUpgradeLevelSO : HeroUpgradeLevelSO
{
    [field: Space(20)]
    [field: SerializeField] public float BlastAttackChance { get; private set; }
    [field: SerializeField] public float StunDuration { get; private set; }
}
