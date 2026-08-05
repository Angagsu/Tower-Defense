using Newtonsoft.Json;
using System;
using Unity.Services.Authentication;
using Unity.Services.CloudCode;
using Unity.Services.CloudCode.GeneratedBindings;
using Unity.Services.LevelPlay;
using UnityEngine;

public class RewardedAdsManager : MonoBehaviour
{
    public event Action<bool> AdSuccessfullyCompleted;
    public event Action<bool> AdAvailable;

    private const string ANDROID_APP_KEY = "27594394d";
    private const string APPLE_APP_KEY = "";
    private const float REWARD_COOLDOWN_SECONDS = 3;

    [SerializeField] private PlayerEconomyManager playerEconomyManager;
    [SerializeField] private string adUnitId = "odxokavoj8oquw65";
    [SerializeField] private string placementName = "Main_Menu";


    private AdRewardsServiceBindings bindings;
    private LevelPlayRewardedAd rewardedAd;

    private bool isInitialized;
    private string lastAdToken;
    private DateTime lastAdCompletionTime;




    private void Start()
    {
        bindings = new AdRewardsServiceBindings(CloudCodeService.Instance);
        RegisterSDKEvents();

        AuthenticationService.Instance.SignedIn += InitializeRewardedAds;

        if(AuthenticationService.Instance.IsSignedIn && !isInitialized)
        {
            InitializeRewardedAds();
        }
    }

    private void InitializeRewardedAds()
    {
        string appKey = GetPlatformAppKey();

        string userId = AuthenticationService.Instance.PlayerId;

        LevelPlay.SetMetaData("is_test_suite", "enable");
        LevelPlay.Init(appKey, userId);
        LevelPlay.SetPauseGame(true);
    }

    private string GetPlatformAppKey()
    {
        
#if UNITY_ANDROID
        return ANDROID_APP_KEY;
#elif UNITY_IPHONE
        return APPLE_APP_KEY;
#else
        Debug.LogWarning("Unexpected platform for ads");
        return "unexpected platform";
#endif

    }

    private void RegisterSDKEvents()
    {
        LevelPlay.OnInitSuccess += OnSDKInitializationComplited;
        LevelPlay.OnInitFailed += OnSDKInitializationFailed;
    }

    private void OnSDKInitializationComplited(LevelPlayConfiguration config)
    {
        if (isInitialized) return;

        isInitialized = true;
        Debug.Log("LevelPlay SDK initialized successfully");
#if DEVELOPMENT_BUILD

        LevelPlay.ValidateIntegration();
        LaunchTestSuite();
        Debug.Log("Launching test suite");

#endif

        CreateRewardedAd();

        RegisterToAdEvents();

        LoadRewardedAd();
    }
    private void CreateRewardedAd()
    {
        rewardedAd = new LevelPlayRewardedAd(adUnitId);
    }

    private void LoadRewardedAd()
    {
        if (rewardedAd != null)
        {
            rewardedAd.LoadAd();
        }
    }

    private void ShowRewardedAd()
    {
        if (string.IsNullOrEmpty(placementName))
        {
            Debug.LogWarning("Placement name is empty, showing as unit without placement");
            rewardedAd.ShowAd();
        }
        else
        {
            rewardedAd.ShowAd(placementName);
        }
    }

    public void ClickShowAdReward()
    {
        if (CanShowAd())
        {
            ShowRewardedAd();
        }
        else
        {
            Debug.LogWarning($"Cannot show ad.");
        }
    }

    private bool CanShowAd()
    {
        if (!isInitialized)
        {
            Debug.LogWarning("SDK not initialized");
            return false;
        }

        if (rewardedAd == null)
        {
            Debug.LogWarning("Rewarded ad object not created");
            return false;
        }

        bool isAdReady = rewardedAd.IsAdReady();
        bool isCooldownExpired = HasCooldownExpired();
        bool isPlacementCapped = LevelPlayRewardedAd.IsPlacementCapped(placementName);

        if (!isAdReady)
        {
            Debug.LogWarning("Ad not ready - still loading or no inventory available");
        }

        if (isPlacementCapped)
        {
            Debug.LogWarning("Placement has reached its capping limit");
        }

        return isAdReady && isCooldownExpired && !isPlacementCapped;
    }

    private bool HasCooldownExpired()
    {
        float remaining = GetRemainingCooldownSeconds();

        if(remaining > 0f)
        {
            Debug.Log($"Ad still on cooldown for {remaining:F1} seconds");
            return false;
        }

        return true;
    }

    public float GetRemainingCooldownSeconds()
    {
        if (lastAdCompletionTime == default)
        {
            return 0f;
        }

        TimeSpan timeSinceLastAd = DateTime.UtcNow - lastAdCompletionTime;
        float remaining = REWARD_COOLDOWN_SECONDS - (float)timeSinceLastAd.TotalSeconds;

        return Math.Max(0, remaining);
    }

    #region Handle Reward Events

    private void RegisterToAdEvents()
    {
        rewardedAd.OnAdLoaded += OnAdLoadedSuccessfully;
        rewardedAd.OnAdLoadFailed += OnAdLoadFailed;

        rewardedAd.OnAdDisplayed += OnAdDisplayed;
        rewardedAd.OnAdDisplayFailed += OnAdFailedToDisplay;

        rewardedAd.OnAdRewarded += OnProcessAdReward;

        rewardedAd.OnAdClosed += OnAdClosed;

        rewardedAd.OnAdClicked += OnAdClicked;
        rewardedAd.OnAdInfoChanged += OnAdInfoChanged;
    }

