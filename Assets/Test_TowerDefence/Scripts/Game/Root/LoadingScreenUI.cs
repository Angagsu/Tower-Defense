using UnityEngine;

public class LoadingScreenUI : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject angagsuLogo;
    [SerializeField] private GameObject splashScreen;
    [SerializeField] private Transform uiSceneContainer;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip logoAudioClip;
    [SerializeField] private AudioClip startLoadingAudioClip;
    [SerializeField] private AudioClip endLoadingAudioClip;

    private int isSceneLoadedHash;

    private void Awake()
    {
        isSceneLoadedHash = Animator.StringToHash("isSceneLoaded");
        //HideLogo();
        //HideLoadingScreen();
    }

    public void SetOpenGateAnimation(bool isSceneLoaded)
    {
        animator.SetBool(isSceneLoadedHash, isSceneLoaded);
    }

    public void ShowLoadingScreen()
    {
        loadingScreen.SetActive(true);
        audioSource.PlayOneShot(startLoadingAudioClip);
    }

    public void HideLoadingScreen()
    {
        loadingScreen.SetActive(false);
    }

    public void AttachSceneUI(GameObject sceneUI)
    {
        ClearSceneUI();

        sceneUI.transform.SetParent(uiSceneContainer, false);
    }

    private void ClearSceneUI()
    {
        var chiledCount = uiSceneContainer.childCount;

        for (int i = 0; i < chiledCount; i++)
        {
            Destroy(uiSceneContainer.GetChild(i).gameObject);
        }
    }

    public void HideLogo()
    {
        angagsuLogo.SetActive(false);
        Destroy(angagsuLogo);
    }

    public void ShowLogo()
    {
        angagsuLogo.SetActive(true);
    }

    public void ShowSplashScreen()
    {
        splashScreen.SetActive(true);
    }

    public void HideSplashScreen()
    {
        splashScreen.SetActive(false);
        Destroy(splashScreen);
    }

    public void PlayOpenGateSFX(float volume)
    {
        audioSource.volume = volume;
        audioSource.PlayOneShot(endLoadingAudioClip);
    }

    public void PlayLogoSFX(float volume)
    {
        audioSource.volume = volume;
        audioSource.PlayOneShot(logoAudioClip);
    }
}
