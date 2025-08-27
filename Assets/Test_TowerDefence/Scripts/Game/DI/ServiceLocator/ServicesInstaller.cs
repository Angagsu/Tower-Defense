using UnityEngine;

public class ServicesInstaller : MonoBehaviour
{
    public static ServicesInstaller Instance { get; private set; }

    [SerializeField] private PlayerInputHandler inputHandler;
    [SerializeField] private LevelsMapDataHandler levelsDataHandler;
    [SerializeField] private StorageService storageService;

    private AsyncSceneLoadService loadService;


    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        loadService = GameEntryPoint.Instance.SceneLoader;

        ServiceLocator.AddService(loadService);
        ServiceLocator.AddService(inputHandler);
        ServiceLocator.AddService(storageService);
        ServiceLocator.AddService(levelsDataHandler);
    }
}
