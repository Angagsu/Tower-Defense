using Newtonsoft.Json;
using System;
using System.Collections.Generic;



[Serializable]
public class TowersDataToStore
{
    
    public List<int> SelectedAcquiredTowersByIndex;

    [JsonProperty("twr")]
    public Dictionary<TowerType, TowerUpgradesData> AllTowers = new();

    public TowersDataToStore()
    {
        for (int i = 0; i < AllHeroAndTowerTypes.TowerTypes.Count; i++) 
        {
            TowerUpgradesData towerUpgradesData = new TowerUpgradesData();

            for (int j = 0; j < AllHeroAndTowerTypes.TowerCards.Count; j++)
            {
                towerUpgradesData.AddTowerData(new TowerData(AllHeroAndTowerTypes.TowerTypes[i], TowerUpgradeLevel.Level_1, AllHeroAndTowerTypes.TowerCards[j], isAcquired: false, price: default));
            }

            AllTowers.Add(AllHeroAndTowerTypes.TowerTypes[i], towerUpgradesData);  
        }

        AllTowers[TowerType.Arrow].TowerUpgrades[0].IsAcquired = true;
        AllTowers[TowerType.Defender].TowerUpgrades[0].IsAcquired = true;
        AllTowers[TowerType.Golem].TowerUpgrades[0].IsAcquired = true;
        AllTowers[TowerType.Ice].TowerUpgrades[0].IsAcquired = true;

        SelectedAcquiredTowersByIndex = new List<int>();
    }
}
