using UnityEngine;

[CreateAssetMenu(fileName = "FireTowerUpgradeLevel", menuName = "SO/TowerSO/TowerUpgradeLevel/Fire")]
public class FireTowerUpgradeLevelSO : TowerUpgradeLevelSO
{
    [field: Space(20)]
    [field: SerializeField] public float PeriodicDamage { get; private set; }
    [field: SerializeField] public float PeriodicDamageDuration { get; private set; }
}
