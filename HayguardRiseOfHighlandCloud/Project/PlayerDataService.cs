using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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
    public const string PLAYER_DATA_KEY = "playerData";
    public const string PLAYER_NAME_KEY = "playerName";

    private readonly ILogger<PlayerDataService> logger;
    private PlayerEconomyService playerEconomyService;


    public PlayerDataService(ILogger<PlayerDataService> logger, PlayerEconomyService playerEconomyService)
    {
        this.logger = logger;
        this.playerEconomyService = playerEconomyService;
    }

    public async Task SaveData(IExecutionContext context, IGameApiClient gameApiClient, string key, object value)
    {
        try
        {
            await gameApiClient.CloudSaveData.SetItemAsync(
                context,
                context.AccessToken!,
                context.ProjectId!,
                context.PlayerId!,
                new SetItemBody(key, value));

            logger.LogInformation("Successfully saved data for key: {Key}", key);
        }
        catch (ApiException ex)
        {
            logger.LogError("Failed to save data for key {Key}. Error: {Error}", key, ex.Message);
            throw new Exception($"Unable to save player data: {ex.Message}");
        }
    }

    public async Task SaveData(IExecutionContext context, IGameApiClient gameApiClient, string key, string value)
    {
        try
        {
            await gameApiClient.CloudSaveData.SetItemAsync(
                context,
                context.AccessToken!,
                context.ProjectId!,
                context.PlayerId!,
                new SetItemBody(key, value));

            logger.LogInformation("Successfully saved data for key: {Key}", key);
        }
        catch (ApiException ex)
        {
            logger.LogError("Failed to save data for key {Key}. Error: {Error}", key, ex.Message);
            throw new Exception($"Unable to save player data: {ex.Message}");
        }
    }


    [CloudCodeFunction("HandleNewPlayerNameEntry")]
    public async Task<string> HandleNewPlayerNameEntry(IExecutionContext context, IGameApiClient gameApiClient, string newName)
    {
        if (IsPlayerNameValid(newName))
        {
            var (_, playerData) = await TryGetPlayerData(context, gameApiClient);

            if(playerData == null)
            {
                throw new InvalidOperationException($"Player data not found for player: {context.PlayerId}");
            }

            playerData.DisplayName = newName;
            await SaveData(context, gameApiClient, PLAYER_DATA_KEY, playerData);

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

    [CloudCodeFunction("HandlePlayerSignIn")]
    public async Task<PlayerDataResponse> HandlePlayerSignIn(IExecutionContext context, IGameApiClient gameApiClient)
    {
        var (playerExists, playerData) = await TryGetPlayerData(context, gameApiClient);

        if (!playerExists || playerData == null)
        {
            return await InitializeNewPlayer(context, gameApiClient);
        }

        PlayerEconomyData economyData = await playerEconomyService.GetPlayerEconomyData(context, gameApiClient);

        return new PlayerDataResponse
        {
            PlayerData = playerData,
            EconomyData = economyData,
            IsNewPlayer = false
        };
    }

    private async Task<(bool playerExists, PlayerData? playerData)> TryGetPlayerData(IExecutionContext context, IGameApiClient gameApiClient)
    {
        try
        {
            var (success, playerDataJson) = await TryGetData(context, gameApiClient, PLAYER_DATA_KEY);

            if (playerDataJson == null)
            {
                return (false, null);
            }

            var playerData = JsonConvert.DeserializeObject<PlayerData>($"{playerDataJson}");
            return (playerData != null, playerData);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Error deserializing player data for player: {context.PlayerId}");
            return (false, null);
        }
    }

    public async Task<(bool success, string? value)> TryGetData(IExecutionContext context, IGameApiClient gameApiClient, string key)
    {
        try
        {
            var response = await gameApiClient.CloudSaveData.GetItemsAsync(
                context,
                context.AccessToken,
                context.ProjectId,
                context.PlayerId ?? throw new InvalidOperationException("PlayerId is null"),
                new List<string> { key });

            var retrivedItem = response.Data.Results.FirstOrDefault();

            if (retrivedItem != null)
            {
                return (true, Convert.ToString(retrivedItem.Value));
            }

            return (false, null);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Error retrieving data from CloudSave for player: {context.PlayerId}");
            return (false, null);
        }
    }

    private async Task<PlayerDataResponse> InitializeNewPlayer(IExecutionContext context, IGameApiClient gameApiClient)
    {
        PlayerData newPlayerData = new PlayerData
        {
            DisplayName = "NewPlayer",
            Experience = 0
        };

        PlayerEconomyData newEconomyData;

        try
        {
            await SaveData(context, gameApiClient, PLAYER_DATA_KEY, newPlayerData);

            newEconomyData = await playerEconomyService.InitializeNewPlayerEconomy(context, gameApiClient);

            logger.LogInformation($"Initialized new player with ID: {context.PlayerId}");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Error initializing new player with ID: {context.PlayerId}");
            throw new Exception($"Faild to initialize new player: {ex.Message}");
        }

        return new PlayerDataResponse
        {
            PlayerData = newPlayerData,
            EconomyData = newEconomyData,
            IsNewPlayer = true
        };
    }
}


