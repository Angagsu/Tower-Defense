using System;
using System.Collections.Generic;


[Serializable]
public class TowerUpgradesCardData 
{
    public TowerType TowerType;
    public List<TowerCardData> TowerUpgradesCards = new List<TowerCardData>();

    public TowerUpgradesCardData(TowerType towerType)
    {
        TowerType = towerType;
    }

    public void AddTowerCardData(TowerCardData cardData)
    {
        TowerUpgradesCards.Add(cardData);
    }
}
