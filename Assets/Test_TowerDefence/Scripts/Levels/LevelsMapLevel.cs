using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelsMapLevel : MonoBehaviour
{
    [field: SerializeField] public LevelDataToStor LevelDataToStor { get; private set; }


    [SerializeField] private Button levelButton;
    [SerializeField] private GameObject[] starImages;
    [SerializeField] private Image lockImage;
    [Space(10)]
    [SerializeField] private LevelsMapController levelsMapController;


    private void Awake()
    {
        levelButton.onClick.AddListener(OnButtonClick);
    }

    public void SetParams(LevelDataToStor levelDataToStor)
    {
        this.LevelDataToStor.IsLevelAchieved = levelDataToStor.IsLevelAchieved;
        this.LevelDataToStor.StarCount = levelDataToStor.StarCount;
        this.LevelDataToStor.LevelID = levelDataToStor.LevelID;

        if (levelDataToStor.IsLevelAchieved)
        {
            lockImage.gameObject.SetActive(false);

            for (int i = 0; i < this.LevelDataToStor.StarCount; i++)
            {
                starImages[i].gameObject.SetActive(true);
            }
        }
        else
        {
            lockImage.gameObject.SetActive(true);
        }
    }

    public void UnlockLevelButton()
    {
        LevelDataToStor.IsLevelAchieved = true;
        lockImage.gameObject.SetActive(false);
    }

    private void OnButtonClick()
    {
        levelsMapController.OnLevelButtonClicked(this);
    }
}



[Serializable]
public class LevelDataToStor
{
    public int StarCount;
    public int LevelID;
    public bool IsLevelAchieved;
}


