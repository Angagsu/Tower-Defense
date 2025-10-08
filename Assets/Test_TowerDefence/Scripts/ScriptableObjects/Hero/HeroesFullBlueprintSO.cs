using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HeroesBlueprint", menuName = "SO/HeroSO/HeroesBlueprint")]
public class HeroesFullBlueprintSO : ScriptableObject
{
    [field: SerializeField] public List<HeroSO> Heroes {  get; private set; }
    [field: SerializeField] public List<HeroType> HeroTypes { get; private set; }
    [field: SerializeField] public List<HeroSO> AcquiredHeroes { get; set; }
}
