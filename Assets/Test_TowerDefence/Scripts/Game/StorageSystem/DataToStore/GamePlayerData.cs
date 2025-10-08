[System.Serializable]
public class GamePlayerData
{
    public int Diamonds;
    public int Gold;
    public int Energy;
    
    

    public GamePlayerData() 
    {
        Diamonds = 0;
        Gold = 0;
        Energy = 20;
    }

    public GamePlayerData(int diamonds,int gold, int energy)
    {
        Diamonds = diamonds;
        Gold = gold;
        Energy = energy;
    }
}
