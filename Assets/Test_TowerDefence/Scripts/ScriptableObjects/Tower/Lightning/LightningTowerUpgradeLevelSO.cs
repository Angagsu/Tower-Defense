using UnityEngine;


[CreateAssetMenu(fileName = "LightningTowerUpgradeLevel", menuName = "SO/TowerSO/TowerUpgradeLevel/Lightning")]
public class LightningTowerUpgradeLevelSO : TowerUpgradeLevelSO
{
    [field: Space(20)]
    [field: SerializeField] public float DecelerationMultiplier { get; private set; }
    [field: SerializeField] public float OverTimeDamageMultiplier { get; private set; }
}
