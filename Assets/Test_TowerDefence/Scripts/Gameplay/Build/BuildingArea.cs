using Assets.Scripts.Tower;
using UnityEngine;


public class BuildingArea : MonoBehaviour
{
    public DefendersController DefendersController { get; private set; }
    public TowerFullBlueprintSO TowerFullBlueprintSO { get; private set; }

    public TowerSO CurrentTowerSO { get; private set; }
    public BaseTower CurrentTower { get; private set; }

    public bool IsUpgraded { get; set; } 
    public bool IsUpgradedSecondTime { get; set; } 
    public bool IsUpgradedThirdTime { get; set; } 

    [SerializeField] private TowersMap towersMap;
    [SerializeField] private BuildsController buildsController;
    [SerializeField] private Transform defendersStartPoint;

    private Vector3 positionOffset;


    public void DestroyTower()
    {   
        if(CurrentTower.IsCrashed)
        {
            Destroy(CurrentTower.gameObject);
            return;
        }

        if (IsUpgradedSecondTime)
        {
            buildsController.PlayerDataHandler.IncreaseMoney(TowerFullBlueprintSO.GetMythicTowerSellCost());
            IsUpgradedSecondTime = false;
        }
        else if (IsUpgraded)
        {
            buildsController.PlayerDataHandler.IncreaseMoney(TowerFullBlueprintSO.GetEpicTowerSellCost());
            IsUpgraded = false;
        }
        else if (IsUpgradedThirdTime)
        {
            buildsController.PlayerDataHandler.IncreaseMoney(TowerFullBlueprintSO.GetRareTowerSellCost());
            IsUpgradedThirdTime = false;
        }
        else
        {
            buildsController.PlayerDataHandler.IncreaseMoney(TowerFullBlueprintSO.GetCommonTowerSellCost());
        }

        Destroy(CurrentTower.gameObject);
    }

    public void DestroyTowerForUpgrade()
    {
        Destroy(CurrentTower.gameObject);
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
           return transform.position + (positionOffset = new Vector3(0, 2, 0.7f));
       }

        return transform.position + positionOffset;
    }

    public void SetTower(TowerSO currentTower, TowerFullBlueprintSO towerFullBlueprintSO = null, BaseTower tower = null)
    {
        CurrentTower = tower;
        TowerFullBlueprintSO = towerFullBlueprintSO;
        CurrentTowerSO = currentTower;
    }

    public void SetDefenders()
    {
        if (TowerFullBlueprintSO == towersMap.DefenderTower)
        {
            DefendersController = CurrentTower.gameObject.GetComponentInChildren<DefendersController>();
        }
    }

    public TowerFullBlueprintSO GetTowerBlueprint() => TowerFullBlueprintSO;

    public Transform GetDefendersStartPoint() => defendersStartPoint;

    public void SelectDefenderTowerOnUI()
    {
        DefendersController.SelectOrDeSelectDefenders(true);
        buildsController.DeselectGround();
    }
}
