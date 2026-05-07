using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Video;

[Serializable]
public class VideoEntry
{
    public string name;
    public string url;
}

[Serializable]
public class VersionManifest
{
    public List<VideoEntry> videos;
}

[Serializable]
public class LocalManifest
{
    public List<VideoEntry> LevelMapVideos = new List<VideoEntry>();
    public Dictionary<string, string> GetVersionMap()
    {
        var d = new Dictionary<string, string>();
        foreach (var v in LevelMapVideos) d[v.name] = v.url;
        return d;
    }
}

public class VideoDownloadHandler : MonoBehaviour, IService
{
    [Header("Server")]
    [Tooltip("Direct URL to version.json (Cloudflare R2 or any HTTPS).")]
    [SerializeField] private string versionJsonUrl;

    [Header("Settings")]
    [SerializeField] private int maxConcurrentDownloads = 4;
    [SerializeField] private int maxRetries = 3;
    [SerializeField] private int retryBaseDelayMs = 800; 
    [SerializeField] private int requestTimeoutSeconds = 60;

    [Header("Paths")]
    [SerializeField] private string videosFolderName = "LevelMapVideos"; 

    private string localFolder;
    private string localManifestPath;
    public LocalManifest LocalManifest { get; private set; } = new LocalManifest();

    private Queue<VideoEntry> downloadQueue = new Queue<VideoEntry>();
    private int activeWorkers = 0;
    private bool isUpdating = false;

    public event Action<string, float> OnDownloadProgress; 
    public event Action<string> OnDownloadComplete;
    public event Action<string, string> OnError; 

    private int downloadCountFromServer;
    private const string PLAYER_PREFS_KEY = "PlayerPrefsKey";

    void Awake()
    {
        downloadCountFromServer = PlayerPrefs.GetInt(PLAYER_PREFS_KEY);

        localFolder = Path.Combine(Application.persistentDataPath, videosFolderName);

        if (!Directory.Exists(localFolder)) Directory.CreateDirectory(localFolder);

        localManifestPath = Path.Combine(localFolder, "local_manifest.json");

        LoadLocalManifest();
    }

    void Start()
    {
        //if (downloadCountFromServer == 0)
        //{
        //    downloadCountFromServer++;
        //    PlayerPrefs.SetInt(PLAYER_PREFS_KEY, downloadCountFromServer);
        //    StartCoroutine(CheckAndUpdateAllCoroutine());
        //    Debug.Log("Starting Updating Cproutine" +  downloadCountFromServer);
        //}   
        StartCoroutine(CheckAndUpdateAllCoroutine());
    }

    #region Manifest IO
    private void LoadLocalManifest()
    {
        if (File.Exists(localManifestPath))
        {
            try
            {
                string txt = File.ReadAllText(localManifestPath);
                LocalManifest = JsonUtility.FromJson<LocalManifest>(txt) ?? new LocalManifest();
            }
            catch (Exception e)
            {
                Debug.LogWarning("Failed to read local manifest: " + e.Message);
                LocalManifest = new LocalManifest();
            }
        }
        else
        {
            LocalManifest = new LocalManifest();
        }
    }

    private void SaveLocalManifest()
    {
        try
        {
            string txt = JsonUtility.ToJson(LocalManifest, true);
            File.WriteAllText(localManifestPath, txt);
            Debug.Log(txt);
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to save local manifest: " + e.Message);
        }
    }
    #endregion

    #region Update Flow
    public IEnumerator CheckAndUpdateAllCoroutine()
    {
        if (isUpdating) yield break;
        isUpdating = true;

        
        UnityWebRequest req = UnityWebRequest.Get(versionJsonUrl);
        req.timeout = requestTimeoutSeconds;
        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Version.json download failed: " + req.error);
            OnError?.Invoke("version", req.error);
            isUpdating = false;
            yield break;
        }

        VersionManifest serverManifest;
        try
        {
            serverManifest = JsonUtility.FromJson<VersionManifest>(req.downloadHandler.text);
        }
        catch (Exception e)
        {
            Debug.LogError("Failed parse version.json: " + e.Message);
            OnError?.Invoke("version", e.Message);
            isUpdating = false;
            yield break;
        }

        if (serverManifest == null || serverManifest.videos == null)
        {
            Debug.LogError("Empty version manifest");
            isUpdating = false;
            yield break;
        }

