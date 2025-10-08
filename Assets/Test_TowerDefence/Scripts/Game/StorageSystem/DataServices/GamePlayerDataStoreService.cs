using System;
using UnityEngine;

public class GamePlayerDataStoreService : MonoBehaviour, IService
{
    public event Action<GamePlayerData> PlayerDataChanged;

    private const string PLAYER_DATA_KEY = "PlayerData.Json";

    [SerializeField] private StorageService storageService;

    private GamePlayerData playerData = new();


    private void Save()
    {
        storageService.JsonSaver.Save(PLAYER_DATA_KEY, playerData, OnSave);
    }

    public void Load()
    {
        try
        {
            storageService.JsonSaver.Load<GamePlayerData>(PLAYER_DATA_KEY, OnLoad);
        }
        catch (Exception e)
        {
            Debug.Log(e);
            Save();
        }
    }

    private void OnLoad(GamePlayerData playerData)
    {
        this.playerData = playerData;
        PlayerDataChanged?.Invoke(this.playerData);
    }

    private void OnSave(bool isSaved)
    {
        Debug.Log("isSaved" + isSaved);
        PlayerDataChanged?.Invoke(playerData);
    }

    public void StoreChangedPlayerData(GamePlayerData playerData)
    {
        this.playerData = playerData;
        Save();
    }

    public GamePlayerData PlayerData { get { return playerData; } }
}
