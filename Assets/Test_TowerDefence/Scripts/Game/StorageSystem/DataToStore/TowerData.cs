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
    public List<TowerCard> ReseivedCards;

    public TowerData(TowerType towerType, TowerUpgradeLevel upgradeLevel, TowerCardType cardType, bool isAcquired, int price)
    {
        TowerType = towerType;
        UpgradeLevel = upgradeLevel;
        CardType = cardType;
        IsAcquired = isAcquired;
        Price = price;
        ReseivedCards = new List<TowerCard>();
    }
}
