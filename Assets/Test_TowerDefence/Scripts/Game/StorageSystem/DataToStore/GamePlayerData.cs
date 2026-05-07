[System.Serializable]
public class GamePlayerData
{
    public int Diamonds;
    public int Gold;
    public int Energy;
    
    

    public GamePlayerData() 
    {
        Diamonds = 1000;
        Gold = 90000;
        Energy = 20;
    }

    public GamePlayerData(int diamonds,int gold, int energy)
    {
        Diamonds = diamonds;
        Gold = gold;
        Energy = energy;
    }
}
