using System;
using UnityEngine;

[DefaultExecutionOrder(9)]
public class GameplayPlayerDataHandler : MonoBehaviour
{
    public event Action<int> LivesAmountChanged;
    public event Action<int> MoneyAmountChanged;
    public event Action<int> WavesAmountChanged;

    public int Money => playerData.Money;
    public int Lives => playerData.Lives;
    public int Waves => playerData.Waves;

    [SerializeField] private int money = 1000;
    [SerializeField] private int lives = 20;

    private int waves = 0;

    private GameplayPlayerData playerData;



    private void Awake()
    {
        playerData = new GameplayPlayerData(money, lives, waves);
    }

    private void Start()
    {
        LivesAmountChanged?.Invoke(playerData.Lives);
        MoneyAmountChanged?.Invoke(playerData.Money);
    }

    public void IncreaseLives(int lives)
    {
        this.lives += lives;
        playerData.Lives += lives;
        LivesAmountChanged?.Invoke(playerData.Lives);
    }

    public void ReduceLives(int lives)
    {
        this.lives -= lives;
        playerData.Lives -= lives;
        LivesAmountChanged?.Invoke(playerData.Lives);
    }

    public void IncreaseMoney(int money)
    {
        this.money += money;
        playerData.Money += money;
        MoneyAmountChanged?.Invoke(playerData.Money);
    }

    public void ReduceMoney(int money)
    {
        this.money -= money;
        playerData.Money -= money;
        MoneyAmountChanged?.Invoke(playerData.Money);
    }

    public void IncreaseWaves(int waves)
    {
        this.waves += waves;
        playerData.Waves += waves;
        WavesAmountChanged?.Invoke(playerData.Waves);
    }

    public void ReduceWaves(int waves)
    {
        this.waves -= waves;
        playerData.Waves -= waves;
        WavesAmountChanged?.Invoke(playerData.Waves);
    }
}
