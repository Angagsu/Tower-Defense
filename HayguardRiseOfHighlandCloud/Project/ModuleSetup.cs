using Unity.Services.CloudCode.Apis.Extensions;
using Unity.Services.CloudCode.Core;
using Microsoft.Extensions.DependencyInjection;


namespace HayguardRiseOfHighlandCloud;

public class ModuleSetup : ICloudCodeSetup
{
    public void Setup(ICloudCodeConfig config)
    {
        //config.Dependencies.AddSingleton(GameApiClient.Create());
        config.AddGameApiClient(); 
        
        config.Dependencies.AddSingleton<PlayerEconomyService>();
        config.Dependencies.AddSingleton<PlayerDataService>();
        //config.Dependencies.AddSingleton<AdRewardsService>();
    }
}
