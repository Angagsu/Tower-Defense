using Newtonsoft.Json;
using System;
using System.Collections.Generic;




[Serializable]
public class HeroesDataToStore 
{
    public List<int> SelectedAcquiredHeroesByIndex;

    [JsonProperty("hrs")]
    public Dictionary<HeroType, HeroData> AllHeroes = new();
    
    //public Dictionary<HeroType, HeroCardData> AllCards = new();

    public HeroesDataToStore()
    {
        for (int i = 0; i < AllHeroAndTowerTypes.HeroTypes.Count; i++)
        {           
            AllHeroes.Add((HeroType)i, new HeroData(AllHeroAndTowerTypes.HeroTypes[i], HeroUpgradeLevel.Level_1, AllHeroAndTowerTypes.HeroCards[i], isAcquired: false, price: default));
           // AllCards.Add((HeroType)i, new HeroCardData(AllHeroAndTowerTypes.HeroCards[i]));
        }

        AllHeroes[HeroType.Knight].IsAcquired = true;
        AllHeroes[HeroType.Archer].IsAcquired = true;

        SelectedAcquiredHeroesByIndex = new List<int>();
    }
}
