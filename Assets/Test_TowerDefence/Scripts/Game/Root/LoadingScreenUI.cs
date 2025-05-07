using UnityEngine;

public class LoadingScreenUI : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Transform uiSceneContainer;


    private void Awake()
    {
        HideLoadingScreen();
    }

    public void ShowLoadingScreen()
    {
        loadingScreen.SetActive(true);
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
}
