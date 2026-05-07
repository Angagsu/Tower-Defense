using UnityEngine;

public class ElfSuperAttackProjectile : SuperAttackProjectile
{
    private float criticalDamage;

    public override void Setup(BaseHero baseHero, bool hasPassiveSkill)
    {
        base.Setup(baseHero, hasPassiveSkill);

        criticalDamage = superAttackDamage * (passiveSkill_1 / 100);
    }

    protected override void Explode()
    {
        int hitCount = Physics.OverlapSphereNonAlloc(transform.position, explosionRadius, overlapResults);

        for (int i = 0; i < hitCount; i++)
        {
            var collider = overlapResults[i];

            if (collider != null && collider.gameObject.TryGetComponent<BaseMonster>(out var monster))
            {
                monster.TakePureDamage(DoDamage.CalculateDamage(monster.AttackStats, attackStats.DamageData, superAttackDamage));
            }
        }
    }

    protected override void ExplodeWithPassiveEffect()
    {
        int hitCount = Physics.OverlapSphereNonAlloc(transform.position, explosionRadius, overlapResults);

        for (int i = 0; i < hitCount; i++)
        {
            var collider = overlapResults[i];

            if (collider != null && collider.gameObject.TryGetComponent<BaseMonster>(out var monster))
            {
                monster.TakePureDamage(DoDamage.CalculateDamage(monster.AttackStats, attackStats.DamageData, criticalDamage));
            }
        }
    }
}
