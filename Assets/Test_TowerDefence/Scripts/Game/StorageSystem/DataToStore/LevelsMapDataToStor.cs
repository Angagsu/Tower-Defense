using Newtonsoft.Json;
using System.Collections.Generic;

public class LevelsMapDataToStor
{
    public int ReceivedStars;

    [JsonProperty("lvls")]
    public Dictionary<int, LevelDataToStor> AchievedLevels = new Dictionary<int, LevelDataToStor>();

    public LevelsMapDataToStor()
    {
        for (int i = 0; i < 85; i++)
        {
            AchievedLevels.Add(i, new LevelDataToStor());
        }
    }
}
