using UnityEngine;

public class ServicesInstaller : MonoBehaviour
{
    public static ServicesInstaller Instance { get; private set; }

    [SerializeField] private PlayerInputHandler inputHandler;
    [SerializeField] private StorageService storageService;
    [SerializeField] private LevelsMapDataStoreService levelsDataStoreService; 
    [SerializeField] private GamePlayerDataStoreService playerDataStorService;
    [SerializeField] private GamePlayerDataHandler playerDataHandler;
    [SerializeField] private HeroesDataStoreService heroesDataStoreService;
    [SerializeField] private TowersDataStoreService towersDataStoreService;
    [SerializeField] private VideoDownloadHandler videoDownloadHandler;

    private AsyncSceneLoadService loadService;


    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        loadService = GameEntryPoint.Instance.SceneLoader;

        ServiceLocator.AddService(loadService);
        ServiceLocator.AddService(inputHandler);
        ServiceLocator.AddService(storageService);
        ServiceLocator.AddService(levelsDataStoreService);
        ServiceLocator.AddService(playerDataStorService);
        ServiceLocator.AddService(playerDataHandler);
        ServiceLocator.AddService(heroesDataStoreService);
        ServiceLocator.AddService(towersDataStoreService);
        ServiceLocator.AddService(videoDownloadHandler);
    }
}
