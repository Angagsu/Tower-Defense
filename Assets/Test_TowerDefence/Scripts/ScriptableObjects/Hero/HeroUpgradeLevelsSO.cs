using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HeroUpgradeLevels", menuName = "SO/HeroSO/HeroUpgradeLevels")]
public class HeroUpgradeLevelsSO : ScriptableObject
{
    public List<HeroUpgradeLevelSO> HeroUpgradeLevelSOs;
}
