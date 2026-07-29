using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HayguardRiseOfHighlandCloud
{
    public class PlayerData
    {
        [JsonProperty("displayName")]
        public string? DisplayName { get; set; }

        [JsonProperty("experience")]
        public int Experience { get; set; }
    }

    public class PlayerEconomyData
    {
        [JsonProperty("currencies")]
        public Dictionary<string, int> Currencies { get; set; } = new Dictionary<string, int>();

        [JsonProperty("itemInventory")]
        public Dictionary<string, int> ItemInventory { get; set; } = new Dictionary<string, int>();
    }

    public class PlayerDataResponse
    {
        [JsonProperty("playerData")]
        public PlayerData PlayerData { get; set; } = new PlayerData();
        [JsonProperty("economyData")]
        public PlayerEconomyData EconomyData { get; set; } = new PlayerEconomyData();

        [JsonProperty("isNewPlayer")]
        public bool IsNewPlayer { get; set; }
    }
}
