using System;

public class Wallet 
{
    public event Action<int> CoinsChanged;

    private IPersistentData persistentData;

    public Wallet(IPersistentData persistentData)
        => this.persistentData = persistentData;

    public void AddCoins(int coins)
    {
        if (coins < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(coins));
        }

        persistentData.PlayerData.Money += coins;

        CoinsChanged?.Invoke(persistentData.PlayerData.Money);
    }

    public int GetCurrentCoins() => persistentData.PlayerData.Money;

    public bool IsEnough(int coins)
    {
        if (coins < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(coins));
        }

        return persistentData.PlayerData.Money >= coins;
    }

    public void Spend(int coins)
    {
        if (coins < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(coins));
        }

        persistentData.PlayerData.Money -= coins;

        CoinsChanged?.Invoke(persistentData.PlayerData.Money);
    }
}
