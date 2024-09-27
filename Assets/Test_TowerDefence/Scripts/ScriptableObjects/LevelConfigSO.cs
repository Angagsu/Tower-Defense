using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level Configs/Level Config")]
public class LevelConfigSO : ScriptableObject
{
    public List<Wayes> Wayes;
}
