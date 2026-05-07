using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "All Towers", menuName = "SO/TowerSO/AllTowersBlueprints")]
public class AllTowersBlueprintSO : ScriptableObject
{
    [field: SerializeField] public List<TowerFullBlueprintSO> AllTowers { get; private set; }
    [field: SerializeField] public List<TowerType> TowerTypes { get; private set; }
    [field: SerializeField] public List<TowerFullBlueprintSO> AcquiredTowers { get; set; }
}
