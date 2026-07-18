using System;
using System.Linq;
using Unity.Services.CloudCode;
using Unity.Services.CloudCode.GeneratedBindings;
using UnityEngine;

public class PlayarDataManager : MonoBehaviour
{
    private PlayerDataServiceBindings bindings;
    public LoginController loginController;
    public string playerName;

    private void Start()
    {
        loginController.PlayerSignedIn += InitializePlayer;
        bindings = new PlayerDataServiceBindings(CloudCodeService.Instance);
    }

    private async void InitializePlayer()
    {
        try
        {
            var resultFromCloud = await bindings.SayHello(playerName);
            Debug.Log(resultFromCloud);
        }
        catch (CloudCodeException ex)
        {
            Debug.LogException(ex);
        }
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
            playerName = await bindings.HandleNewPlayerNameEntry(playerName);
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
