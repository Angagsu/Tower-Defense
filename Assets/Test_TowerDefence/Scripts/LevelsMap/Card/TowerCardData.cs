using System;
using System.Collections.Generic;

[Serializable]
public class TowerCardData 
{
    public TowerType TowerType;
    public TowerCardType CardType;
    public List<TowerCard> ReceivedCards;

    public TowerCardData(TowerType towerType, TowerCardType cardType)
    {
        TowerType = towerType;
        CardType = cardType;
        ReceivedCards = new List<TowerCard>();
    }

    public void AddTowerCard(TowerCard card)
    {
        ReceivedCards.Add(card);
    }
}
