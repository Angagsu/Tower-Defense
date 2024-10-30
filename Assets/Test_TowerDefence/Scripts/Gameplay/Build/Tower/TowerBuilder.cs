using UnityEngine;

public class TowerBuilder : MonoBehaviour
{
    [SerializeField] private GameObject buildEffectPrefab;

    private TowerFullBlueprintSO towerFullBlueprintSO;
    private BuildingArea buildingArea;
    private DefenderMovement defenderMovement;

    public void SetBuildingAreaAndTowerBlueprint(TowerFullBlueprintSO towerBlueprint, BuildingArea buildingArea)
    {
        this.towerFullBlueprintSO = towerBlueprint;
        this.buildingArea = buildingArea;
    }

    public void Build()
    {
        if (PlayerStats.Money < towerFullBlueprintSO.Tower.Cost)
        {
            Debug.Log("Not Enough Money to Build !!!");
            return;
        }

        PlayerStats.Money -= towerFullBlueprintSO.Tower.Cost;

        buildingArea.SetTower(towerFullBlueprintSO, CreatTower());
        buildingArea.SetDefenders();
    }

    public void UpgradeTower()
    {
        if (PlayerStats.Money < towerFullBlueprintSO.UpgradedTower.Cost)
        {
            Debug.Log("Not Enough Money to Upgrade !!!");
            return;
        }

        PlayerStats.Money -= towerFullBlueprintSO.UpgradedTower.Cost;


        buildingArea.DestroyTower();

        buildingArea.SetTower(towerFullBlueprintSO, CreatTower());
        buildingArea.SetDefenders();

        buildingArea.IsUpgraded = true;
        buildingArea.IsUpgradedSecondTime = false;
        buildingArea.IsUpgradedThirdTime = false;
    }

    public void SecondTimeUpgradeTower()
    {
        if (PlayerStats.Money < towerFullBlueprintSO.SecondTimeUpgradedTower.Cost)
        {
            Debug.Log("Not Enough Money to Upgrade !!!");
            return;
        }
        PlayerStats.Money -= towerFullBlueprintSO.SecondTimeUpgradedTower.Cost;

        buildingArea.DestroyTower();

        buildingArea.SetTower(towerFullBlueprintSO, CreatTower());
        buildingArea.SetDefenders();

        buildingArea.IsUpgradedSecondTime = true;
        buildingArea.IsUpgradedThirdTime = false;
        buildingArea.IsUpgraded = false;
    }

    public void ThirdTimeUpgradeTower()
    {
        if (PlayerStats.Money < towerFullBlueprintSO.ThirdTimeUpgradedTower.Cost)
        {
            Debug.Log("Not Enough Money to Upgrade !!!");
            return;
        }
        PlayerStats.Money -= towerFullBlueprintSO.ThirdTimeUpgradedTower.Cost;

        buildingArea.DestroyTower();
        buildingArea.SetTower(towerFullBlueprintSO, CreatTower());
        buildingArea.SetDefenders();

        buildingArea.IsUpgradedThirdTime = true;
        buildingArea.IsUpgradedSecondTime = false;
        buildingArea.IsUpgraded = false;
    }

    private GameObject CreatTower()
    {
        GameObject tower = Instantiate(towerFullBlueprintSO.Tower.TowerPrefab, buildingArea.GetBuildPosition(), Quaternion.identity);
        GameObject effect = Instantiate(buildEffectPrefab, buildingArea.GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 2f);

        return tower;
    }
}
