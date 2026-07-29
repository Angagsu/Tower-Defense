using System;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.CloudCode.GeneratedBindings.HayguardRiseOfHighlandCloud;
using Unity.Services.Economy;
using UnityEngine;

public class PlayerEconomyManager : MonoBehaviour
{
    public PlayerEconomyData EconomyDataLocal { get; private set; } = new PlayerEconomyData
    {
        Currencies = new Dictionary<string, int>(),
        ItemInventory = new Dictionary<string, int>()
    };

    public const string GOLD_CURRENCY_KEY = "GOLD";

    public int Gold
    {
        get => GetCurrencyAmount(GOLD_CURRENCY_KEY);
    }

    public int GetCurrencyAmount(string currencyKey)
    {
        if (EconomyDataLocal.Currencies.TryGetValue(currencyKey, out int amount))
        {
            return amount;
        }

        return 0;
    }

    public event Action<PlayerEconomyData> PlayerEconomyUpdated;
    public event Action EconomyConfigSynced;



    private void Start()
    {
        AuthenticationService.Instance.SignedIn += SyncEconomyConfig;
    }

    private async void SyncEconomyConfig()
    {
        try
        {
            await EconomyService.Instance.Configuration.SyncConfigurationAsync();
            Debug.Log("Economy configuration synced");
            EconomyConfigSynced?.Invoke();
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    public void HandleEconomyUpdate(PlayerEconomyData economyData)
    {
        EconomyDataLocal = economyData;
        PlayerEconomyUpdated?.Invoke(EconomyDataLocal);
    }

    private void OnDisable()
    {
        AuthenticationService.Instance.SignedIn -= SyncEconomyConfig;
    }
}
