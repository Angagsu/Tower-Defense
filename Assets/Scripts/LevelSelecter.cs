using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelecter : MonoBehaviour
{
    [SerializeField] SceneFader sceneFader;
    public void LevelSelect(string levelSelect)
    {
        sceneFader.FadeTo(levelSelect);
    }
}
