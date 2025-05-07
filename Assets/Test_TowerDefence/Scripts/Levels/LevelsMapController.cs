using System.Collections.Generic;
using UnityEngine;

public class LevelsMapController : MonoBehaviour
{
    [field: SerializeField] public List<LevelsMapLevel> Levels { get; private set; }

    private LevelsMapDataHandler levelsDataHandler;
    private AsyncSceneLoadService sceneLoadService;

    [Inject]
    public void Costruct(AsyncSceneLoadService sceneLoadService, LevelsMapDataHandler levelsDataHandler)
    {
        this.sceneLoadService = sceneLoadService;
        this.levelsDataHandler = levelsDataHandler;

        levelsDataHandler.SetLevelsMapController(this);
        levelsDataHandler.Load();
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
        levelsDataHandler.SetCurrentSelectedLevel(levelsMapLevel.LevelDataToStor.LevelID);

        int levelIndex = levelsMapLevel.LevelDataToStor.LevelID + 3;
        sceneLoadService.LoadSceneByIndex(levelIndex);
    }
}
