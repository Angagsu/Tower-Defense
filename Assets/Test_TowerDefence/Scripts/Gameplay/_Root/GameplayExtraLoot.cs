using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameplayExtraLoot 
{
    public int Energy {  get; private set; }
    public int Gold {  get; private set; }
    public List<Card> Cards { get; private set; }


    public int[] CalculateExtraLoot(int starsAmount)
    {
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

    public List<Card> CalculateHeroCardLoot(int starsAmount)
    {
        if (starsAmount < 3)
        {
            return null;
        }
        int minValue = (int)HeroType.Knight;
        int maxValue = (int)HeroType.Mage + 1;
        int result = Random.Range(minValue, maxValue);
        Cards = new List<Card>();
        Cards.Add(new Card((HeroType)result));
        result = Random.Range(0, maxValue);
        Cards.Add(new Card((HeroType)result));
        return Cards;
    } 

    private List<Card> CalculateTowerCardLoot()
    {
        return null;
    }
}
