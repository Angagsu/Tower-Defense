
using UnityEngine;

public class TowerBuildManager : MonoBehaviour
{
    public static TowerBuildManager Instance;
    [SerializeField] private TowersBuildUI towersBuildUI;
    [SerializeField] private TowerUpgradeUI towerUpgradeUI;
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
    }
    
    public void SelectedGround(GroundBehavior groundBehavior)
    {
        if (selectedGround == groundBehavior)
        {
            DeselectGround();
            return;
        }

        towersBuildUI.towerBuildUI.SetActive(!towersBuildUI.towerBuildUI.activeSelf);

        if (towersBuildUI.towerBuildUI.activeSelf)
        {
            towersBuildUI.towerBuildUI.SetActive(false);
        }

        selectedGround = groundBehavior;
        towerToBuild = null;
        towerUpgradeUI.SetTargetGround(groundBehavior);
    }

    public void SelectedGroundForBuildTowerUI(GroundBehavior groundBehavior)
    {
        if (selectedGround == groundBehavior)
        {
            DeselectGround();
            return;
        }
        
        towerUpgradeUI.towerUpgradeUI.SetActive(!towerUpgradeUI.towerUpgradeUI.activeSelf);

        if (towerUpgradeUI.towerUpgradeUI.activeSelf)
        {
            towerUpgradeUI.towerUpgradeUI.SetActive(false);
        }
        selectedGround = groundBehavior;
        towerToBuild = null;
        towersBuildUI.SetTargetGroundForBuilding(groundBehavior);
    }

    public void DeselectGround()
    {
        selectedGround = null;
        towerUpgradeUI.HideCanvas();
        towersBuildUI.HideCanvas();
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
