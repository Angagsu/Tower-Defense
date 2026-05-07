using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameplayExtraLoot 
{
    public int Energy {  get; private set; }
    public int Gold {  get; private set; }

    public List<HeroCard> HeroCards { get; private set; } = new List<HeroCard>();
    public List<TowerCard> TowerCards { get; private set; } = new List<TowerCard>();

    private int starAmount;

    public int[] CalculateExtraLoot(int starsAmount)
    {
        this.starAmount = starsAmount;
        if (starsAmount == 3) 
        {
            return new int[] { Gold = Random.Range(10, 15), Energy = Random.Range(1, 3)};
        }
        else if (starsAmount == 2)
        {
            return new int[] { Gold = 10, Energy = Random.Range(1, 3) };
        }
        else if(starsAmount == 1)
        {
            return new int[] { Energy = 1 };
        }

        return default;
    }


    public List<HeroCard> GetHeroCardLoot()
    {
        if (starAmount < 3)
        {
            return null;
        }

        int minValue = (int)HeroType.Knight;
        int maxValue = (int)HeroType.Mage + 1;
        int result = Random.Range(minValue, maxValue);

        HeroCards.Add(new HeroCard((HeroType)result));

        return HeroCards;
    } 

    public List<TowerCard> GetTowerCardLoot()
    {
        if (starAmount < 2) return null;

        int minValue = (int)TowerType.Arrow;
        int maxValue = (int)TowerType.Lightning + 1;
        int towerResult = Random.Range(minValue, maxValue);

        int minCardRarityValue = (int)TowerCardType.Common;
        int maxCardRarityValue = (int)TowerCardType.Rare + 1;
        int cardResult = Random.Range(minCardRarityValue, maxCardRarityValue);

        TowerCards.Add(new TowerCard((TowerType)towerResult, (TowerCardType)cardResult));


        towerResult = Random.Range(minValue, maxValue);
        cardResult = Random.Range(minCardRarityValue, maxCardRarityValue);

        TowerCards.Add(new TowerCard((TowerType)towerResult, (TowerCardType)cardResult));

        if(starAmount == 3)
        {
            towerResult = Random.Range(minValue, maxValue);
            cardResult = Random.Range(minCardRarityValue, maxCardRarityValue);

            TowerCards.Add(new TowerCard((TowerType)towerResult, (TowerCardType)cardResult));

            towerResult = Random.Range(minValue, maxValue);
            cardResult = Random.Range(minCardRarityValue, maxCardRarityValue);

            TowerCards.Add(new TowerCard((TowerType)towerResult, (TowerCardType)cardResult));
        }

        return TowerCards;
    }
}
