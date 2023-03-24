
using UnityEngine;

public class TowerOnBuy : MonoBehaviour
{
    private TowerBuildManager towerBuildManager;
    public GroundBehavior GroundBehavior;
    [Header("Type Of Towers")]
    public TowerBlueprint ArrowTower;
    public TowerBlueprint GolemTower;
    public TowerBlueprint ThunderTower;
    public TowerBlueprint FireTower;
    public TowerBlueprint IceTower;
    public TowerBlueprint DefenderTower;
    
    private void Start()
    {
        towerBuildManager = TowerBuildManager.Instance;
    }
    public void SelectArrowTower()
    {
        
        towerBuildManager.SelectTowerToBuild(ArrowTower);
        towerBuildManager.SelectStandardTower = true;
        towerBuildManager.SelectMissileLauncherTower = false;
        towerBuildManager.SelectLaserTower = false;
        GroundBehavior.BuildTower(towerBuildManager.GetTowerToBuild());
    }

    public void SelectGolemTower()
    {
        
        towerBuildManager.SelectTowerToBuild(GolemTower);
        towerBuildManager.SelectMissileLauncherTower = true;
        towerBuildManager.SelectStandardTower = false;
        towerBuildManager.SelectLaserTower = false;
        GroundBehavior.BuildTower(towerBuildManager.GetTowerToBuild());
    }
    public void SelectFireTower()
    {

        towerBuildManager.SelectTowerToBuild(FireTower);
        towerBuildManager.SelectStandardTower = true;
        towerBuildManager.SelectMissileLauncherTower = false;
        towerBuildManager.SelectLaserTower = false;
        GroundBehavior.BuildTower(towerBuildManager.GetTowerToBuild());
    }

    public void SelectThunderTower()
    {
        towerBuildManager.SelectTowerToBuild(ThunderTower);
        towerBuildManager.SelectLaserTower = true;
        towerBuildManager.SelectMissileLauncherTower = false;
        towerBuildManager.SelectStandardTower = false;
        GroundBehavior.BuildTower(towerBuildManager.GetTowerToBuild());
    }
    public void SelectIceTower()
    {
        towerBuildManager.SelectTowerToBuild(IceTower);
        towerBuildManager.SelectLaserTower = true;
        towerBuildManager.SelectMissileLauncherTower = false;
        towerBuildManager.SelectStandardTower = false;
        GroundBehavior.BuildTower(towerBuildManager.GetTowerToBuild());
    }

    public void SelectDefenderTower()
    {
        towerBuildManager.SelectTowerToBuild(DefenderTower);
        towerBuildManager.SelectLaserTower = false;
        towerBuildManager.SelectMissileLauncherTower = false;
        towerBuildManager.SelectStandardTower = false;
        GroundBehavior.BuildTower(towerBuildManager.GetTowerToBuild());
    }

    public void SetGroundForBuilding(GroundBehavior groundBehavior)
    {
        this.GroundBehavior = groundBehavior;
        
    }
}
