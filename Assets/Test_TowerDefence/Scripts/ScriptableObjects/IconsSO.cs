using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IconsSO", menuName = "SO/IconsSO")]
public class IconsSO : ScriptableObject
{
    [field: SerializeField] public List<Sprite> AllIconsSprites { get; private set; }
    [field: SerializeField] public List<Sprite> AllHeroIconsSprites { get; private set; }
    [field: SerializeField] public List<Sprite> AttackStatsSprites { get; private set; }
    [field: SerializeField] public List<Sprite> DefenseSprites { get; private set; }    
}
