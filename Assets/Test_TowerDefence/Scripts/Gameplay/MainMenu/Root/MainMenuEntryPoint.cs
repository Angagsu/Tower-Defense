using System;
using UnityEngine;

public class MainMenuEntryPoint : MonoBehaviour
{
    public event Action GoToGameplaySceneRequested;

    [SerializeField] private UIMainMenuRootBinder sceneUIRootPrefab;

    public void Run(LoadingScreenUI uIRootView)
    {
        var sceneUI = Instantiate(sceneUIRootPrefab);
        uIRootView.AttachSceneUI(sceneUI.gameObject);

        sceneUI.GoToGameplayButtonClicked += () => { GoToGameplaySceneRequested?.Invoke(); };
    }
}
