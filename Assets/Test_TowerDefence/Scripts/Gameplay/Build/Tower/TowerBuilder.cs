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
        if (PlayerStats.Money < towerFullBlueprintSO.Towers[0].Cost)
        {
            Debug.Log("Not Enough Money to Build !!!");
            return;
        }

        PlayerStats.Money -= towerFullBlueprintSO.Towers[0].Cost;

        buildingArea.SetTower(towerFullBlueprintSO, CreatTower(0));
        buildingArea.SetDefenders();
    }

    public void UpgradeTower()
    {
        if (PlayerStats.Money < towerFullBlueprintSO.Towers[1].Cost)
        {
            Debug.Log("Not Enough Money to Upgrade !!!");
            return;
        }

        PlayerStats.Money -= towerFullBlueprintSO.Towers[1].Cost;


        buildingArea.DestroyTower();

        buildingArea.SetTower(towerFullBlueprintSO, CreatTower(1));
        buildingArea.SetDefenders();

        buildingArea.IsUpgraded = true;
        buildingArea.IsUpgradedSecondTime = false;
        buildingArea.IsUpgradedThirdTime = false;
    }

    public void SecondTimeUpgradeTower()
    {
        if (PlayerStats.Money < towerFullBlueprintSO.Towers[2].Cost)
        {
            Debug.Log("Not Enough Money to Upgrade !!!");
            return;
        }
        PlayerStats.Money -= towerFullBlueprintSO.Towers[2].Cost;

        buildingArea.DestroyTower();

        buildingArea.SetTower(towerFullBlueprintSO, CreatTower(2));
        buildingArea.SetDefenders();

        buildingArea.IsUpgradedSecondTime = true;
        buildingArea.IsUpgradedThirdTime = false;
        buildingArea.IsUpgraded = false;
    }

    public void ThirdTimeUpgradeTower()
    {
        if (PlayerStats.Money < towerFullBlueprintSO.Towers[3].Cost)
        {
            Debug.Log("Not Enough Money to Upgrade !!!");
            return;
        }
        PlayerStats.Money -= towerFullBlueprintSO.Towers[3].Cost;

        buildingArea.DestroyTower();
        buildingArea.SetTower(towerFullBlueprintSO, CreatTower(3));
        buildingArea.SetDefenders();

        buildingArea.IsUpgradedThirdTime = true;
        buildingArea.IsUpgradedSecondTime = false;
        buildingArea.IsUpgraded = false;
    }

    private GameObject CreatTower(int towerId)
    {
        GameObject tower = Instantiate(towerFullBlueprintSO.Towers[towerId].TowerPrefab, buildingArea.GetBuildPosition(), Quaternion.identity);
        GameObject effect = Instantiate(buildEffectPrefab, buildingArea.GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 2f);

        return tower;
    }
}
