using Assets.Scripts.Tower;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;


public class VideoPlaylist : MonoBehaviour
{
    public List<VideoPlayer> videoPlayers;
    public List<string> paths;

    //private VideoManager videoManager;
    private VideoDownloadHandler videoDownloadHandler;

    [Inject]
    public void Construct(VideoDownloadHandler videoDownloadHandler)
    {
        this.videoDownloadHandler = videoDownloadHandler;
    }

    private void Start()
    {

        //videoManager = VideoManager.Instance;

        for (int i = 0; i < videoPlayers.Count; i++)
        {
            //videoPlayers[i].url = paths[i];
            videoPlayers[i].url = videoDownloadHandler.LocalManifest.LevelMapVideos[i].url;
        }
        for (int i = 0; i < videoPlayers.Count; i++)
        {
            videoPlayers[i].prepareCompleted += VideoPlaylist_prepareCompleted;
            videoPlayers[i].Prepare();
            //videoPlayers[i].Play();
        }
        
    }

    private void VideoPlaylist_prepareCompleted(VideoPlayer source)
    {
        source.prepareCompleted -= VideoPlaylist_prepareCompleted;
        source.Play();
    }


}
