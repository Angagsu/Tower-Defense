//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using System.Security.Cryptography;
//using System.Text;
//using UnityEngine;
//using UnityEngine.Networking;
//using UnityEngine.Video;

////[Serializable]
////public class VideoEntry
////{
////    public string name;
////    public string version;
////    public string url;
////    public string checksum; // optional, format "sha256:abcdef..."
////}

////[Serializable]
////public class VersionManifest
////{
////    public List<VideoEntry> videos;
////    public string updated_at;
////}

////[Serializable]
////public class LocalManifest
////{
////    public List<VideoEntry> videos = new List<VideoEntry>();
////    public Dictionary<string, string> GetVersionMap()
////    {
////        var d = new Dictionary<string, string>();
////        foreach (var v in videos) d[v.name] = v.version;
////        return d;
////    }
////}

//public class VideoManager : MonoBehaviour
//{
//    public static VideoManager Instance { get; private set; }

//    [Header("Server")]
//    [Tooltip("Direct URL to version.json (Cloudflare R2 or any HTTPS).")]
//    public string versionJsonUrl;

//    [Header("Settings")]
//    public int maxConcurrentDownloads = 4;
//    public int maxRetries = 3;
//    public int retryBaseDelayMs = 800; // exponential backoff base
//    public int requestTimeoutSeconds = 60;

//    [Header("Paths")]
//    public string videosFolderName = "videos"; // inside persistentDataPath

//    [Header("References")]
//    public VideoPlayer videoPlayer; // optional, for PlayVideo

//    // internal
//    private string localFolder;
//    private string localManifestPath;
//    public LocalManifest localManifest = new LocalManifest();

//    private Queue<VideoEntry> downloadQueue = new Queue<VideoEntry>();
//    private int activeWorkers = 0;
//    private bool isUpdating = false;

//    // events
//    public event Action<string, float> OnDownloadProgress; // name, 0..1
//    public event Action<string> OnDownloadComplete;
//    public event Action<string, string> OnError; // name, error



//    void Awake()
//    {
//        //if (Instance != null && Instance != this) { Destroy(gameObject); return; }
//        Instance = this;
//        //DontDestroyOnLoad(gameObject);

//        localFolder = Path.Combine(Application.persistentDataPath, videosFolderName);
//        if (!Directory.Exists(localFolder)) Directory.CreateDirectory(localFolder);
//        localManifestPath = Path.Combine(localFolder, "local_manifest.json");
//        LoadLocalManifest();
//    }

//    void Start()
//    {
//        StartCoroutine(CheckAndUpdateAllCoroutine());
//    }

//    #region Manifest IO
//    private void LoadLocalManifest()
//    {
//        if (File.Exists(localManifestPath))
//        {
//            try
//            {
//                string txt = File.ReadAllText(localManifestPath);
//                localManifest = JsonUtility.FromJson<LocalManifest>(txt) ?? new LocalManifest();
//            }
//            catch (Exception e)
//            {
//                Debug.LogWarning("Failed to read local manifest: " + e.Message);
//                localManifest = new LocalManifest();
//            }
//        }
//        else localManifest = new LocalManifest();
//    }

//    private void SaveLocalManifest()
//    {
//        try
//        {
//            string txt = JsonUtility.ToJson(localManifest, true);
//            File.WriteAllText(localManifestPath, txt);
//        }
//        catch (Exception e)
//        {
//            Debug.LogError("Failed to save local manifest: " + e.Message);
//        }
//    }
//    #endregion

//    #region Update Flow
//    public IEnumerator CheckAndUpdateAllCoroutine()
//    {
//        if (isUpdating) yield break;
//        isUpdating = true;

//        // 1) download version.json
//        UnityWebRequest req = UnityWebRequest.Get(versionJsonUrl);
//        req.timeout = requestTimeoutSeconds;
//        yield return req.SendWebRequest();

