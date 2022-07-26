
using UnityEngine;

public class Shop : MonoBehaviour
{
    private TowerBuildManager towerBuildManager;

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
    }

    public void SelectMissileLauncherTower()
    {
        
        towerBuildManager.SelectTowerToBuild(missileLauncherTower);
        towerBuildManager.SelectMissileLauncherTower = true;
        towerBuildManager.SelectStandardTower = false;
        towerBuildManager.SelectLaserTower = false;
    }

    public void SelectLaserTower()
    {
        towerBuildManager.SelectTowerToBuild(laserTower);
        towerBuildManager.SelectLaserTower = true;
        towerBuildManager.SelectMissileLauncherTower = false;
        towerBuildManager.SelectStandardTower = false;
    }
}
