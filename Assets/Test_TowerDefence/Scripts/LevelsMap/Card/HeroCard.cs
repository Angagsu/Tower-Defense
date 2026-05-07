using System;

[Serializable]
public class HeroCard 
{
    public HeroType CardType;

    public HeroCard(HeroType cardType)
    {
        CardType = cardType;
    }

}
