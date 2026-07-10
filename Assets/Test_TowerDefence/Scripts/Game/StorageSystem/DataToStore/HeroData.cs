using System.Collections.Generic;

[System.Serializable]
public class HeroData 
{
    public HeroType HeroType;
    public HeroUpgradeLevel UpgradeLevel;
    public HeroCardType CardType;
    public bool IsAcquired;
    public int Price;
    public List<HeroCard> ReceivedCards;
    public int ReceivedCardsAmount => ReceivedCards.Count;


    public HeroData(HeroType heroType, HeroUpgradeLevel upgradeLevel, HeroCardType cardType, bool isAcquired, int price)
    {
        HeroType = heroType;
        UpgradeLevel = upgradeLevel;
        CardType = cardType;
        IsAcquired = isAcquired;
        Price = price;
        ReceivedCards = new List<HeroCard>();
    }
}
