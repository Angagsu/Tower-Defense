using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level_1_Config", menuName = "SO/Level Config")]
public class LevelConfigSO : ScriptableObject
{
    public List<Wayes> Wayes;
}
