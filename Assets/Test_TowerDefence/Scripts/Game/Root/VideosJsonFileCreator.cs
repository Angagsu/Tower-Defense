using System;
using UnityEngine;

public class VideosJsonFileCreator : MonoBehaviour
{
    
    private VideosDataToStore videosDataToStore = new VideosDataToStore();

    private const string VIDEOS_DATA_KEY = "videos";
    private StorageService storageService;

    [Inject]
    public void Construct(StorageService storageService)
    {
        this.storageService = storageService;
    }

    [ContextMenu("Save")]
    public void Save()
    {
        storageService.JsonSaver.Save(VIDEOS_DATA_KEY, videosDataToStore, OnSave);
    }

    private void OnSave(bool obj)
    {
        Debug.Log(obj);
    }
}
