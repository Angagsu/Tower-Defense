using System;
using System.Collections.Generic;


[Serializable]
public class VideosDataToStore 
{
    public List<VideoEntry> videos = new();

    public VideosDataToStore()
    {
        videos.Add(new VideoEntry { name = "LevelMapCBR4K.mp4", url = "https://pub-4f027770d64d489bbf0f2da17068f7a0.r2.dev/LevelMapCBR4K.mp4" });
        videos.Add(new VideoEntry { name = "LevelMapCBR12_4K.mp4", url = "https://pub-4f027770d64d489bbf0f2da17068f7a0.r2.dev/LevelMapCBR12_4K.mp4" });
        videos.Add(new VideoEntry { name = "LevelMapVBR12_4K.mp4", url = "https://pub-4f027770d64d489bbf0f2da17068f7a0.r2.dev/LevelMapVBR12_4K.mp4" });
    }
}
