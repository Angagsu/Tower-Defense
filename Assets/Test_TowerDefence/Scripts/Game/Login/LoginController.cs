using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Authentication.PlayerAccounts;
using Unity.Services.Core;
using UnityEngine;
using Facebook.Unity;
#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif


public class LoginController : MonoBehaviour
{
    public Action PlayerSignedIn;
    private string googlePlayGamesToken;
    

    private void Awake()
    {
#if UNITY_ANDROID
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        LoginGooglePlayGames();
# endif

        InitializeFacebook();  

        if(UnityServices.State.Equals(ServicesInitializationState.Initialized))
        {
            UnitySignInSubscription();
        }
        else
        {
            UnityServices.Initialized += UnitySignInSubscription;
        }
    }

    private async void Start()
    {
        if (!AuthenticationService.Instance.SessionTokenExists)
        {
            Debug.Log("No session token found");
            return;
        }

        Debug.Log("Returning player signing in...");
        await SignInAnonymouslyAsync();
    }

    private void OnDestroy()
    {
        PlayerAccountService.Instance.SignedIn -= SignInOrLinkWithUnity;
        UnityServices.Initialized -= UnitySignInSubscription;
    }

    private void UnitySignInSubscription()
    {
        PlayerAccountService.Instance.SignedIn += SignInOrLinkWithUnity;
    }

    #region Anonymous
    public async void StartSignInAnonymously()
    {
        await SignInAnonymouslyAsync();
    }

    private async Task SignInAnonymouslyAsync()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();

            Debug.Log("Sign in anonymously succeeded!");
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");

