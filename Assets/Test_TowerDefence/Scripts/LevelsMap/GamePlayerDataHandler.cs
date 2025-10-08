using System;
using UnityEngine;

public class GamePlayerDataHandler : MonoBehaviour, IService
{
    public event Action<GamePlayerData> PlayerDataChanged;
    public event Action<TimeSpan> EnergyTimerTicked;

    public int Diamonds { get; private set; }
    public int Gold { get; private set; }  
    public int Energy {  get; private set; }


    [SerializeField] private GamePlayerDataStoreService gamePlayerDataStoreService;

    private GamePlayerData playerData;
    private EnergyCounter energyCounter;

    private const int ENERGY_MAX_THRESHOLD = 40;


    private void Start()
    {
        gamePlayerDataStoreService.PlayerDataChanged += OnPlayerDataChange;

        gamePlayerDataStoreService.Load();

        energyCounter = new EnergyCounter(this, Energy, ENERGY_MAX_THRESHOLD);
        energyCounter.EnergyAmountChanged += OnEnergyChange;
        energyCounter.EnergyTimerTicked += OnEnergyTimerTick;
        energyCounter.ApplyOfflineProgress();

        StartCoroutine(energyCounter.Regenerate());
    }

    private void OnEnergyTimerTick(TimeSpan ts)
    {
        EnergyTimerTicked?.Invoke(ts);
    }

    private void OnEnergyChange(int value)
    {
        Energy = value;
        playerData.Energy = value;

        SaveChangedPlayerData();
    }

    private void SetPlayerData(GamePlayerData playerData)
    {
        this.playerData = playerData;

        Diamonds = playerData.Diamonds;
        Gold = playerData.Gold;
        Energy = playerData.Energy;
    }

    private void OnPlayerDataChange(GamePlayerData playerData)
    {
        SetPlayerData(playerData);
        PlayerDataChanged?.Invoke(playerData);
    }

    public void SaveChangedPlayerData()
    {
        gamePlayerDataStoreService.StoreChangedPlayerData(playerData);
        PlayerDataChanged?.Invoke(playerData);
    }

    public void AddDiamond(int value)
    {
        Diamonds += value;
        playerData.Diamonds += value;

        SaveChangedPlayerData();
    }

    public void AddGold(int value)
    {
        Gold += value;
        playerData.Gold += value;

        SaveChangedPlayerData();
    }

    public void AddEnergy(int value) 
    {
        Energy = Mathf.Min(Energy + value, ENERGY_MAX_THRESHOLD);
        playerData.Energy = Energy;

        SaveChangedPlayerData();
    }

    public bool TrySpendDiamond(int value)
    {
        if (Diamonds - value < 0)
        {
            return false;
        }
        else
        {
            Diamonds -= value;
            playerData.Diamonds -= value;
            SaveChangedPlayerData();
            return true;
        }
    }

    public bool TrySpendGold(int value)
    {
        if (Gold - value < 0)
        {
            return false;
        }
        else
        {
            Gold -= value;
            playerData.Gold -= value;
            SaveChangedPlayerData();
            return true;
        }
    }

    public bool TrySpendEnergy(int value)
    {
        if (Energy - value < 0)
        {
            Debug.Log("Not Enough Energy to spend!");
            return false;
        }
        else
        {
            Energy -= value;
            playerData.Energy -= value;
            SaveChangedPlayerData();
            return true;
        }
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause) energyCounter.SaveEnergyState();
        else energyCounter.ApplyOfflineProgress();
    }

    private void OnApplicationQuit()
    {
        energyCounter.SaveEnergyState();
        gamePlayerDataStoreService.StoreChangedPlayerData(playerData);
    }
}
