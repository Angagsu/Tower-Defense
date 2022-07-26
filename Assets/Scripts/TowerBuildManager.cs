
using UnityEngine;

public class TowerBuildManager : MonoBehaviour
{
    public static TowerBuildManager Instance;

    private TowerBlueprint towerToBuild;


    public GameObject standartTowerPrefab;
    public GameObject missileLauncherTowerPrefab;
    public GameObject buildEffectPrefab;

    public bool SelectMissileLauncherTower = false;
    public bool SelectStandardTower = false;

    public bool CanBuild { get { return towerToBuild != null; } }
    public bool HasManey { get { return PlayerStats.Money >= towerToBuild.cost; } }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one TowerBuilManager in scene!");
            return;
        }

        Instance = this;

    }

    public void SelectTowerToBuild(TowerBlueprint tower)
    {
        towerToBuild = tower;
    }

    public void BuildTowerOn(GroundBehavior groundBehavior)
    {
        if (PlayerStats.Money < towerToBuild.cost)
        {
            Debug.Log("Not Enough Money to Build !!!");
            SelectStandardTower = false;
            SelectMissileLauncherTower = false;
            return;
        }

        PlayerStats.Money -= towerToBuild.cost;

        GameObject tower = Instantiate(towerToBuild.prefab, groundBehavior.GetBuildPosition(), Quaternion.identity);
        GameObject effect = Instantiate(buildEffectPrefab, groundBehavior.GetBuildPosition(), Quaternion.identity);
        Destroy(effect.gameObject, 4f);
        groundBehavior.tower = tower;
        SelectStandardTower = false;
        SelectMissileLauncherTower = false;

        Debug.Log("Tower build! Money left: " + PlayerStats.Money);
    }
}
