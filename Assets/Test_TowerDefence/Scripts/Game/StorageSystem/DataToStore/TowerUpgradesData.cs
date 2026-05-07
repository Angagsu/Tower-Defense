using System.Collections.Generic;
using System;


[Serializable]
public class TowerUpgradesData 
{
    public List<TowerData> TowerUpgrades = new List<TowerData>();

    public void AddTowerData(TowerData towerData)
    {
        TowerUpgrades.Add(towerData);
    }
}
