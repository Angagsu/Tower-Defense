using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class SaveAndLoadSystemManager : MonoBehaviour
{

    private readonly string Player_Save_Data = "Player_Save_Data";
    private readonly string Game_Save_Data = "Game_Save_Data";

    

    [SerializeField] private bool isEncrypted = true;

    public static SaveAndLoadSystemManager Instance { get; private set; }
 
    private List<ISavablePlayerData> savablePlayerDataObjects;
    private List<ISavableGameData> savableGameDataObjects;

    private DataHandler<PlayerSavableData> playerDataHandler;
    private DataHandler<GameSavableData> gameDataHandler;

    private PlayerSavableData playerSavableData;
    private GameSavableData gameSavableData;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Found more then one SaveAndLoadSystemManager");
        }
        
        Instance = this;
    }

    private void Start()
    {
        Debug.Log(Application.persistentDataPath);

        playerDataHandler = new DataHandler<PlayerSavableData>(Player_Save_Data, playerSavableData, isEncrypted);
        gameDataHandler = new DataHandler<GameSavableData>(Game_Save_Data, gameSavableData, isEncrypted);

        savablePlayerDataObjects = FindAllPlayerDataPersistenceObjects();
        savableGameDataObjects = FindAllGameDataPersistenceObjects();

        LoadGame();
    }

    private void SetDataOnStart()
    {
        if (playerSavableData == null)
        {
            Debug.Log("No Player_Savable_Data was found. Initializing data to defaults.");
            playerSavableData = new PlayerSavableData();
        }

        if (gameSavableData == null)
        {
            Debug.Log("No Game_Savable_Data was found. Initializing data to defaults.");
            gameSavableData = new GameSavableData();
        }
    }

    public void SaveGame()
    {
        // pass the data to other scripts so they can update it
        foreach (ISavablePlayerData savableObj in savablePlayerDataObjects)
        {
            savableObj.SaveData(ref playerSavableData);
        }

        foreach (var savableObj in savableGameDataObjects)
        {
            savableObj.SaveData(ref gameSavableData);
        }

        // save that data to a file using the data handler
        playerDataHandler.SaveData(playerSavableData);
        gameDataHandler.SaveData(gameSavableData);
    }

    public void LoadGame()
    {
        // load any data from a file using the data handler
        gameSavableData = gameDataHandler.LoadData();
        playerSavableData = playerDataHandler.LoadData();

        SetDataOnStart();

        // push the loaded data to all other scripts that need it
        foreach (ISavablePlayerData savableObj in savablePlayerDataObjects)
        {
            savableObj.LoadData(playerSavableData);
        }

        foreach (var savableObj in savableGameDataObjects)
        {
            savableObj.LoadData(gameSavableData);
        }

    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<ISavablePlayerData> FindAllPlayerDataPersistenceObjects()
    {
        IEnumerable<ISavablePlayerData> savableObjects = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
            .OfType<ISavablePlayerData>();

        return new List<ISavablePlayerData>(savableObjects);
    }

    private List<ISavableGameData> FindAllGameDataPersistenceObjects()
    {
        IEnumerable<ISavableGameData> savableGameDataObjects = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
            .OfType<ISavableGameData>();

        return new List<ISavableGameData>(savableGameDataObjects);
    }
}