//        if (req.result != UnityWebRequest.Result.Success)
//        {
//            Debug.LogError("Version.json download failed: " + req.error);
//            OnError?.Invoke("version", req.error);
//            isUpdating = false;
//            yield break;
//        }

//        VersionManifest serverManifest;
//        try
//        {
//            serverManifest = JsonUtility.FromJson<VersionManifest>(req.downloadHandler.text);
//        }
//        catch (Exception e)
//        {
//            Debug.LogError("Failed parse version.json: " + e.Message);
//            OnError?.Invoke("version", e.Message);
//            isUpdating = false;
//            yield break;
//        }

//        if (serverManifest == null || serverManifest.videos == null)
//        {
//            Debug.LogError("Empty version manifest");
//            isUpdating = false;
//            yield break;
//        }

//        // Build server map & decide what to download
//        var serverMap = new Dictionary<string, VideoEntry>();
//        foreach (var v in serverManifest.videos)
//        {
//            serverMap[v.name] = v;
//        }

//        var localVersions = localManifest.GetVersionMap();
//        var toDownload = new List<VideoEntry>();

//        foreach (var sv in serverManifest.videos)
//        {
//            bool need = true;
//            if (localVersions.TryGetValue(sv.name, out string localVer))
//            {
//                if (localVer == sv.version)
//                {
//                    // file exists? check existence
//                    string path = GetLocalFilePath(sv);
//                    if (File.Exists(path))
//                    {
//                        need = false;
//                    }
//                }
//            }
//            if (need) toDownload.Add(sv);
//        }

//        // Enqueue downloads
//        downloadQueue.Clear();
//        foreach (var d in toDownload) downloadQueue.Enqueue(d);

//        // Start workers
//        int workers = Mathf.Clamp(maxConcurrentDownloads, 1, 16);
//        activeWorkers = 0;
//        for (int i = 0; i < workers; i++)
//        {
//            StartCoroutine(DownloadWorkerCoroutine());
//        }

//        // Wait for queue to complete
//        while (downloadQueue.Count > 0 || activeWorkers > 0)
//            yield return null;

//        // After downloads, cleanup: remove local files not in server manifest
//        CleanupObsoleteFiles(serverMap);

//        // Save local manifest (replace entries with server entries for ones that exist locally)
//        UpdateLocalManifestFromServer(serverManifest);

//        SaveLocalManifest();

//        isUpdating = false;
//        yield break;
//    }

//    private IEnumerator DownloadWorkerCoroutine()
//    {
//        activeWorkers++;
//        while (true)
//        {
//            VideoEntry entry = null;
//            lock (downloadQueue)
//            {
//                if (downloadQueue.Count > 0) entry = downloadQueue.Dequeue();
//            }
//            if (entry == null) break;

//            // download with retries
//            int attempt = 0;
//            bool success = false;
//            string lastError = null;
//            while (attempt <= maxRetries && !success)
//            {
//                attempt++;
//                var download = DownloadFileCoroutine(entry);
//                yield return StartCoroutine(download);

//                // check if file exists & checksum ok
//                string localPath = GetLocalFilePath(entry);
//                if (File.Exists(localPath))
//                {
//                    if (string.IsNullOrEmpty(entry.checksum) || VerifyChecksum(localPath, entry.checksum))
//                    {
//                        success = true;
//                        break;
//                    }
//                    else
//                    {
//                        lastError = "checksum_mismatch";
//                        Debug.LogWarning($"Checksum mismatch for {entry.name}, attempt {attempt}");
//                        // delete bad file and retry
//                        try { File.Delete(localPath); } catch { }
//                    }
//                }
//                else
//                {
//                    lastError = "download_failed";
//                }

//                // exponential backoff
//                int delay = retryBaseDelayMs * (1 << (attempt - 1));
//                yield return new WaitForSeconds(delay / 1000f);
//            }

