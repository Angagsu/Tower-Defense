using UnityEngine;

[CreateAssetMenu(fileName = "IceTowerUpgradeLevel", menuName = "SO/TowerSO/TowerUpgradeLevel/Ice")]
public class IceTowerUpgradeLevelSO : TowerUpgradeLevelSO
{
    [field: Space(20)]
    [field: SerializeField] public float DecelerationMultiplier { get; private set; }
    [field: SerializeField] public float OverTimeDamageMultiplier { get; private set; }
}
