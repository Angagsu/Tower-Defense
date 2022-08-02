using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TowerBlueprint 
{
    public GameObject TowerPrefab;
    public int Cost;

    public GameObject UpgrateTower;
    public int UpgradeCost;

    public int GetTowerSellCost()
    {
        return Cost / 2;
    }
    public int GetUpgradedTowerSellCost()
    {
        return (Cost / 2) + (UpgradeCost / 2);
    }
}
