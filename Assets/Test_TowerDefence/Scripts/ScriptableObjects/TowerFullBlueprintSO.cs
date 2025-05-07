using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerBlueprint", menuName = "SO/TowerSO/TowersBlueprint")]
public class TowerFullBlueprintSO : ScriptableObject
{
    public List<TowerSO> Towers;

    private int upgradedTowerSellCost;
    private int secondTimeUpgradedTowerSellCost;
    private int thirdTimeUpgradedTowerSellCost;


    public int GetTowerSellCost()
    {
        return Towers[0].Cost / 2;
    }
    public int GetUpgradedTowerSellCost()
    {
        upgradedTowerSellCost = (Towers[0].Cost / 2) + (Towers[1].Cost / 2);
        return upgradedTowerSellCost;
    }

    public int GetSecondTimeUpgradedTowerSellCost()
    {
        secondTimeUpgradedTowerSellCost = upgradedTowerSellCost + (Towers[2].Cost / 2);
        return secondTimeUpgradedTowerSellCost;
    }

    public int GetThirdTimeUpgradedTowerSellCost()
    {
        thirdTimeUpgradedTowerSellCost = secondTimeUpgradedTowerSellCost + (Towers[3].Cost / 2);
        return thirdTimeUpgradedTowerSellCost;
    }
}
