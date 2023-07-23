using UnityEngine.UI;
using UnityEngine;

public class LevelSelecter : MonoBehaviour
{
    [SerializeField] SceneFader sceneFader;
    [SerializeField] private Button[] levelButtons;
    [SerializeField] private Sprite inactivButtonsSprite;
    private string levelReachedKey = "levelReached";


    private void Awake()
    {
        int levelReached = PlayerPrefs.GetInt(levelReachedKey, 1);
        //PlayerPrefs.DeleteAll();
        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (i + 1 > levelReached)
            {
                levelButtons[i].interactable = false;
                //levelButtons[i].image.sprite = inactivButtonsSprite;
            }
        }
    }
    
    public void LevelSelect(string levelSelect)
    {
        sceneFader.FadeTo(levelSelect);
    }
}
