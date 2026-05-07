using UnityEngine;

public class StorageService : MonoBehaviour , IService
{
    public IStorageService JsonSaver { get; private set; }


    private void Awake()
    {
        JsonSaver = new JsonToFileStorageHandler();
    }
}




