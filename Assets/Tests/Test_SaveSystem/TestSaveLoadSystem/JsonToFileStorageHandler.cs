using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;

public class JsonToFileStorageHandler : IStorageService
{
    private bool isInProgress;

    public void Save<T>(string key, T data, Action<bool> callback = null)
    {
        if (!isInProgress)
        {
            SaveAsync(key, data, callback);
        }
        else
        {
            callback?.Invoke(false);
        }   
    }

    private async void SaveAsync(string key, object data, Action<bool> callback)
    {
        string path = BuildPath(key);

        string json = JsonConvert.SerializeObject(data, Formatting.Indented, new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });

        using (var fileStream = new StreamWriter(path))
        {
            await fileStream.WriteAsync(json);
        }

        if (File.Exists(path))
        {
            callback?.Invoke(true);
        }
        else
        {
            callback?.Invoke(false);
        }
    }

    public void Load<T>(string key, Action<T> callback)
    {
        string path = BuildPath(key);

        using (var fileStream = new StreamReader(path))
        {
            var json = fileStream.ReadToEnd();
            var data = JsonConvert.DeserializeObject<T>(json);

            callback?.Invoke(data);
        }
    }

    private string BuildPath(string key)
    {
        return Path.Combine(Application.persistentDataPath, key);
    }
}
