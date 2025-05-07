using UnityEngine;

public class MainMenuLoading : MonoBehaviour
{

    private AsyncSceneLoadService sceneLoadService;

    [Inject]
    public void Construct(AsyncSceneLoadService sceneLoadService)
    {
        this.sceneLoadService = sceneLoadService;
    }

    public void StartGame()
    {
        sceneLoadService.StartGame();
    }
}
