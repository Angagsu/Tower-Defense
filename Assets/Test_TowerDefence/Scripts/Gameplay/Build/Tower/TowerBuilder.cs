using Assets.Scripts.Tower;
using UnityEngine;

public class TowerBuilder : MonoBehaviour
{
    [SerializeField] private GameObject buildEffectPrefab;
    [SerializeField] private GameplayPlayerDataHandler playerDataHandler;
    [SerializeField] private TowerDependenciesInjecter towerDependenciesInjecter;

    private TowerFullBlueprintSO towerFullBlueprintSO;
    private BuildingArea buildingArea;


    public void SetBuildingAreaAndTowerBlueprint(TowerFullBlueprintSO towerBlueprint, BuildingArea buildingArea)
    {
        this.towerFullBlueprintSO = towerBlueprint;
        this.buildingArea = buildingArea;
    }

    public void Build()
    {
        if (playerDataHandler.Money < towerFullBlueprintSO.Towers[0].Cost)
        {
            Debug.Log("Not Enough Money to Build !!!");
            return;
        }

        playerDataHandler.ReduceMoney(towerFullBlueprintSO.Towers[0].Cost);

        buildingArea.SetTower(towerFullBlueprintSO, CreatTower(0));
        buildingArea.SetDefenders();
    }

    public void UpgradeTower()
    {
        if (playerDataHandler.Money < towerFullBlueprintSO.Towers[1].Cost)
        {
            Debug.Log("Not Enough Money to Build !!!");
            return;
        }

        playerDataHandler.ReduceMoney(towerFullBlueprintSO.Towers[1].Cost);

        buildingArea.DestroyTowerForUpgrade();
        buildingArea.SetTower(towerFullBlueprintSO, CreatTower(1));
        buildingArea.SetDefenders();

        buildingArea.IsUpgraded = true;
        buildingArea.IsUpgradedSecondTime = false;
        buildingArea.IsUpgradedThirdTime = false;
    }

    public void SecondTimeUpgradeTower()
    {
        if (playerDataHandler.Money < towerFullBlueprintSO.Towers[2].Cost)
        {
            Debug.Log("Not Enough Money to Build !!!");
            return;
        }

        playerDataHandler.ReduceMoney(towerFullBlueprintSO.Towers[2].Cost);

        buildingArea.DestroyTowerForUpgrade();
        buildingArea.SetTower(towerFullBlueprintSO, CreatTower(2));
        buildingArea.SetDefenders();

        buildingArea.IsUpgradedSecondTime = true;
        buildingArea.IsUpgradedThirdTime = false;
        buildingArea.IsUpgraded = false;
    }

    public void ThirdTimeUpgradeTower()
    {
        if (playerDataHandler.Money < towerFullBlueprintSO.Towers[3].Cost)
        {
            Debug.Log("Not Enough Money to Build !!!");
            return;
        }

        playerDataHandler.ReduceMoney(towerFullBlueprintSO.Towers[3].Cost);

        buildingArea.DestroyTowerForUpgrade();
        buildingArea.SetTower(towerFullBlueprintSO, CreatTower(3));
        buildingArea.SetDefenders();

        buildingArea.IsUpgradedThirdTime = true;
        buildingArea.IsUpgradedSecondTime = false;
        buildingArea.IsUpgraded = false;
    }

    private GameObject CreatTower(int towerId)
    {
        GameObject towerObj = Instantiate(towerFullBlueprintSO.Towers[towerId].TowerPrefab, buildingArea.GetBuildPosition(), Quaternion.identity);

        if (towerObj.TryGetComponent(out BaseTower tower))
        {
            towerDependenciesInjecter.SetTowerDependencies(tower);
        }
        else if(towerObj.TryGetComponent(out DefenderTower defenderTower))
        {
            towerDependenciesInjecter.SetDefenderTowerDependencies(defenderTower);
        }
        
        GameObject effect = Instantiate(buildEffectPrefab, buildingArea.GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 2f);

        return towerObj;
    }
}
