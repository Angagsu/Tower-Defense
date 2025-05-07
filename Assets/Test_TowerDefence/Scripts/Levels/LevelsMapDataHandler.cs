using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;


public class LevelsMapDataHandler : MonoBehaviour, IService
{
    private const string ACHIEVED_LEVELS_DATA_KEY = "AchievedLevelsData.Json";


    [SerializeField] private StorageService storageService;


    private LevelsMapDataToStor levelsMapDataToStor = new LevelsMapDataToStor();
    private LevelsMapController levelsMap;
    private int currentLevelID;



    public void SetLevelsMapController(LevelsMapController levelsMap)
    {
        this.levelsMap = levelsMap;
        
        SetLevels();

        Debug.Log(Application.persistentDataPath);
    }

    public void SetLevels()
    {
        foreach (LevelsMapLevel levelsMapLevel in levelsMap.Levels)
        {
            if (!levelsMapDataToStor.achievedLevels.ContainsKey(levelsMapLevel.LevelDataToStor.LevelID))
            {
                levelsMapDataToStor.achievedLevels.Add(levelsMapLevel.LevelDataToStor.LevelID, levelsMapLevel.LevelDataToStor);
            }
        }
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
        Debug.Log("Loaded");

        for (int i = 0; i < levelsMapDataToStor.achievedLevels.Count; i++)
        {
            levelsMapDataToStor.achievedLevels.TryGetValue(i + 1, out var levelData);         

            levelsMap.SetSavedLevelsDate(levelData, i);
        }
    }

    public void SaveLevelAchievements(bool isLevelAchieved, int starCount)
    {
        LevelDataToStor dataToStore = new LevelDataToStor { LevelID = currentLevelID, IsLevelAchieved = isLevelAchieved, StarCount = starCount };

        var currentlevelDataToStore = levelsMapDataToStor.achievedLevels[currentLevelID];

        if (currentlevelDataToStore.StarCount < dataToStore.StarCount)
        {
            currentlevelDataToStore.StarCount = dataToStore.StarCount;
        }

        if (levelsMapDataToStor.achievedLevels.ContainsKey(currentLevelID + 1) && currentlevelDataToStore.StarCount >= 1)
        {
            levelsMapDataToStor.achievedLevels[currentLevelID + 1].IsLevelAchieved = true;
        }

        Save();
    }

    private void OnSaved(bool isSaved)
    {
        Debug.Log("isSaved" + isSaved);
    }

    public void SetCurrentSelectedLevel(int currentLevelID)
    {
        this.currentLevelID = currentLevelID;
    }
}


public class LevelsMapDataToStor
{
    [JsonProperty("lvls")]
    public Dictionary<int, LevelDataToStor> achievedLevels = new();
}
