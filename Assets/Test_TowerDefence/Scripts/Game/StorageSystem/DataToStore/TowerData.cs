using System.Collections.Generic;
using System;



[Serializable]
public class TowerData 
{
    public TowerType TowerType;
    public TowerUpgradeLevel UpgradeLevel;
    public TowerCardType CardType;
    public bool IsAcquired;
    public int Price;
    public List<TowerCard> ReceivedCards;

    public int CostOfChange;
    public int ReceivedCardsAmount => ReceivedCards.Count;


    public TowerData(TowerType towerType, TowerUpgradeLevel upgradeLevel, TowerCardType cardType, bool isAcquired, int price)
    {
        TowerType = towerType;
        UpgradeLevel = upgradeLevel;
        CardType = cardType;
        IsAcquired = isAcquired;
        Price = price;
        ReceivedCards = new List<TowerCard>();

        switch (CardType)
        {
            case TowerCardType.Common:
                CostOfChange = 0;
                break;
            case TowerCardType.Epic:
                CostOfChange = 5;
                break;
            case TowerCardType.Mythic:
                CostOfChange = 10;
                break;
            case TowerCardType.Rare:
                CostOfChange = 15;
                break;
        }
    }
}
