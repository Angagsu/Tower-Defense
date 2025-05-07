using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEntryPoint
{
    public static GameEntryPoint Instance { get; private set; }
    private Coroutines coroutines;
    private LoadingScreenUI loadingScreenUI;
    public AsyncSceneLoadService SceneLoader { get; private set; }



    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void AutoStartGame()
    {
        Application.targetFrameRate = 120;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
   
        Instance = new GameEntryPoint();
        Instance.RunGame();
    }

    private GameEntryPoint()
    {
        coroutines = new GameObject("[COROUTINES]").AddComponent<Coroutines>();
        Object.DontDestroyOnLoad(coroutines.gameObject);

        SceneLoader = new GameObject("[SceneLoadService]").AddComponent<AsyncSceneLoadService>();
        Object.DontDestroyOnLoad(SceneLoader.gameObject);

        var loadingScreenUIPrefab = Resources.Load<LoadingScreenUI>("LoadingScreenUI");
        loadingScreenUI = Object.Instantiate(loadingScreenUIPrefab);
        Object.DontDestroyOnLoad(loadingScreenUI.gameObject);
        SceneLoader.SetLoadingScreen(loadingScreenUI);
    }

    private void RunGame()
    {
#if UNITY_EDITOR
        var sceneName = SceneManager.GetActiveScene().name;
        coroutines.StartCoroutine(LoadSceneInEditor(sceneName));
#else
        coroutines.StartCoroutine(LoadScene(Scenes.LEVELS_MAP));
#endif
    }

    private IEnumerator LoadScene(string sceneName)
    {
        yield return SceneManager.LoadSceneAsync(Scenes.BOOT);
        coroutines.StartCoroutine(SceneLoader.LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneInEditor(string sceneName)
    {
        yield return SceneManager.LoadSceneAsync(Scenes.BOOT);
        coroutines.StartCoroutine(SceneLoader.LoadSceneAsync(sceneName));
    }
}
