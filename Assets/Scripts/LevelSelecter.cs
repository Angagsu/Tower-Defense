using UnityEngine.UI;
using UnityEngine;

public class LevelSelecter : MonoBehaviour
{
    [SerializeField] SceneFader sceneFader;
    [SerializeField] private Button[] levelButtons;
    private string levelReachedKey = "levelReached";
    
    private void Start()
    {
        int levelReached = PlayerPrefs.GetInt(levelReachedKey, 1);

        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (i + 1 > levelReached)
            {
                levelButtons[i].interactable = false;
            }
        }
    }
    public void LevelSelect(string levelSelect)
    {
        sceneFader.FadeTo(levelSelect);
    }
}
