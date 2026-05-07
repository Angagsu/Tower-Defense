using System;


[Serializable]
public class TowerCard 
{
    public TowerType TowerType;
    public TowerCardType CardType;


    public TowerCard(TowerType towerType, TowerCardType cardType)
    {
        TowerType = towerType; 
        CardType = cardType;
    }
}