    private void UnregisterToAdEvents()
    {
        if (rewardedAd == null) return;

        rewardedAd.OnAdLoaded -= OnAdLoadedSuccessfully;
        rewardedAd.OnAdLoadFailed -= OnAdLoadFailed;

        rewardedAd.OnAdDisplayed -= OnAdDisplayed;
        rewardedAd.OnAdDisplayFailed -= OnAdFailedToDisplay;

        rewardedAd.OnAdRewarded -= OnProcessAdReward;

        rewardedAd.OnAdClosed -= OnAdClosed;

        rewardedAd.OnAdClicked -= OnAdClicked;
        rewardedAd.OnAdInfoChanged -= OnAdInfoChanged;
    }


    private void OnAdLoadedSuccessfully(LevelPlayAdInfo adInfo)
    {
        AdAvailable?.Invoke(true);
        Debug.Log($"Rewarded ad loaded: {adInfo.AdNetwork}");
    }

    private void OnAdLoadFailed(LevelPlayAdError error)
    {
        Debug.LogError($"Rewarded ad failed to load: {error.ErrorMessage}(Code: {error.ErrorCode})");

        switch (error.ErrorCode)
        {

            case 509:
                Debug.LogWarning("No asd to show, will retry");
                Invoke(nameof(LoadRewardedAd), 5f);
                break;

            case 520:
                Debug.LogWarning("No internet connection");
                AdAvailable?.Invoke(false);
                break;

            case 524:
                Debug.LogWarning("Placement is capped, will not retry loading");
                AdAvailable?.Invoke(false);
                break;

            case 526:
                Debug.LogWarning("Ad unit has reached daily cap, will not retry");
                AdAvailable?.Invoke(false);
                break;

            default:
                Debug.LogWarning($"Unknown error code {error.ErrorCode}, retrying with standard delay");
                Invoke(nameof(LoadRewardedAd), 2f);
                break;

        }
    }

    private void OnAdDisplayed(LevelPlayAdInfo adInfo)
    {
        Debug.Log("Rewarded ad displayed");
    }

    private void OnAdFailedToDisplay(LevelPlayAdInfo adInfo, LevelPlayAdError error)
    {
        Debug.LogError($"Rewarded ad failed to display: {error}");
        AdSuccessfullyCompleted?.Invoke(false);
    }

    private async void OnProcessAdReward(LevelPlayAdInfo adInfo, LevelPlayReward reward)
    {
        try
        {
            Debug.Log($"Validating ad reward: {reward.Name} amount: {reward.Amount}");

            string adToken = GenerateAdToken(adInfo, reward);
            DateTime completionTime = DateTime.UtcNow;
            
            lastAdToken = adToken;
            lastAdCompletionTime = completionTime;
            
            var playerEconomyData = await bindings.HandleGrantVideoAdReward(adToken);
            playerEconomyManager.HandleEconomyUpdate(playerEconomyData);
            
            AdSuccessfullyCompleted?.Invoke(true);

            Debug.Log($"Ad reward granted successfully: {reward.Name} x{reward.Amount}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to validate ad reward: {e.Message}");
            AdSuccessfullyCompleted?.Invoke(false);
        }
    }

    private string GenerateAdToken(LevelPlayAdInfo adInfo, LevelPlayReward reward)
    {
        if(adInfo == null)
        {
            throw new ArgumentException("Ad info cannot be null for token generation");
        }

        if(reward == null)
        {
            throw new ArgumentException("Reward info cannot be null for token generation");
        }

        if (string.IsNullOrEmpty(adInfo.InstanceId))
        {
            throw new ArgumentException("Ad instance ID cannot be null or empty for token generation");
        }

        var tokenData = new
        {
            Timestamp = DateTime.UtcNow.Ticks,
            InstanceId = adInfo.InstanceId,
            InstanceName = adInfo.InstanceName,
            AdNetwork = adInfo.AdNetwork,
            RewardName = "GOLD",    // can use RewardName = reward.Name, but in editor reward is "editor_reward" and will fail validation
            RewardAmount = reward.Amount
        };

        string json = JsonConvert.SerializeObject(tokenData);
        Debug.Log($"Generated ad token: {json}");
        return json;
    }

    private void OnAdClosed(LevelPlayAdInfo adInfo)
    {
        Debug.Log("Rewarded ad closed");
        LoadRewardedAd();
    }

    private void OnAdClicked(LevelPlayAdInfo adInfo)
    {
        Debug.Log("Rewarded ad clicked");
    }

    private void OnAdInfoChanged(LevelPlayAdInfo adInfo)
    {
        Debug.Log($"Rewarded ad info changed: {adInfo.AdNetwork}");

        if(adInfo != null)
        {
            Debug.Log($"Updated ad info - Network: {adInfo.AdNetwork}, Instance: {adInfo.InstanceId}");
            // Could trigger UI updates here
        }
    }

    #endregion

    
    private void LaunchTestSuite()
    {
        LevelPlay.LaunchTestSuite();
    }

    private void OnSDKInitializationFailed(LevelPlayInitError error)
    {
        throw new NotImplementedException();
    }

    private void OnDestroy()
    {
        LevelPlay.OnInitSuccess -= OnSDKInitializationComplited;
        LevelPlay.OnInitFailed -= OnSDKInitializationFailed;

        AuthenticationService.Instance.SignedIn -= InitializeRewardedAds;

        UnregisterToAdEvents();
    }
}
