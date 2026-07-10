using System;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Authentication.PlayerAccounts;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.UI;
using Facebook.Unity;



public class LoginController : MonoBehaviour
{

    private async void Awake()
    {
        InitializeFacebook();

        if(UnityServices.State.Equals(ServicesInitializationState.Uninitialized))
        {
            Debug.Log("Initializing Unity Services...");
            await UnityServices.InitializeAsync();
        }

        PlayerAccountService.Instance.SignedIn += SignInOrLinkWithUnity;
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
        var permissions = new System.Collections.Generic.List<string>() { "public_profile", "email" };
        FB.LogInWithReadPermissions(permissions, async result =>
        {
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
        });
    }

    private async void FacebookAuthCallback(ILoginResult result)
    {
        if(FB.IsLoggedIn)
        {
            var accessToken = AccessToken.CurrentAccessToken.TokenString;
            Debug.Log($"Facebook login succeeded. Access Token: {accessToken}");
            if(!AuthenticationService.Instance.IsSignedIn)
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

    //public void SignOut(bool clearSessionToken = false)
    //{
    //    AuthenticationService.Instance.SignOut(clearSessionToken);
    //
    //    PlayerAccountService.Instance.SignOut();
    //}
    //
    //public void ClearSessionToken()
    //{
    //    AuthenticationService.Instance.ClearSessionToken();
    //
    //    Debug.Log("Session token cleared.");
    //}
    //
    //public void DeleteAccount()
    //{
    //    AuthenticationService.Instance.DeleteAccountAsync();
    //
    //    Debug.Log("Account deleted.");
    //}
}
