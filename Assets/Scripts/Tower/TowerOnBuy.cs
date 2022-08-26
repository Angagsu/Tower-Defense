
using UnityEngine;

public class TowerOnBuy : MonoBehaviour
{
    private TowerBuildManager towerBuildManager;
    private GroundBehavior groundBehavior;
    public TowerBlueprint standardTower;
    public TowerBlueprint missileLauncherTower;
    public TowerBlueprint laserTower;
    private void Start()
    {
        towerBuildManager = TowerBuildManager.Instance;
    }
    public void SelectStandardTower()
    {
        
        towerBuildManager.SelectTowerToBuild(standardTower);
        towerBuildManager.SelectStandardTower = true;
        towerBuildManager.SelectMissileLauncherTower = false;
        towerBuildManager.SelectLaserTower = false;
        groundBehavior.BuildTower(towerBuildManager.GetTowerToBuild());
    }

    public void SelectMissileLauncherTower()
    {
        
        towerBuildManager.SelectTowerToBuild(missileLauncherTower);
        towerBuildManager.SelectMissileLauncherTower = true;
        towerBuildManager.SelectStandardTower = false;
        towerBuildManager.SelectLaserTower = false;
        groundBehavior.BuildTower(towerBuildManager.GetTowerToBuild());
    }

    public void SelectLaserTower()
    {
        towerBuildManager.SelectTowerToBuild(laserTower);
        towerBuildManager.SelectLaserTower = true;
        towerBuildManager.SelectMissileLauncherTower = false;
        towerBuildManager.SelectStandardTower = false;
        groundBehavior.BuildTower(towerBuildManager.GetTowerToBuild());
    }

    public void SetGroundForBuilding(GroundBehavior groundBehavior)
    {
        this.groundBehavior = groundBehavior;
    }
}
