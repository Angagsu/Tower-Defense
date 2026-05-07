using System;
using UnityEngine;


public static class DoDamage 
{
    public static void ApplyDamage(AttackStats damageReceiver,DamageData attackerDamageData)
    {
        switch (attackerDamageData.DamageType)
        {
            case DamageType.Physical:
                ApplyPhysicalDamage(damageReceiver, attackerDamageData);
                break;

            case DamageType.Magical:
                ApplyMagicalDamage(damageReceiver, attackerDamageData);
                break;

            case DamageType.Pure:
                ApplyPureDamage(damageReceiver, attackerDamageData);
                break;
        }

        damageReceiver.Health = Mathf.Max(0f, damageReceiver.Health);
    }

    private static void ApplySplitDamage(AttackStats enemy,DamageData attackerDamageData)
    {
        float armorValue = Mathf.Max(0f, enemy.PhysicalResistance - attackerDamageData.PhysicalPenetration);

        float multiplier = 100f / (100f + armorValue);
        float reducedDamage = attackerDamageData.DamageAmount * multiplier;

        float armorDamage = reducedDamage * attackerDamageData.ArmorPercent;
        float healthDamage = reducedDamage * attackerDamageData.HealthPercent;

        
        enemy.Armor = Mathf.Max(0f, enemy.Armor - armorDamage);

        enemy.Health = enemy.Armor != 0 ? Mathf.Max(0f, enemy.Health - healthDamage) : Mathf.Max(0f, enemy.Health - reducedDamage);
        //enemy.Health = Mathf.Max(0f, enemy.Health - healthDamage);
    }

    private static void ApplyPhysicalDamage(AttackStats damageReceiver,DamageData attackerDamageData)
    {
        //float armorValue = Mathf.Max(0f, enemy.PhysicalResistance - damage.PhysicalPenetration);
        //
        //float multiplier = 100f / (100f + armorValue);
        //float reducedDamage = damage.DamageAmount * multiplier;
        //
        //float toArmor = Mathf.Min(enemy.Armor, reducedDamage);
        //enemy.Armor -= toArmor;
        //
        //enemy.Health -= (reducedDamage - toArmor);

        float armorValue = Mathf.Max(0f, damageReceiver.PhysicalResistance - attackerDamageData.PhysicalPenetration);

        float multiplier = 100f / (100f + armorValue);
        float reducedDamage = attackerDamageData.DamageAmount * multiplier;

        float armorDamage = reducedDamage * attackerDamageData.ArmorPercent;
        float healthDamage = reducedDamage * attackerDamageData.HealthPercent;


        damageReceiver.Armor = Mathf.Max(0f, damageReceiver.Armor - armorDamage);

        damageReceiver.Health = damageReceiver.Armor != 0 ? Mathf.Max(0f, damageReceiver.Health - healthDamage) : Mathf.Max(0f, damageReceiver.Health - reducedDamage);
    }

    private static void ApplyMagicalDamage(AttackStats damageReceiver, DamageData attackerDamageData)
    {
        float armorValue = Mathf.Max(0f, damageReceiver.MagicalResistance - attackerDamageData.MagicalPenetration);

        float multiplier = 100f / (100f + armorValue);
        float reducedDamage = attackerDamageData.DamageAmount * multiplier;

        damageReceiver.Health -= reducedDamage;
    }

    private static void ApplyPureDamage(AttackStats enemy,DamageData damage)
    {
        enemy.Health -= damage.DamageAmount;
    }

    public static float CalculateDamage(AttackStats damageReceiver,DamageData attackerDamageData, float passiveDamage)
    {
        float physicArmor = Mathf.Max(0f, damageReceiver.PhysicalResistance - attackerDamageData.PhysicalPenetration);
        float magicArmor = Mathf.Max(0f, damageReceiver.MagicalResistance - attackerDamageData.MagicalPenetration);

        float physicDamageMultiplier = 100f / (100f + physicArmor);
        float reducedPhysicDamage = passiveDamage * physicDamageMultiplier;

        float magicDamageMultiplier = 100f / (100f + magicArmor);
        float reducedMagicDamage = passiveDamage * magicDamageMultiplier;

       //float armorDamage = reducedMagicDamage * attackerDamageData.ArmorPercent;
       //float healthDamage = reducedPhysicDamage * attackerDamageData.HealthPercent;

        float toArmor = Mathf.Min(damageReceiver.Armor, reducedPhysicDamage);

        float finalDamage = default;

        switch (attackerDamageData.DamageType)
        {
            case DamageType.Physical:
                finalDamage = reducedPhysicDamage - toArmor;
                break;
            case DamageType.Magical:
                finalDamage = reducedMagicDamage;
                break;
            case DamageType.Pure:
                finalDamage = attackerDamageData.DamageAmount;
                break;
            default:
                break;
        }

        return finalDamage;
    }
}

[Serializable]
public class AttackStats
{
    public float Health;
    public float MaxHealth;

    public float Armor;
    public float MaxArmor;

    public float PhysicalResistance;
    public float MagicalResistance;

    public ArmorType ArmorType;
    public DamageData DamageData;  
}

[Serializable]
public struct DamageData
{
    public float DamageAmount;
    public DamageType DamageType;

    public float PhysicalPenetration;
    public float MagicalPenetration;
    public float ArmorPercent;
    public float HealthPercent;

    //public bool HasPassiveSkill;
    //public float[] PassiveSkills;
}

public enum DamageType
{
    Physical,
    Magical,
    Pure
}

public enum ArmorType
{
    Unarmored,
    Light,
    Medium,
    Heavy
}