using UnityEngine;

[CreateAssetMenu(fileName = "GolemTowerUpgradeLevel", menuName = "SO/TowerSO/TowerUpgradeLevel/Golem")]
public class GolemTowerUpgradeLevelSO : TowerUpgradeLevelSO
{
    [field: Space(20)]
    [field: SerializeField] public float BlastRadius { get; private set; }
    [field: SerializeField] public float StunDuration { get; private set; }
}
