using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AsyncSceneLoadService : MonoBehaviour, IService
{
    private LoadingScreenUI loadingScreenUI;

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



    public IEnumerator LoadSceneAsync(string sceneName)
    {
        loadingScreenUI.ShowLoadingScreen();

        SceneManager.LoadSceneAsync(Scenes.LOADING);
        
        yield return SceneManager.LoadSceneAsync(sceneName);

        yield return new WaitForSeconds(1);

        loadingScreenUI.HideLoadingScreen();
    }

    private IEnumerator LoadSceneAsyncByIndex(int index)
    {
        loadingScreenUI.ShowLoadingScreen();

        SceneManager.LoadSceneAsync(Scenes.LOADING);

        yield return SceneManager.LoadSceneAsync(index);

        yield return new WaitForSeconds(1);

        loadingScreenUI.HideLoadingScreen();
    }

    public void StartGame()
    {
        StartCoroutine(LoadSceneAsync(Scenes.LEVELS_MAP));
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
