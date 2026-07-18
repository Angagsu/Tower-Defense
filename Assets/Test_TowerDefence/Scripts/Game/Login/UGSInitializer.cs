using Unity.Services.Analytics;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using Unity.Services.Analytics.Internal;
using UnityEngine.UnityConsent;


public class UGSInitializer : MonoBehaviour
{
    private async void Awake()
    {
        if (UnityServices.State.Equals(ServicesInitializationState.Uninitialized))
        {
            Debug.Log("Initializing Unity Services...");
            await UnityServices.InitializeAsync();
        }

        EndUserConsent.SetConsentState(new ConsentState
        {
            AnalyticsIntent = ConsentStatus.Granted,
            AdsIntent = ConsentStatus.Denied
        });

    }
}
