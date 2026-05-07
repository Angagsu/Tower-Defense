using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Hero", menuName = "SO/HeroSO/Hero")]
public class HeroSO : ScriptableObject
{
    [field: Header("Hero Description")]
    [field: Space(10)]
    [field: SerializeField] public string HeroName { get; private set; }
    [field: SerializeField] public string HeroDescription { get; private set; }
    [field: SerializeField] public Sprite Sprite { get; private set; }
    [field: SerializeField] public Sprite BigSprite { get; private set; }
    [field: SerializeField] public Sprite CardSprite { get; private set; }
    [field: SerializeField] public BaseHero Prefab { get; private set; }
   
    [field: Header("Hero Upgrades")]
    [field: Space(10)]
    [field: SerializeField] public HeroUpgradeLevelSO CurrentUpgradeLevel { get;  set; }
    [field: SerializeField] public HeroUpgradeLevelsSO UpgradeLevelsSO { get; private set; }

    [field: Header("Active Skills")]
    [field: Space(10)]
    [field: SerializeField] public Sprite ActiveSkillSprite { get; private set; }

    //[field: Space(10)]
    //[field: SerializeField, Range(0, 2)] public int ActiveSkillsAmount { get; private set; }

    [field: Space(15)]
    [field: SerializeField, Range(0, 10)] public int ActiveSkillUnlockLevel { get; private set; }
    [field: SerializeField] public float ActiveSkillMaxValue { get; private set; }
    [field: SerializeField] public string ActiveSkillName { get; private set; }
    [field: SerializeField] public string ActiveSkillDescripsion { get; private set; }


    [field: Header("Passive Skills")]
    [field: Space(10)]
    [field: SerializeField] public List<Sprite> PassivSkillsSprites { get; private set; }

    [field: Space(10)]
    [field: SerializeField, Range(0, 2)] public int PassiveSkillsAmount { get; private set; }

    [field: Space(15)]
    [field: SerializeField, Range(0, 10)] public List<int> PassiveSkillsUnlockLevels { get; private set; }
    [field: SerializeField] public List<string> PassiveSkillsNames { get; private set; }
    [field: SerializeField] public List<string> PassiveSkillsDescripsions { get; private set; }

    [field: Header("Hero Storable Data")]
    [field: Space(10)]
    [field: SerializeField] public HeroData HeroData { get; private set; }
}


