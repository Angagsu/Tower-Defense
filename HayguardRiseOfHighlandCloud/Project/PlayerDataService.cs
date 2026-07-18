using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Unity.Services.CloudCode.Apis;
using Unity.Services.CloudCode.Core;
using Unity.Services.CloudCode.Shared;
using Unity.Services.CloudSave.Model;

namespace HayguardRiseOfHighlandCloud;

public class PlayerDataService
{       
    //private readonly IGameApiClient m_GameApiClient;
    private readonly ILogger<PlayerDataService> m_Logger;
    public const string PLAYER_NAME_KEY = "playerName";

    public PlayerDataService(ILogger<PlayerDataService> logger)
    {
        m_Logger = logger;
    }

    //public PlayerDataService(IGameApiClient gameApiClient, ILogger<PlayerDataService> logger)
    //{
    //    m_GameApiClient = gameApiClient;
    //    m_Logger = logger;
    //}

    private async Task SavePlayerData(IExecutionContext context, IGameApiClient gameApiClient, string key, string value)
    {
        try
        {
            await gameApiClient.CloudSaveData.SetItemAsync(
                context,
                context.AccessToken!,
                context.ProjectId!,
                context.PlayerId!,
                new SetItemBody(key, value));

            m_Logger.LogInformation("Successfully saved data for key: {Key}", key);
        }
        catch (ApiException ex)
        {
            m_Logger.LogError("Failed to save data for key {Key}. Error: {Error}", key, ex.Message);
            throw new Exception($"Unable to save player data: {ex.Message}");
        }
    }

    private async Task<string> GetPlayerData(IExecutionContext context, IGameApiClient gameApiClient, string key)
    {
        try
        {
            var result = await gameApiClient.CloudSaveData.GetItemsAsync(
                context,
                context.AccessToken!,
                context.ProjectId!,
                context.PlayerId!,
                new List<string> { key });

            var data = result.Data.Results.FirstOrDefault()?.Value?.ToString() ?? string.Empty;
            m_Logger.LogInformation("Successfully retrieved data for key: {Key}", key);
            return data;
        }
        catch (ApiException ex)
        {
            m_Logger.LogError("Failed to retrieve data for key {Key}. Error: {Error}", key, ex.Message);
            throw new Exception($"Unable to retrieve player data: {ex.Message}");
        }
    }

    

    [CloudCodeFunction("SayHello")]
    public string Hello(string name)
    {
        return $"Hello, {name}!";
    }

    [CloudCodeFunction("GetServerTime")]
    public string GetServerTime(IExecutionContext context)
    {
        return DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);
    }

    [CloudCodeFunction("HandleNewPlayerNameEntry")]
    public async Task<string> HandleNewPlayerNameEntry(IExecutionContext context, IGameApiClient gameApiClient, string newName)
    {
        if (IsPlayerNameValid(newName))
        {
            await SavePlayerData(context, gameApiClient, PLAYER_NAME_KEY, newName);
            return newName;
        }

        throw new ArgumentException("Invalid player name. Name must be between 4 and 16 characters and contain only letters and digits.");
    }

    private bool IsPlayerNameValid(string name)
    {
        if (name.Length is < 4 or > 16)
        {
            return false;
        }

        if (!name.All(c => char.IsLetterOrDigit(c)))
        {
            return false;
        }

        return true;
    }
}