            PlayerSignedIn?.Invoke();
        }
        catch (AuthenticationException ex)
        {
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            Debug.LogException(ex);
        }
    }
    #endregion

    #region Unity
    public async void StartUnitySignInAsync()
    {
        if (PlayerAccountService.Instance.IsSignedIn)
        {
            SignInOrLinkWithUnity();
            return;
        }

        try
        {
            await PlayerAccountService.Instance.StartSignInAsync();
        }
        catch (RequestFailedException ex)
        {
            Debug.LogException(ex);
        }
    }

    private async void SignInOrLinkWithUnity()
    {
        try
        {
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                Debug.Log("Signing up with Unity PlayerAccout...");

                await AuthenticationService.Instance.SignInWithUnityAsync(PlayerAccountService.Instance.AccessToken);

                Debug.Log("Sign in with Unity PlayerAccount succeeded!");

                return;
            }

            if (!HasUnityID())
            {
                Debug.Log("Linking with Unity PlayerAccount...");

                await LinkWithUnityAsync(PlayerAccountService.Instance.AccessToken);

                return;
            }

            Debug.Log("Player is already signed in to their Unity Player Account");
        }
        catch (RequestFailedException ex)
        {
            Debug.LogException(ex);
        }
    }

    private bool HasUnityID()
    {
        return !string.IsNullOrEmpty(AuthenticationService.Instance.PlayerInfo.GetUnityId());
    }

    private async Task LinkWithUnityAsync(string accessToken)
    {
        try
        {
            await AuthenticationService.Instance.LinkWithUnityAsync(accessToken);

            Debug.Log("Link is successful.");
        }
        catch (AuthenticationException ex) when (ex.ErrorCode == AuthenticationErrorCodes.AccountAlreadyLinked)
        {
            Debug.LogError("This user is already linked with another account. Log in instead.");
        }
        catch (AuthenticationException ex)
        {
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            Debug.LogException(ex);
        }
    }
    #endregion

    #region Facebook
    private void InitializeFacebook()
    {
        if (!FB.IsInitialized)
        {
            FB.Init(InitCallback, OnHideUnity);
        }
        else
        {
            FB.ActivateApp();
        }
    }

    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
            FB.ActivateApp();
        }
        else
        {
            Debug.LogError("Failed to Initialize the Facebook SDK");
        }
    }

    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void StartFacebookSignIn()
    {
        Debug.Log("========== FACEBOOK LOGIN START ==========");
        Debug.Log($"FB.IsInitialized: {FB.IsInitialized}");
        Debug.Log($"FB.IsLoggedIn BEFORE: {FB.IsLoggedIn}");

        if (AccessToken.CurrentAccessToken != null)
        {
            Debug.Log($"Current Access Token: {AccessToken.CurrentAccessToken.TokenString}");
            Debug.Log($"Current User ID: {AccessToken.CurrentAccessToken.UserId}");
            Debug.Log($"Granted Permissions: {string.Join(", ", AccessToken.CurrentAccessToken.Permissions)}");
        }
        else
        {
            Debug.Log("CurrentAccessToken: NULL");
        }

        var permissions = new System.Collections.Generic.List<string>() { "public_profile", "email" };

        FB.LogInWithReadPermissions(permissions, async result =>
        {
            Debug.Log("========== FACEBOOK LOGIN CALLBACK ==========");

            Debug.Log($"Result.Cancelled: {result.Cancelled}");
            Debug.Log($"Result.Error: {result.Error}");
            Debug.Log($"Result.RawResult: {result.RawResult}");

            Debug.Log($"FB.IsLoggedIn AFTER: {FB.IsLoggedIn}");

            if (AccessToken.CurrentAccessToken != null)
            {
                Debug.Log($"Access Token: {AccessToken.CurrentAccessToken.TokenString}");
                Debug.Log($"User ID: {AccessToken.CurrentAccessToken.UserId}");
                Debug.Log($"Granted Permissions: {string.Join(", ", AccessToken.CurrentAccessToken.Permissions)}");
            }
            else
            {
                Debug.Log("CurrentAccessToken: NULL");
            }

            if (FB.IsLoggedIn)
            {
                var accessToken = AccessToken.CurrentAccessToken.TokenString;
                Debug.Log($"Facebook login succeeded. Access Token: {accessToken}");
                if (!AuthenticationService.Instance.IsSignedIn)
                {
                    Debug.Log("Signing in with Facebook...");
                    await SignInWithFacebookAsync(accessToken);
                }
                else
                {
                    Debug.Log("Linking with Facebook...");
                    await LinkWithFacebookAsync(accessToken);
                }
            }
            else
            {
                Debug.LogError("User cancelled Facebook login");
            }

            Debug.Log("========== FACEBOOK LOGIN END ==========");
        });
    }

    private async Task LinkWithFacebookAsync(string accessToken)
    {
        try
        {
            await AuthenticationService.Instance.LinkWithFacebookAsync(accessToken);
        }
        catch (AuthenticationException ex) when (ex.ErrorCode == AuthenticationErrorCodes.AccountAlreadyLinked)
        {
            Debug.LogException(ex);
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    private async Task SignInWithFacebookAsync(string accessToken)
    {
        try
        {
            await AuthenticationService.Instance.SignInWithFacebookAsync(accessToken);
            Debug.Log("Sign in with Facebook succeeded!");
        }
        catch (RequestFailedException ex)
        {
            Debug.LogException(ex);
        }
    }

    #endregion

    #region Google Play Games

    public void StartSignInWithGooglePlayGames()
    {
        if (!PlayGamesPlatform.Instance.IsAuthenticated())
        {
            Debug.LogWarning("Google Play Games is not authenticated. Please log in first.");
            LoginGooglePlayGames();
            return;
        }

        SignInOrLinkWithGooglePlayGames();
    }

    private void LoginGooglePlayGames()
    {
        PlayGamesPlatform.Instance.Authenticate((status) =>
        {
            if (status == SignInStatus.Success)
            {
                Debug.Log("Google Play Games login succeeded!");

                PlayGamesPlatform.Instance.RequestServerSideAccess(true, code =>
                {
                    Debug.Log($"Google Play Games server auth code: {code}");
                    googlePlayGamesToken = code;
                });
            }
            else
            {
                Debug.LogError($"Google Play Games login failed: {status}");
            }
        });
    }

    private async void SignInOrLinkWithGooglePlayGames()
    {
        if (string.IsNullOrEmpty(googlePlayGamesToken))
        {
            //Debug.LogWarning("Google Play Games token is not available. Please log in first.");
            //LoginGooglePlayGames();
            //return;
            Debug.LogWarning("Authorization code is null or empty");
            return;
        }

        if (!AuthenticationService.Instance.IsSignedIn)
        {
            Debug.Log("Signing in with Google Play Games...");
            await SignInWithGooglePlayGamesAsync(googlePlayGamesToken);
        }
        else
        {
            Debug.Log("Linking with Google Play Games...");
            await LinkWithGooglePlayGamesAsync(googlePlayGamesToken);
        }
    }

    private async Task SignInWithGooglePlayGamesAsync(string googlePlayGamesToken)
    {
        try
        {
            await AuthenticationService.Instance.SignInWithGooglePlayGamesAsync(googlePlayGamesToken);
            Debug.Log("Sign in with Google Play Games succeeded!");
        }
        catch (AuthenticationException ex)
        {
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            Debug.LogException(ex);
        }
    }

    private async Task LinkWithGooglePlayGamesAsync(string googlePlayGamesToken)
    {
        try
        {
            await AuthenticationService.Instance.LinkWithGooglePlayGamesAsync(googlePlayGamesToken);
            Debug.Log("Link with Google Play Games succeeded!");
        }
        catch (AuthenticationException ex) when (ex.ErrorCode == AuthenticationErrorCodes.AccountAlreadyLinked)
        {
            Debug.LogException(ex);
        }
        catch (AuthenticationException ex)
        {
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            Debug.LogException(ex);
        }
    }

    #endregion





    //async Task UnlinkUnityAsync()
    //{
    //    try
    //    {
    //        await AuthenticationService.Instance.UnlinkUnityAsync();
    //        Debug.Log("Unlink is successful.");
    //    }
    //    catch (AuthenticationException ex)
    //    {
    //        Debug.LogException(ex);
    //    }
    //    catch (RequestFailedException ex)
    //    {
    //        Debug.LogException(ex);
    //    }
    //}

    public void SignOut(bool clearSessionToken = false)
    {
        AuthenticationService.Instance.SignOut(clearSessionToken);
        PlayerAccountService.Instance.SignOut();

        ClearSessionToken();

        if (FB.IsLoggedIn)
        {
            Debug.Log("========== FACEBOOK LOGOUT ==========");

            FB.LogOut();

            if (AccessToken.CurrentAccessToken != null)
            {
                Debug.Log("AccessToken still exists after logout!");
            }
            else
            {
                Debug.Log("AccessToken is NULL after logout.");
            }

            Debug.Log("========== LOGOUT END ==========");
        }

        DeleteAccount();
    }
    
    public void ClearSessionToken()
    {
        AuthenticationService.Instance.ClearSessionToken();
    
        Debug.Log("Session token cleared.");
    }
    
    public void DeleteAccount()
    {
        AuthenticationService.Instance.DeleteAccountAsync();

        Debug.Log("Account deleted.");
    }
}
