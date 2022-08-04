
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] SceneFader sceneFader;
    
    public void Play()
    {
        sceneFader.FadeTo("LevelsMap");
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}
