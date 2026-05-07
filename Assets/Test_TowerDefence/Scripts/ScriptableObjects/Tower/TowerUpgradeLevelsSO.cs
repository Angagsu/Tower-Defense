using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "TowerUpgradeLevels", menuName = "SO/TowerSO/TowerUpgradeLevels")]
public class TowerUpgradeLevelsSO : ScriptableObject
{
    public List<TowerUpgradeLevelSO> TowerUpgradeLevelSOs;
}