//            if (!success)
//            {
//                Debug.LogError($"Failed to download {entry.name} after {maxRetries} attempts. Last error: {lastError}");
//                OnError?.Invoke(entry.name, lastError ?? "unknown");
//            }
//            else
//            {
//                // mark local manifest entry
//                ReplaceLocalManifestEntry(entry);
//                OnDownloadComplete?.Invoke(entry.name);
//            }
//        }
//        activeWorkers--;
//    }

//    private IEnumerator DownloadFileCoroutine(VideoEntry entry)
//    {
//        string finalPath = GetLocalFilePath(entry);
//        string dir = Path.GetDirectoryName(finalPath);
//        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

//        string partPath = finalPath + ".part";
//        long existingLen = 0;
//        if (File.Exists(partPath))
//        {
//            existingLen = new FileInfo(partPath).Length;
//        }

//        // Create request
//        UnityWebRequest request = UnityWebRequest.Get(entry.url);
//        request.timeout = requestTimeoutSeconds;

//        // DownloadHandlerFile with append = true if we have partial file
//        try
//        {
//            var dh = new DownloadHandlerFile(partPath, existingLen > 0);
//            request.downloadHandler = dh;
//        }
//        catch (Exception)
//        {
//            // fallback to buffer + write manually
//            request.downloadHandler = new DownloadHandlerBuffer();
//        }

//        // If we have partial, set Range header
//        if (existingLen > 0)
//        {
//            request.SetRequestHeader("Range", $"bytes={existingLen}-");
//        }

//        // Start
//        var op = request.SendWebRequest();
//        float lastProgress = 0f;

//        // Track progress and invoke event (name-based)
//        while (!op.isDone)
//        {
//            float progress = existingLen > 0 ? Mathf.Clamp01(existingLen / (float)Math.Max(1, (long)request.downloadedBytes + existingLen)) : request.downloadProgress;
//            // only fire if meaningful
//            if (Mathf.Abs(progress - lastProgress) > 0.01f)
//            {
//                OnDownloadProgress?.Invoke(entry.name, progress);
//                lastProgress = progress;
//            }
//            yield return null;
//        }

//        // finalize
//        if (request.result == UnityWebRequest.Result.Success || request.responseCode == 206 || request.responseCode == 200)
//        {
//            // If DownloadHandlerBuffer used, write bytes
//            if (request.downloadHandler is DownloadHandlerBuffer)
//            {
//                var b = request.downloadHandler.data;
//                File.WriteAllBytes(partPath, b);
//            }

//            // Move .part -> final (atomic on most OS)
//            try
//            {
//                if (File.Exists(finalPath)) File.Delete(finalPath);
//                File.Move(partPath, finalPath);
//            }
//            catch (Exception e)
//            {
//                Debug.LogError("Failed to move part to final: " + e.Message);
//                OnError?.Invoke(entry.name, "file_move_failed");
//                yield break;
//            }

//            OnDownloadProgress?.Invoke(entry.name, 1f);
//        }
//        else
//        {
//            Debug.LogWarning($"Download request failed for {entry.name}: {request.error} (code {request.responseCode})");
//            OnError?.Invoke(entry.name, request.error);
//        }

//        yield break;
//    }
//    #endregion

//    #region Helpers
//    private string GetLocalFilePath(VideoEntry entry)
//    {
//        // store as persistent/videos/<name>_<version>.ext OR keep original filename
//        Uri u;
//        string ext = ".mp4";
//        try
//        {
//            u = new Uri(entry.url);
//            string last = Path.GetFileName(u.LocalPath);
//            if (!string.IsNullOrEmpty(last) && last.IndexOf('.') >= 0) ext = Path.GetExtension(last);
//        }
//        catch { /* fallback */ }
//        string fileName = $"{entry.name}_{entry.version}{ext}";
//        return Path.Combine(localFolder, fileName);
//    }