        // Build server map & decide what to download
        var serverMap = new Dictionary<string, VideoEntry>();
        foreach (var v in serverManifest.videos)
        {
            serverMap[v.name] = v;
        }

        var localVersions = LocalManifest.GetVersionMap();
        var toDownload = new List<VideoEntry>();

        foreach (var sv in serverManifest.videos)
        {
            bool need = true;
            if (localVersions.TryGetValue(sv.name, out string localVer))
            {
                if (localVer == sv.url)
                {
                    // file exists? check existence
                    string path = GetLocalFilePath(sv);
                    if (File.Exists(path))
                    {
                        need = false;
                    }
                }
            }
            if (need)
            {
                toDownload.Add(sv);
                Debug.Log("Need");
            }
        }

        // Enqueue downloads
        downloadQueue.Clear();
        foreach (var d in toDownload) downloadQueue.Enqueue(d);

        // Start workers
        int workers = Mathf.Clamp(maxConcurrentDownloads, 1, 16);
        activeWorkers = 0;
        for (int i = 0; i < workers; i++)
        {
            StartCoroutine(DownloadWorkerCoroutine());
        }

        // Wait for queue to complete
        while (downloadQueue.Count > 0 || activeWorkers > 0)
            yield return null;

        // After downloads, cleanup: remove local files not in server manifest
       // CleanupObsoleteFiles(serverMap);
       //
       // // Save local manifest (replace entries with server entries for ones that exist locally)
       // UpdateLocalManifestFromServer(serverManifest);
       //
        SaveLocalManifest();

