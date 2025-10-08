using UnityEngine;


[CreateAssetMenu(fileName = "Hero", menuName = "SO/HeroSO/Hero")]
public class HeroSO : ScriptableObject
{
    [field: SerializeField] public HeroUpgradeLevelSO CurrentUpgradeLevel { get;  set; }
    [field: SerializeField] public HeroUpgradeLevelsSO UpgradeLevelsSO { get; private set; }
    [field: SerializeField] public Sprite Sprite {  get; private set; }
    [field: SerializeField] public BaseHero Prefab { get; private set; }
    [field: SerializeField] public HeroData HeroData { get; private set; }
}


