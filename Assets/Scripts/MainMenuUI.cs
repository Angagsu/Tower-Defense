
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] SceneFader sceneFader;
    
    public void Play()
    {
        sceneFader.FadeTo("MainLevel");
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}
