using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AsyncSceneLoadService : MonoBehaviour, IService
{
    private LoadingScreenUI loadingScreenUI;

    private void Awake()
    {
        SceneManager.sceneLoaded += SceneManager_SceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneManager_SceneLoaded;
    }

    private void SceneManager_SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        IsSceneLoaded();
    }

    public void SetLoadingScreen(LoadingScreenUI loadingScreenUI)
    {
        this.loadingScreenUI = loadingScreenUI;
    }
    
    private IEnumerator LoadAndStartGameplay()
    {
        yield return LoadSceneAsync(Scenes.BOOT);
        yield return LoadSceneAsync(Scenes.LOADING);

        yield return new WaitForSeconds(2);

    }

    private IEnumerator LoadAndStartMainMenu()
    {
        yield return LoadSceneAsync(Scenes.BOOT);
        yield return LoadSceneAsync(Scenes.MAIN_MENU);

        yield return new WaitForSeconds(2);
    }

    private bool IsSceneLoaded()
    {
        return true;
    }

    public IEnumerator LoadSceneAsync(string sceneName)
    {
        loadingScreenUI.ShowLoadingScreen();

        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadSceneAsync(Scenes.LOADING);
        
        yield return SceneManager.LoadSceneAsync(sceneName);


        yield return new WaitUntil(IsSceneLoaded);

        loadingScreenUI.SetOpenGateAnimation(true);
        loadingScreenUI.PlayOpenGateSFX(volume: 0.5f);
    }

    private IEnumerator LoadSceneAsyncByIndex(int index)
    {
        loadingScreenUI.ShowLoadingScreen();

        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadSceneAsync(Scenes.LOADING);

        yield return SceneManager.LoadSceneAsync(index);

 
        yield return new WaitUntil(IsSceneLoaded);

        loadingScreenUI.SetOpenGateAnimation(true);
        loadingScreenUI.PlayOpenGateSFX(volume: 0.5f);
    }

    public IEnumerator LoadSceneAsyncInStartGame(string sceneName)
    {
        SceneManager.LoadSceneAsync(Scenes.LOADING);

        yield return SceneManager.LoadSceneAsync(sceneName);
    }

    public void LoadSceneInStartGame(string sceneName)
    {
        StartCoroutine(LoadSceneAsyncInStartGame(sceneName));
    }

    public void LoadSceneByName(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    public void LoadSceneByIndex(int sceneIndex)
    {
        StartCoroutine(LoadSceneAsyncByIndex(sceneIndex));
    }
}
