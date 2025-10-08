using System;

[Serializable]
public class Card 
{
    public HeroType CardType;

    public Card(HeroType cardType)
    {
        CardType = cardType;
    }

}
