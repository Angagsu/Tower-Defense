using Newtonsoft.Json;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class JsonToFileStorageHandler : IStorageService
{
    private string encryptionKey = "1234567890ABCDEFGHIJKLMNOPQRSTUV";
    private bool useEncryption = false;
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


        var savedData = useEncryption ? Encrypt(json, encryptionKey) : json; 


        using (var fileStream = new StreamWriter(path))
        {
            await fileStream.WriteAsync(savedData);
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

            var loadedData = useEncryption ? Decrypt(json, encryptionKey) : json;

            var data = JsonConvert.DeserializeObject<T>(loadedData);

            callback?.Invoke(data);
        }
    }

    private string BuildPath(string key)
    {
        return Path.Combine(Application.persistentDataPath, key);
    }

    private string Encrypt(string json, string key)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = GetKey(key);
            aesAlg.Mode = CipherMode.CBC;
            aesAlg.Padding = PaddingMode.PKCS7;
            aesAlg.GenerateIV();

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);

                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(json);
                    }
                }

                return Convert.ToBase64String(msEncrypt.ToArray());
            }
        }
    }

    private string Decrypt(string cipherText, string key)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = GetKey(key);
            aesAlg.Mode = CipherMode.CBC;
            aesAlg.Padding = PaddingMode.PKCS7;

            byte[] iv = new byte[16];
            byte[] cipherBytes = Convert.FromBase64String(cipherText);

            Array.Copy(cipherBytes, 0, iv, 0, iv.Length);
            aesAlg.IV = iv;

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msDecrypt = new MemoryStream(cipherBytes, iv.Length, cipherBytes.Length - iv.Length))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
        }
    }

    private byte[] GetKey(string password)
    {
        byte[] keyBytes = Encoding.UTF8.GetBytes(password);
        using (var key = new Rfc2898DeriveBytes(password, keyBytes, 10000))
        {
            return key.GetBytes(32);
        }
    }
}