        isUpdating = false;
        yield break;
    }

    private IEnumerator DownloadWorkerCoroutine()
    {
        activeWorkers++;
        while (true)
        {
            VideoEntry entry = null;
            lock (downloadQueue)
            {
                if (downloadQueue.Count > 0) entry = downloadQueue.Dequeue();
            }
            if (entry == null) break;

            // download with retries
            int attempt = 0;
            bool success = false;
            string lastError = null;
            while (attempt <= maxRetries && !success)
            {
                attempt++;
                var download = DownloadFileCoroutine(entry);
                yield return StartCoroutine(download);

                // check if file exists & checksum ok
                string localPath = GetLocalFilePath(entry);
                if (File.Exists(localPath))
                {
                    success = true;
                    break;
                }
                else
                {
                    lastError = "download_failed";
                }

                // exponential backoff
                int delay = retryBaseDelayMs * (1 << (attempt - 1));
                yield return new WaitForSeconds(delay / 1000f);
            }

            if (!success)
            {
                Debug.LogError($"Failed to download {entry.name} after {maxRetries} attempts. Last error: {lastError}");
                OnError?.Invoke(entry.name, lastError ?? "unknown");
            }
            else
            {
                // mark local manifest entry
                ReplaceLocalManifestEntry(entry);
                OnDownloadComplete?.Invoke(entry.name);
            }
        }
        activeWorkers--;
    }

    private IEnumerator DownloadFileCoroutine(VideoEntry entry)
    {
        string finalPath = GetLocalFilePath(entry);
        string dir = Path.GetDirectoryName(finalPath);
        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

        string partPath = finalPath + ".part";
        long existingLen = 0;

        if (File.Exists(partPath))
        {
            existingLen = new FileInfo(partPath).Length;
        }

        // Create request
        UnityWebRequest request = UnityWebRequest.Get(entry.url);
        request.timeout = requestTimeoutSeconds;

        // DownloadHandlerFile with append = true if we have partial file
        try
        {
            var dh = new DownloadHandlerFile(partPath, existingLen > 0);
            request.downloadHandler = dh;
        }
        catch (Exception)
        {
            // fallback to buffer + write manually
            request.downloadHandler = new DownloadHandlerBuffer();
        }

        // If we have partial, set Range header
        if (existingLen > 0)
        {
            request.SetRequestHeader("Range", $"bytes={existingLen}-");
        }

        // Start
        var op = request.SendWebRequest();
        float lastProgress = 0f;

        // Track progress and invoke event (name-based)
        while (!op.isDone)
        {
            float progress = existingLen > 0 ? Mathf.Clamp01(existingLen / (float)Math.Max(1, (long)request.downloadedBytes + existingLen)) : request.downloadProgress;
            // only fire if meaningful
            if (Mathf.Abs(progress - lastProgress) > 0.01f)
            {
                OnDownloadProgress?.Invoke(entry.name, progress);
                lastProgress = progress;
            }
            yield return null;
        }

        // finalize
        if (request.result == UnityWebRequest.Result.Success || request.responseCode == 206 || request.responseCode == 200)
        {
            // If DownloadHandlerBuffer used, write bytes
            if (request.downloadHandler is DownloadHandlerBuffer)
            {
                var b = request.downloadHandler.data;
                File.WriteAllBytes(partPath, b);
            }

            // Move .part -> final (atomic on most OS)
            try
            {
                if (File.Exists(finalPath)) File.Delete(finalPath);
                File.Move(partPath, finalPath);
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to move part to final: " + e.Message);
                OnError?.Invoke(entry.name, "file_move_failed");
                yield break;
            }

            OnDownloadProgress?.Invoke(entry.name, 1f);
        }
        else
        {
            Debug.LogWarning($"Download request failed for {entry.name}: {request.error} (code {request.responseCode})");
            OnError?.Invoke(entry.name, request.error);
        }

        yield break;
    }
    #endregion

    #region Helpers
    private string GetLocalFilePath(VideoEntry entry)
    {
        // store as persistent/videos/<name>_<version>.ext OR keep original filename
        Uri u;
        string ext = ".mp4";
        try
        {
            u = new Uri(entry.url);
            string last = Path.GetFileName(u.LocalPath);
            if (!string.IsNullOrEmpty(last) && last.IndexOf('.') >= 0) ext = Path.GetExtension(last);
        }
        catch { /* fallback */ }
        string fileName = $"{entry.name}";
        return Path.Combine(localFolder, fileName);
    }

    private void ReplaceLocalManifestEntry(VideoEntry e)
    {
        LocalManifest.LevelMapVideos.RemoveAll(x => x.name == e.name);

        var copy = new VideoEntry() { name = e.name, url = e.url };

        LocalManifest.LevelMapVideos.Add(copy);

       // SaveLocalManifest();
    }

    private void UpdateLocalManifestFromServer(VersionManifest server)
    {
        foreach (var sv in server.videos)
        {
            string path = GetLocalFilePath(sv);
            if (File.Exists(path))
            {
                ReplaceLocalManifestEntry(sv);
            }
        }
    }

    private void CleanupObsoleteFiles(Dictionary<string, VideoEntry> serverMap)
    {
        // Delete any local file that does not correspond to server manifest (by name)

        var files = Directory.GetFiles(localFolder, "*", SearchOption.TopDirectoryOnly);

        foreach (var f in files)
        {
            string fname = Path.GetFileName(f);
            bool keep = false;
            foreach (var kv in serverMap)
            {
                // match prefix "name_"
                if (fname.StartsWith(kv.Key))
                { 
                    keep = true;
                    break; 
                }
            }
            if (!keep && fname != "local_manifest.json")
            {
                try { File.Delete(f); } catch (Exception ex) { Debug.LogWarning("Cleanup failed: " + ex.Message); }
            }
        }

        // Also cleanup local manifest entries that no longer exist on server
        LocalManifest.LevelMapVideos.RemoveAll(x => !serverMap.ContainsKey(x.name));

       // SaveLocalManifest();
    }

    private bool VerifyChecksum(string localPath, string checksumField)
    {
        if (string.IsNullOrEmpty(checksumField)) return true;
        try
        {
            // expected format "sha256:abcdef..."
            string[] parts = checksumField.Split(':');
            if (parts.Length != 2) return true;
            string alg = parts[0].ToLower();
            string expected = parts[1].ToLower();

            if (alg == "sha256")
            {
                using (var stream = File.OpenRead(localPath))
                using (var sha = SHA256.Create())
                {
                    var hash = sha.ComputeHash(stream);
                    var hex = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                    return hex == expected;
                }
            }
            else
            {
                Debug.LogWarning("Checksum algorithm not supported: " + alg);
                return true; // don't fail only for unsupported alg
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning("Checksum verify error: " + e.Message);
            return false;
        }
    }
    #endregion

    #region Play / API
    public bool HasLocalVideo(string name)
    {
        foreach (var v in LocalManifest.LevelMapVideos)
            if (v.name == name)
                return File.Exists(GetLocalFilePath(v));
        return false;
    }

    public string GetLocalVideoPath(string name)
    {
        foreach (var v in LocalManifest.LevelMapVideos)
            if (v.name == name)
                return GetLocalFilePath(v);
        return null;
    }

    #endregion
}
