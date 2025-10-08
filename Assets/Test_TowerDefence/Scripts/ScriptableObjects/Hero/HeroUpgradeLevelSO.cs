using UnityEngine;

[CreateAssetMenu(fileName = "HeroUpgradeLevel", menuName = "SO/HeroSO/HeroUpgradeLevel")]
public class HeroUpgradeLevelSO : ScriptableObject
{
    public HeroUpgradeLevel HeroUpgradeLevel;
    public float Health;
    public float Damage;
    public float MoveSpeed;
    public int UpgradeCostByGold;
}
