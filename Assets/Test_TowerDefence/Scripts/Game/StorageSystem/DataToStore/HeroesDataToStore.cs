using Newtonsoft.Json;
using System;
using System.Collections.Generic;




[Serializable]
public class HeroesDataToStore 
{
    public List<int> SelectedAcquiredHeroesByIndex;

    [JsonProperty("hrs")]
    public Dictionary<HeroType, HeroData> AllHeroes = new();
    
    public Dictionary<HeroType, HeroCardData> AllCards = new();

    public HeroesDataToStore()
    {
        for (int i = 0; i < AllHeroTypes.HeroType.Count; i++)
        {           
            AllHeroes.Add((HeroType)i, new HeroData(AllHeroTypes.HeroType[i], HeroUpgradeLevel.Level_1, AllHeroTypes.HeroCards[i], isAcquired: false, price: default));
        }
        AllHeroes[HeroType.Knight].IsAcquired = true;
        AllHeroes[HeroType.Archer].IsAcquired = true;

        SelectedAcquiredHeroesByIndex = new List<int>();

        for (int i = 0; i < AllHeroTypes.HeroCards.Count; i++)
        {
            AllCards.Add((HeroType)i, new HeroCardData(AllHeroTypes.HeroCards[i]));
        }
    }
}