//    private void ReplaceLocalManifestEntry(VideoEntry e)
//    {
//        // remove old entries with same name
//        localManifest.videos.RemoveAll(x => x.name == e.name);
//        // add new
//        var copy = new VideoEntry() { name = e.name, version = e.version, url = e.url, checksum = e.checksum };
//        localManifest.videos.Add(copy);
//        SaveLocalManifest();
//    }

//    private void UpdateLocalManifestFromServer(VersionManifest server)
//    {
//        // For each server entry, if local file exists, ensure localManifest entry is set to server version
//        foreach (var sv in server.videos)
//        {
//            string path = GetLocalFilePath(sv);
//            if (File.Exists(path))
//            {
//                ReplaceLocalManifestEntry(sv);
//            }
//        }
//    }

//    private void CleanupObsoleteFiles(Dictionary<string, VideoEntry> serverMap)
//    {
//        // Delete any local file that does not correspond to server manifest (by name)
//        // Our naming rule is name_version.ext
//        var files = Directory.GetFiles(localFolder, "*", SearchOption.TopDirectoryOnly);
//        foreach (var f in files)
//        {
//            string fname = Path.GetFileName(f);
//            bool keep = false;
//            foreach (var kv in serverMap)
//            {
//                // match prefix "name_"
//                if (fname.StartsWith(kv.Key + "_")) { keep = true; break; }
//            }
//            if (!keep && fname != "local_manifest.json")
//            {
//                try { File.Delete(f); } catch (Exception ex) { Debug.LogWarning("Cleanup failed: " + ex.Message); }
//            }
//        }

//        // Also cleanup local manifest entries that no longer exist on server
//        localManifest.videos.RemoveAll(x => !serverMap.ContainsKey(x.name));
//        SaveLocalManifest();
//    }

//    private bool VerifyChecksum(string localPath, string checksumField)
//    {
//        if (string.IsNullOrEmpty(checksumField)) return true;
//        try
//        {
//            // expected format "sha256:abcdef..."
//            string[] parts = checksumField.Split(':');
//            if (parts.Length != 2) return true;
//            string alg = parts[0].ToLower();
//            string expected = parts[1].ToLower();

//            if (alg == "sha256")
//            {
//                using (var stream = File.OpenRead(localPath))
//                using (var sha = SHA256.Create())
//                {
//                    var hash = sha.ComputeHash(stream);
//                    var hex = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
//                    return hex == expected;
//                }
//            }
//            else
//            {
//                Debug.LogWarning("Checksum algorithm not supported: " + alg);
//                return true; // don't fail only for unsupported alg
//            }
//        }
//        catch (Exception e)
//        {
//            Debug.LogWarning("Checksum verify error: " + e.Message);
//            return false;
//        }
//    }
//    #endregion

//    #region Play / API
//    public bool HasLocalVideo(string name)
//    {
//        foreach (var v in localManifest.videos)
//            if (v.name == name)
//                return File.Exists(GetLocalFilePath(v));
//        return false;
//    }

//    public string GetLocalVideoPath(string name)
//    {
//        foreach (var v in localManifest.videos)
//            if (v.name == name)
//                return GetLocalFilePath(v);
//        return null;
//    }

//    public void PlayVideo(string name, bool fallbackToStream = true)
//    {
//        string local = GetLocalVideoPath(name);
//        if (!string.IsNullOrEmpty(local) && File.Exists(local))
//        {
//            if (videoPlayer != null)
//            {
//                videoPlayer.url = local;
//                videoPlayer.Play();
//            }
//            else
//            {
//                Debug.Log("Local video ready at: " + local);
//            }
//            return;
//        }

//        // fallback: stream from url in local manifest (if exists)
//        var it = localManifest.videos.Find(x => x.name == name);
//        if (it != null && fallbackToStream)
//        {
//            if (videoPlayer != null)
//            {
//                videoPlayer.url = it.url;
//                videoPlayer.Play();
//            }
//        }
//        else
//        {
//            Debug.LogWarning("Video not found locally or remotely: " + name);
//        }
//    }
//    #endregion
//}

