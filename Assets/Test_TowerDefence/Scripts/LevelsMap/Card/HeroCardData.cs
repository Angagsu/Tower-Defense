using System;
using System.Collections.Generic;

[Serializable]
public class HeroCardData 
{
    public HeroCardType CardType;
    public List<HeroCard> ReceivedCards;

    public HeroCardData(HeroCardType cardType)
    {
        CardType = cardType;
        ReceivedCards = new List<HeroCard>();
    }
}
