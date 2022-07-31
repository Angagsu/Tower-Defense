
using UnityEngine;

public class TowerBuildManager : MonoBehaviour
{
    public static TowerBuildManager Instance;

    private TowerUI towerUI;
    private TowerBlueprint towerToBuild;
    private GroundBehavior selectedGround;

    [SerializeField] private GameObject buildEffectPrefab;

    public bool SelectMissileLauncherTower = false;
    public bool SelectStandardTower = false;
    public bool SelectLaserTower = false;

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

    public void BuildTowerOn(GroundBehavior groundBehavior)
    {
        if (PlayerStats.Money < towerToBuild.Cost)
        {
            Debug.Log("Not Enough Money to Build !!!");
            SelectStandardTower = false;
            SelectMissileLauncherTower = false;
            SelectLaserTower = false;
            return;
        }

        PlayerStats.Money -= towerToBuild.Cost;

        GameObject tower = Instantiate(towerToBuild.TowerPrefab, groundBehavior.GetBuildPosition(), Quaternion.identity);
        GameObject effect = Instantiate(buildEffectPrefab, groundBehavior.GetBuildPosition(), Quaternion.identity);
        Destroy(effect.gameObject, 4f);
        groundBehavior.tower = tower;
        SelectStandardTower = false;
        SelectMissileLauncherTower = false;
        SelectLaserTower = false;

        Debug.Log("Tower build! Money left: " + PlayerStats.Money);
    }
}
