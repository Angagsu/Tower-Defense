using UnityEngine;

public class TowersMap : MonoBehaviour
{
    [field: SerializeField] public TowerFullBlueprintSO[] Towers { get; private set; } 
    [field: Space(10)] [field: SerializeField] public TowerFullBlueprintSO ArrowTower { get; private set; }
    [field: Space(10)] [field: SerializeField] public TowerFullBlueprintSO DefenderTower { get; private set; }
    [field: Space(10)] [field: SerializeField] public TowerFullBlueprintSO FireTower { get; private set; }
    [field: Space(10)] [field: SerializeField] public TowerFullBlueprintSO LightningTower { get; private set; }
    [field: Space(10)] [field: SerializeField] public TowerFullBlueprintSO IceTower { get; private set; }
    [field: Space(10)] [field: SerializeField] public TowerFullBlueprintSO GolemTower { get; private set; } 
}
