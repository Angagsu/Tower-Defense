using UnityEngine;

public class GameplayTestBootstrap : MonoBehaviour
{
    [SerializeField] private Transform characterSpawnPoint;
    [SerializeField] private Transform mazeSpawnPoint;
    [SerializeField] private CharacterFactory characterFactory;
    [SerializeField] private MazeFactory mazeFactory;

    private IDataProvider dataProvider;
    private IPersistentData persistentPlayerData;

    private void Awake()
    {
        InitializeData();
        DoTestSpawn();
    }

    private void DoTestSpawn()
    {
        CharacterMark character = characterFactory.Get(persistentPlayerData.PlayerData.SelectedCharacterSkin,
            characterSpawnPoint.position);
        MazeMark maze = mazeFactory.Get(persistentPlayerData.PlayerData.SelectedMazeSkin, mazeSpawnPoint.position);

        Debug.Log($"Character Spawned {persistentPlayerData.PlayerData.SelectedCharacterSkin} and Maze " +
            $"{persistentPlayerData.PlayerData.SelectedMazeSkin}");
    }

    private void InitializeData()
    {
        persistentPlayerData = new PersistentData();
        dataProvider = new DataLocalProvider(persistentPlayerData);

        LoadDataOrInit();
    }

    private void LoadDataOrInit()
    {
        if (!dataProvider.TryLoad())
        {
            persistentPlayerData.PlayerData = new PlayerData();
        }
    }

}
