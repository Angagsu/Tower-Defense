using Newtonsoft.Json;
using System;
using System.Collections.Generic;



[Serializable]
public class TowersDataToStore
{
    
    public List<int> SelectedAcquiredTowersByIndex;

    [JsonProperty("twr")]
    public Dictionary<TowerType, TowerUpgradesData> AllTowers = new();
    [JsonProperty("crds")]
    public Dictionary<TowerType, TowerUpgradesCardData> AllTowerUpgradesCards = new();

    public TowersDataToStore()
    {
        for (int i = 0; i < AllHeroAndTowerTypes.TowerTypes.Count; i++) 
        {
            TowerUpgradesData towerUpgradesData = new TowerUpgradesData();
            TowerUpgradesCardData towerUpgradesCardData = new TowerUpgradesCardData((TowerType)i);

            for (int j = 0; j < AllHeroAndTowerTypes.TowerCards.Count; j++)
            {
                towerUpgradesData.AddTowerData(new TowerData(AllHeroAndTowerTypes.TowerTypes[i], TowerUpgradeLevel.Level_1, AllHeroAndTowerTypes.TowerCards[j], isAcquired: false, price: default));
                towerUpgradesCardData.AddTowerCardData(new TowerCardData(AllHeroAndTowerTypes.TowerTypes[i], AllHeroAndTowerTypes.TowerCards[j]));
            }

            AllTowers.Add(AllHeroAndTowerTypes.TowerTypes[i], towerUpgradesData);
            AllTowerUpgradesCards.Add(AllHeroAndTowerTypes.TowerTypes[i], towerUpgradesCardData);    
        }

        AllTowers[TowerType.Arrow].TowerUpgrades[0].IsAcquired = true;
        AllTowers[TowerType.Defender].TowerUpgrades[0].IsAcquired = true;
        AllTowers[TowerType.Golem].TowerUpgrades[0].IsAcquired = true;
        AllTowers[TowerType.Ice].TowerUpgrades[0].IsAcquired = true;

        SelectedAcquiredTowersByIndex = new List<int>();
    }
}
