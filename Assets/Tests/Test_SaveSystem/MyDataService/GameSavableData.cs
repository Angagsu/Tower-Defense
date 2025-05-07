using UnityEngine;

[System.Serializable]
public class GameSavableData : ISavable
{
    public int Gold;
    public int Rubin;
    public int Diamond;
    public float Money;


    public GameSavableData()
    {
        Gold = 0;
        Rubin = 0;
        Diamond = 0;
        Money = 0;
    }
}
