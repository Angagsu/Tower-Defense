using UnityEngine;

[CreateAssetMenu(fileName = "ArrowTowerUpgradeLevel", menuName = "SO/TowerSO/TowerUpgradeLevel/Arrow")]
public class ArrowTowerUpgradLevelSO : TowerUpgradeLevelSO
{
    [field: Space(20)]
    [field: SerializeField] public float CriticalDamage {  get; private set; }
    [field: SerializeField] public float CriticalDamageChance { get; private set; }
}
