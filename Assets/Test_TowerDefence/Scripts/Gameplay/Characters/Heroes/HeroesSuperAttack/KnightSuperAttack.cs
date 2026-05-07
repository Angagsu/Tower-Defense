using UnityEngine;

public class KnightSuperAttack : HeroSuperAttack
{
    public override void AreaEffect(AttackStats stats, float areaRadius, float passiveEffectDuration = 0, float passiveEffect_2 = 0)
    {
        ApplyMassiveDamage(stats, areaRadius, passiveEffectDuration, superAttackValue);
    }

    protected override void ApplyMassiveDamage(AttackStats attackStats, float areaRadius, float stunDuration, float damage)
    {
        DetectMonsters(areaRadius);
        MassiveDamageWithStunEffect(attackStats, stunDuration, damage);
    }

    private void MassiveDamageWithStunEffect(AttackStats attackStats, float stunDuration, float damage)
    {
        foreach (var monster in affectableMonsters)
        {
            var finalDamage = DoDamage.CalculateDamage(monster.AttackStats, attackStats.DamageData, damage);

            monster.TakePureDamage(finalDamage);
            monster.GetStun(stunDuration);
        }

        affectableMonsters.Clear();
    }
}
