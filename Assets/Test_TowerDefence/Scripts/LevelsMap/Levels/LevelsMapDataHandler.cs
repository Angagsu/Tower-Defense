using System.Collections.Generic;
using UnityEngine;

public class LevelsMapDataHandler : MonoBehaviour
{
    public int ReceivedStars { get; private set; }
    public int PlayCost => playCost;

    [SerializeField] private List<LevelsMapLevel> Levels;
    [SerializeField] private LevelsMapMainUI levelsMapMainUI;
    [SerializeField] private int playCost = 5;

    private LevelsMapDataStoreService levelsDataStoreService;
    private AsyncSceneLoadService sceneLoadService;
    private GamePlayerDataHandler playerDataHandler;
    private LevelsMapLevel currentSelectedLevel;

    private int levelIndex;
    private int sceneListIndexOffset = 3;
    

    [Inject]
    public void Construct(AsyncSceneLoadService sceneLoadService, LevelsMapDataStoreService levelsDataStoreService, GamePlayerDataHandler playerDataHandler)
    {
        this.sceneLoadService = sceneLoadService;
        this.levelsDataStoreService = levelsDataStoreService;
        this.playerDataHandler = playerDataHandler;

        this.levelsDataStoreService.SetLevelsMapDataHandler(this);
        this.levelsDataStoreService.Load();
    }

    private void Start()
    {
        Levels[0].UnlockLevelButton();
    }

    public void SetSavedLevelsDate(LevelDataToStor levelDataToStor, int index)
    {
        Levels[index].SetParams(levelDataToStor);
    }

    public void OnLevelButtonClicked(LevelsMapLevel levelsMapLevel)
    {
        currentSelectedLevel = levelsMapLevel;
        levelsMapMainUI.ShowPreparingToPlayPanel();
    }

    public bool TryToStartPlay()
    {
        if (playerDataHandler.TrySpendEnergy(playCost))
        {
            playerDataHandler.SaveChangedPlayerData();

            levelsDataStoreService.SetCurrentSelectedLevel(currentSelectedLevel.LevelDataToStor.LevelID);

            levelIndex = currentSelectedLevel.LevelDataToStor.LevelID + sceneListIndexOffset;
            sceneLoadService.LoadSceneByIndex(levelIndex);
            return true;
        }
        else
        {
            Debug.Log("Not enought energy to play !!!");
            return false;
        }
        
    }

    public void SetReceivedStars(int receivedStars)
    {
        ReceivedStars = receivedStars;
    }
}
