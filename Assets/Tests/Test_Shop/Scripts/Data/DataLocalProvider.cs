using Newtonsoft.Json;
using System.IO;
using UnityEngine;

public class DataLocalProvider : IDataProvider
{
    private const string FileName = "PlayerSave";
    private const string SaveFileExtension = ".json";

    private IPersistentData persistentData;

    public DataLocalProvider(IPersistentData persistentData) => this.persistentData = persistentData;

    private string SavePath => Application.persistentDataPath;
    private string FullPath => Path.Combine(SavePath, $"{FileName}{SaveFileExtension}");

    public bool TryLoad()
    {
        if (IsDataAlreadyExist() == false)
        {
            return false;
        }

        persistentData.PlayerData = JsonConvert.DeserializeObject<PlayerData>(File.ReadAllText(FullPath));
        return true;
    }

    public void Save()
    {
        File.WriteAllText(FullPath, JsonConvert.SerializeObject(persistentData.PlayerData, Formatting.Indented, new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        }));
    }

    private bool IsDataAlreadyExist() => File.Exists(FullPath);
}
