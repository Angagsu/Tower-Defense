using Assets.Scripts.Tower;
using System.Collections.Generic;
using System;
using UnityEngine;

public class TowerBuilder : MonoBehaviour
{
    public List<BaseTower> Towers { get; private set; } = new List<BaseTower>();

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
        if (playerDataHandler.Money < towerFullBlueprintSO.TowerUpgrades[0].Cost)
        {
            Debug.Log("Not Enough Money to Build !!!");
            return;
        }

        playerDataHandler.ReduceMoney(towerFullBlueprintSO.TowerUpgrades[0].Cost);

        buildingArea.SetTower(towerFullBlueprintSO.TowerUpgrades[0], towerFullBlueprintSO, CreatTower(0));
        buildingArea.SetDefenders();
    }

    public void UpgradeTower()
    {
        if (playerDataHandler.Money < towerFullBlueprintSO.TowerUpgrades[1].Cost)
        {
            Debug.Log("Not Enough Money to Build !!!");
            return;
        }

        playerDataHandler.ReduceMoney(towerFullBlueprintSO.TowerUpgrades[1].Cost);

        Towers.Remove(buildingArea.CurrentTower);

        buildingArea.DestroyTowerForUpgrade();
        buildingArea.SetTower(towerFullBlueprintSO.TowerUpgrades[1], towerFullBlueprintSO, CreatTower(1));
        buildingArea.SetDefenders();

        buildingArea.IsUpgraded = true;
        buildingArea.IsUpgradedSecondTime = false;
        buildingArea.IsUpgradedThirdTime = false;
    }

    public void SecondTimeUpgradeTower()
    {
        if (playerDataHandler.Money < towerFullBlueprintSO.TowerUpgrades[2].Cost)
        {
            Debug.Log("Not Enough Money to Build !!!");
            return;
        }

        playerDataHandler.ReduceMoney(towerFullBlueprintSO.TowerUpgrades[2].Cost);

        Towers.Remove(buildingArea.CurrentTower);

        buildingArea.DestroyTowerForUpgrade();
        buildingArea.SetTower(towerFullBlueprintSO.TowerUpgrades[2], towerFullBlueprintSO, CreatTower(2));
        buildingArea.SetDefenders();

        buildingArea.IsUpgradedSecondTime = true;
        buildingArea.IsUpgradedThirdTime = false;
        buildingArea.IsUpgraded = false;
    }

    public void ThirdTimeUpgradeTower()
    {
        if (playerDataHandler.Money < towerFullBlueprintSO.TowerUpgrades[3].Cost)
        {
            Debug.Log("Not Enough Money to Build !!!");
            return;
        }

        playerDataHandler.ReduceMoney(towerFullBlueprintSO.TowerUpgrades[3].Cost);

        Towers.Remove(buildingArea.CurrentTower);

        buildingArea.DestroyTowerForUpgrade();
        buildingArea.SetTower(towerFullBlueprintSO.TowerUpgrades[3], towerFullBlueprintSO, CreatTower(3));
        buildingArea.SetDefenders();

        buildingArea.IsUpgradedThirdTime = true;
        buildingArea.IsUpgradedSecondTime = false;
        buildingArea.IsUpgraded = false;
    }

    public void SellTower()
    {
        Towers.Remove(buildingArea.CurrentTower);

        buildingArea.DestroyTower();
    }

    private BaseTower CreatTower(int towerId)
    {
        GameObject towerObj = Instantiate(towerFullBlueprintSO.TowerUpgrades[towerId].TowerPrefab, buildingArea.GetBuildPosition(), Quaternion.identity);

        if (towerObj.TryGetComponent(out BaseTower tower))
        {
            if (tower is DefenderTower)
            {
                var defenderTower = tower as DefenderTower;
                towerDependenciesInjecter.SetDefenderTowerDependencies(defenderTower, towerFullBlueprintSO.TowerUpgrades[towerId]);
            }
            else
            {
                towerDependenciesInjecter.SetTowerDependencies(tower, towerFullBlueprintSO.TowerUpgrades[towerId]);
            }
        }

        Towers.Add(tower);

        GameObject effect = Instantiate(buildEffectPrefab, buildingArea.GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 2f);

        return tower;
    }
}
