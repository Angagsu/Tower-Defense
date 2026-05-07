using UnityEngine;



public class DwarfSuperAttack : HeroSuperAttack
{
    public override void AreaEffect(AttackStats stats, float areaRadius, float effectDuration = 0, float effectValue = 0)
    {
        ApplyMassiveBuff(stats, areaRadius, effectDuration, superAttackValue);
    }

    protected override void ApplyMassiveBuff(AttackStats stats, float areaRadius, float effectDuration, float effectValue)
    {
        DetectHeroes(areaRadius);
        IncreaseAlliesResistance(stats, effectDuration, effectValue);
    }

    private void IncreaseAlliesResistance(AttackStats stats, float effectDuration, float effectValue)
    {
        foreach (BaseHero hero in heroes)
        {
            hero.IncreaseResistance(stats.DamageData.DamageType, effectValue, effectDuration);
        }

        affectableHeroes.Clear();
    }
}
