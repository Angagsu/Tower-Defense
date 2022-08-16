using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TowerBlueprint 
{
    public GameObject TowerPrefab;
    public int Cost;

    public GameObject UpgradedTower;
    public int UpgradeCost;

    public GameObject SecondTimeUpgradedTower;
    public int SecondTimeUpgradeCost;

    public GameObject ThirdTimeUpgradedTower;
    public int ThirdTimeUpgradeCost;

    private int upgradedTowerSellCost;
    private int secondTimeUpgradedTowerSellCost;
    private int thirdTimeUpgradedTowerSellCost;
    public int GetTowerSellCost()
    {
        return Cost / 2;
    }
    public int GetUpgradedTowerSellCost()
    {
        upgradedTowerSellCost = (Cost / 2) + (UpgradeCost / 2);
        return upgradedTowerSellCost;
    }

    public int GetSecondTimeUpgradedTowerSellCost()
    { 
        secondTimeUpgradedTowerSellCost = upgradedTowerSellCost + (SecondTimeUpgradeCost / 2);
        return secondTimeUpgradedTowerSellCost;
    }

    public int GetThirdTimeUpgradedTowerSellCost()
    {
        thirdTimeUpgradedTowerSellCost = secondTimeUpgradedTowerSellCost + (ThirdTimeUpgradeCost / 2);
        return thirdTimeUpgradedTowerSellCost;
    }
}
