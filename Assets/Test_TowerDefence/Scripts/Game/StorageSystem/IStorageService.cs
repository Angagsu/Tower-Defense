using System;

public interface IStorageService 
{
    void Save<T>(string key, T data, Action<bool> callback = null);
    void Load<T>(string key, Action<T> callback);
}
