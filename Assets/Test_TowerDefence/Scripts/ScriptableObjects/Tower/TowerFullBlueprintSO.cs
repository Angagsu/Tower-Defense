using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerBlueprint", menuName = "SO/TowerSO/TowersBlueprint")]
public class TowerFullBlueprintSO : ScriptableObject
{
    public TowerType TowerType;
    [Space(10)]
    public List<TowerSO> TowerUpgrades;
    
    private int epicTowerSellCost;
    private int mythicTowerSellCost;
    private int rareTowerSellCost;


    public int GetCommonTowerSellCost()
    {
        return TowerUpgrades[0].Cost / 2;
    }
    public int GetEpicTowerSellCost()
    {
        epicTowerSellCost = (TowerUpgrades[0].Cost / 2) + (TowerUpgrades[1].Cost / 2);
        return epicTowerSellCost;
    }

    public int GetMythicTowerSellCost()
    {
        mythicTowerSellCost = epicTowerSellCost + (TowerUpgrades[2].Cost / 2);
        return mythicTowerSellCost;
    }

    public int GetRareTowerSellCost()
    {
        rareTowerSellCost = mythicTowerSellCost + (TowerUpgrades[3].Cost / 2);
        return rareTowerSellCost;
    }

    public int GetTowerSellCostByIndex(int index)
    {
        switch (index)
        {
            case 0:
                return GetCommonTowerSellCost();

            case 1:
                return GetEpicTowerSellCost();
  
            case 2:
                return GetMythicTowerSellCost();

            case 3:
                return GetRareTowerSellCost();

        }
        return default;
    }
}
