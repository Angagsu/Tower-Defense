using Newtonsoft.Json;
using System;
using System.Linq;
using Unity.Services.CloudCode;
using Unity.Services.CloudCode.GeneratedBindings;
using Unity.Services.CloudCode.GeneratedBindings.HayguardRiseOfHighlandCloud;
using UnityEngine;

public class PlayarDataManager : MonoBehaviour
{
    public event Action<PlayerData> PlayerDataUpdated;
    [SerializeField] private PlayerEconomyManager playerEconomyManager;
    private PlayerDataServiceBindings playerDataServiceBindings;
    public LoginController loginController;
    public string playerName;
    public PlayerData PlayerDataLocal;

    private void Start()
    {
        loginController.PlayerSignedIn += InitializePlayer;

        playerDataServiceBindings = new PlayerDataServiceBindings(CloudCodeService.Instance);
        
    }

    private async void InitializePlayer()
    {
        try
        {
            var playerDataResponse = await playerDataServiceBindings.HandlePlayerSignIn();

            PlayerDataLocal = playerDataResponse.PlayerData;
            PlayerDataUpdated?.Invoke(PlayerDataLocal);

            playerEconomyManager.HandleEconomyUpdate(playerDataResponse.EconomyData);

            LogResponse(playerDataResponse);
        }
        catch (CloudCodeException ex)
        {
            Debug.LogException(ex);
        }
    }

    private void LogResponse(PlayerDataResponse playerDataResponse)
    {
        string economyJson = JsonConvert.SerializeObject(playerDataResponse.EconomyData, Formatting.Indented);

        Debug.Log(
            $"===== Player Sign-In Response =====\n" +
            $"Name: {playerDataResponse.PlayerData.DisplayName}\n" +
            $"New player: {playerDataResponse.IsNewPlayer}\n" +
            $"XP: {playerDataResponse.PlayerData.Experience}\n" +
            $"Economy: {economyJson}\n" +
            $"============================================"
            );
    }

    public async void SaveNewPlayerName()
    {
       //if(!IsPlayerNameValid(playerName))
       //{
       //    Debug.LogError("Invalid player name. It must be between 4 and 16 characters and contain only letters and digits.");
       //    return;
       //}

        try
        {
            playerName = await playerDataServiceBindings.HandleNewPlayerNameEntry(playerName);
            Debug.Log($"Player name saved successfully: {playerName}");
        }
        catch (Exception ex)
        {
            Debug.LogWarning("Player name invalid or already exists. Please choose a different name.");
            Debug.LogException(ex);
        }
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

    private void OnDestroy()
    {
        loginController.PlayerSignedIn -= InitializePlayer;
    }
}
