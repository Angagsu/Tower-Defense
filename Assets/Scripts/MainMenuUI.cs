using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("LevelsMap");
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}
