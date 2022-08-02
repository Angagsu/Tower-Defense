
using UnityEngine;

public class TowerBuildManager : MonoBehaviour
{
    public static TowerBuildManager Instance;

    private TowerUI towerUI;
    private GroundBehavior selectedGround;

    [HideInInspector]
    public TowerBlueprint towerToBuild;

    [HideInInspector] public bool SelectMissileLauncherTower = false;
    [HideInInspector] public bool SelectStandardTower = false;
    [HideInInspector] public bool SelectLaserTower = false;
    

    public bool CanBuild { get { return towerToBuild != null; } }
    public bool HasManey { get { return PlayerStats.Money >= towerToBuild.Cost; } }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one TowerBuilManager in scene!");
            return;
        }

        Instance = this;

        towerUI = GameObject.Find("TowerUI").GetComponent<TowerUI>();
    }

    public void SelectedGround(GroundBehavior groundBehavior)
    {
        if (selectedGround == groundBehavior)
        {
            DeselectGround();
            return;
        }

        selectedGround = groundBehavior;
        towerToBuild = null;
        towerUI.SetTargetGround(groundBehavior);
    }

    public void DeselectGround()
    {
        selectedGround = null;
        towerUI.HideCanvas();
    }
    public void SelectTowerToBuild(TowerBlueprint tower)
    {
        towerToBuild = tower;
        DeselectGround();
    }

    public TowerBlueprint GetTowerToBuild()
    {
        return towerToBuild;
    }
}
