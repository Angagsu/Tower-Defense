using UnityEngine;

[CreateAssetMenu(fileName = "DefenderTowerUpgradeLevel", menuName = "SO/TowerSO/TowerUpgradeLevel/Defender")]
public class DefenderTowerUpgradeLevelSO : TowerUpgradeLevelSO
{
    [field: Space(20)]
    [field: SerializeField] public float TroopsHealth {  get; private set; }

    [field: Space(30)]
    [field: SerializeField] public float StunChance { get; private set; }
    [field: SerializeField] public float StunDuration { get; private set; }
}
