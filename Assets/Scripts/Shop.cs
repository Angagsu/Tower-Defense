
using UnityEngine;

public class Shop : MonoBehaviour
{
    private TowerBuildManager towerBuildManager;

    public TowerBlueprint standardTower;
    public TowerBlueprint missileLauncherTower;

    private void Start()
    {
        towerBuildManager = TowerBuildManager.Instance;
    }
    public void SelectStandardTower()
    {
        
        towerBuildManager.SelectTowerToBuild(standardTower);
        towerBuildManager.SelectStandardTower = true;
        towerBuildManager.SelectMissileLauncherTower = false;
    }

    public void SelectMissileLauncherTower()
    {
        
        towerBuildManager.SelectTowerToBuild(missileLauncherTower);
        towerBuildManager.SelectMissileLauncherTower = true;
        towerBuildManager.SelectStandardTower = false;
    }
}
