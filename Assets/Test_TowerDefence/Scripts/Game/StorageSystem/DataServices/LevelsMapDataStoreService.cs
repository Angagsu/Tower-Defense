using System;
using UnityEngine;


public class LevelsMapDataStoreService : MonoBehaviour, IService
{
    public int ReceivedStars => receivedStars;

    private const string ACHIEVED_LEVELS_DATA_KEY = "AchievedLevelsData.Json";


    [SerializeField] private StorageService storageService;


    private LevelsMapDataToStor levelsMapDataToStor = new LevelsMapDataToStor();
    private LevelsMapDataHandler levelsMapDataHandler;
    private int currentLevelID;
    private int receivedStars;


    public void SetLevelsMapDataHandler(LevelsMapDataHandler levelsMapDataHandler)
    {
        this.levelsMapDataHandler = levelsMapDataHandler;

        Debug.Log(Application.persistentDataPath);
    }

    public void Save()
    {
        storageService.JsonSaver.Save(ACHIEVED_LEVELS_DATA_KEY, levelsMapDataToStor, OnSaved);
    }

    public void Load()
    {
        try
        {
            storageService.JsonSaver.Load<LevelsMapDataToStor>(ACHIEVED_LEVELS_DATA_KEY, OnLoad);
        }
        catch (Exception e)
        {
            Debug.Log(e);
            Save();
        }  
    }

    private void OnLoad(LevelsMapDataToStor levelsMapDataToStor)
    {
        this.levelsMapDataToStor = levelsMapDataToStor;

        for (int i = 0; i < levelsMapDataToStor.AchievedLevels.Count; i++)
        {
            levelsMapDataToStor.AchievedLevels.TryGetValue(i, out var levelData);         
            
            levelsMapDataHandler.SetSavedLevelsDate(levelData, i);
        }

        levelsMapDataHandler.SetReceivedStars(levelsMapDataToStor.ReceivedStars);
        receivedStars = levelsMapDataToStor.ReceivedStars;
    }

    public void SaveLevelAchievements(bool isLevelAchieved, int starCount)
    {
        LevelDataToStor dataToStore = new LevelDataToStor { LevelID = currentLevelID, IsLevelAchieved = isLevelAchieved, StarCount = starCount, IsStarReceived = false };

        var currentlevelDataToStore = levelsMapDataToStor.AchievedLevels[currentLevelID - 1];
        currentlevelDataToStore.IsLevelAchieved = true;

        if (!currentlevelDataToStore.IsStarReceived)
        {
            levelsMapDataToStor.AchievedLevels[currentLevelID - 1].IsStarReceived = true;
            levelsMapDataToStor.ReceivedStars += dataToStore.StarCount;
            receivedStars = levelsMapDataToStor.ReceivedStars;
        }
        else if(currentlevelDataToStore.StarCount < dataToStore.StarCount)
        {
            int additionalReachedStar = dataToStore.StarCount - currentlevelDataToStore.StarCount;

            levelsMapDataToStor.ReceivedStars += additionalReachedStar;

            receivedStars = levelsMapDataToStor.ReceivedStars;
        }

        if (currentlevelDataToStore.StarCount < dataToStore.StarCount)
        {
            currentlevelDataToStore.StarCount = dataToStore.StarCount;
        }

        if (levelsMapDataToStor.AchievedLevels.ContainsKey(currentLevelID) && currentlevelDataToStore.StarCount >= 1)
        {
            levelsMapDataToStor.AchievedLevels[currentLevelID].IsLevelAchieved = true;
        }

        levelsMapDataToStor.AchievedLevels[currentLevelID - 1] = currentlevelDataToStore;

        Save();
    }

    private void OnSaved(bool isSaved)
    {
        Debug.Log("LevelMapData_isSaved " + isSaved);
    }

    public void SetCurrentSelectedLevel(int currentLevelID)
    {
        this.currentLevelID = currentLevelID;
    }
}
