using UnityEngine;


public class BuildingArea : MonoBehaviour
{
    public DefenderMovement DefenderMovement { get; private set; }
    public TowerFullBlueprintSO TowerFullBlueprintSO { get; private set; }
    public GameObject Tower { get; set; }

    public bool IsUpgraded { get; set; } 
    public bool IsUpgradedSecondTime { get; set; } 
    public bool IsUpgradedThirdTime { get; set; } 

    [SerializeField] private TowersMap towersMap;
    [SerializeField] private BuildsController buildsController;
    [SerializeField] private Transform defendersStartPoint;

    private Vector3 positionOffset;



    public void DestroyTower()
    {
        Destroy(Tower);
        if (IsUpgradedSecondTime)
        {
            PlayerStats.Money += TowerFullBlueprintSO.GetSecondTimeUpgradedTowerSellCost();
            IsUpgradedSecondTime = false;
        }
        else if (IsUpgraded)
        {
            PlayerStats.Money += TowerFullBlueprintSO.GetUpgradedTowerSellCost();
            IsUpgraded = false;
        }
        else if (IsUpgradedThirdTime)
        {
            PlayerStats.Money += TowerFullBlueprintSO.GetThirdTimeUpgradedTowerSellCost();
            IsUpgradedThirdTime = false;
        }
        else
        {
            PlayerStats.Money += TowerFullBlueprintSO.GetTowerSellCost();
        }
    }

    public Vector3 GetBuildPosition()
    {
        if (TowerFullBlueprintSO == towersMap.ArrowTower)
        {
            return transform.position + (positionOffset = new Vector3(0, -0.7f, 0));
        }

        if (TowerFullBlueprintSO == towersMap.GolemTower || TowerFullBlueprintSO == towersMap.FireTower)
        {
            return transform.position + (positionOffset = new Vector3(0, 2f, 0));
        }

        if (TowerFullBlueprintSO == towersMap.LightningTower || TowerFullBlueprintSO == towersMap.IceTower)
        {
            return transform.position + (positionOffset = new Vector3(0, 0.5f, 0));
        }

        if (TowerFullBlueprintSO == towersMap.DefenderTower)
        {
            return transform.position + (positionOffset = new Vector3(0, 2, 0));
        }

        return transform.position + (positionOffset = new Vector3(0, 2, 0));
    }

    public void SetTower(TowerFullBlueprintSO towerFullBlueprintSO = null, GameObject tower = null)
    {
        this.Tower = tower;
        this.TowerFullBlueprintSO = towerFullBlueprintSO;
    }

    public void SetDefenders()
    {
        if (TowerFullBlueprintSO == towersMap.DefenderTower)
        {
            DefenderMovement = Tower.GetComponentInChildren<DefenderMovement>();
        }
    }

    public TowerFullBlueprintSO GetTowerBlueprint() => TowerFullBlueprintSO;

    public Transform GetDefendersStartPoint() => defendersStartPoint;

    public void SelectDefenderTowerOnUI()
    {
        DefenderMovement.SelectOrDeSelectDefenders(true);
        buildsController.DeselectGround();
    }
}
