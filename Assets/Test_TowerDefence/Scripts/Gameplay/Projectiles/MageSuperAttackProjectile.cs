using UnityEngine;

public class MageSuperAttackProjectile : SuperAttackProjectile
{
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
                monster.TakePureDamage(DoDamage.CalculateDamage(monster.AttackStats, attackStats.DamageData, superAttackDamage));
                monster.TakePeriodicDamage(attackStats, passiveSkill_1, passiveSkill_2);
            }
        }
    }
}
