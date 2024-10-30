using UnityEngine;

[CreateAssetMenu(fileName = "TowerBlueprint", menuName = "SO/TowerSO/TowersBlueprint")]
public class TowerFullBlueprintSO : ScriptableObject
{
    [Space(10)] public TowerSO Tower;

    [Space(10)] public TowerSO UpgradedTower;

    [Space(10)] public TowerSO SecondTimeUpgradedTower;

    [Space(10)] public TowerSO ThirdTimeUpgradedTower;

    private int upgradedTowerSellCost;
    private int secondTimeUpgradedTowerSellCost;
    private int thirdTimeUpgradedTowerSellCost;


    public int GetTowerSellCost()
    {
        return Tower.Cost / 2;
    }
    public int GetUpgradedTowerSellCost()
    {
        upgradedTowerSellCost = (Tower.Cost / 2) + (UpgradedTower.Cost / 2);
        return upgradedTowerSellCost;
    }

    public int GetSecondTimeUpgradedTowerSellCost()
    {
        secondTimeUpgradedTowerSellCost = upgradedTowerSellCost + (SecondTimeUpgradedTower.Cost / 2);
        return secondTimeUpgradedTowerSellCost;
    }

    public int GetThirdTimeUpgradedTowerSellCost()
    {
        thirdTimeUpgradedTowerSellCost = secondTimeUpgradedTowerSellCost + (ThirdTimeUpgradedTower.Cost / 2);
        return thirdTimeUpgradedTowerSellCost;
    }
}
