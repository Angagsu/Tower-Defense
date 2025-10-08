using System;
using System.Collections.Generic;

[Serializable]
public class HeroCardData 
{
    public HeroCardType CardType;
    public List<Card> ReceivedCards;

    public HeroCardData(HeroCardType cardType)
    {
        CardType = cardType;
        ReceivedCards = new List<Card>();
    }
}
