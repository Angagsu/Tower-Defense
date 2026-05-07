using Assets.Scripts;
using UnityEngine;

public class StoneProjectile : LauncherProjectile
{
    private float stunDuration;

    public override void SetTarget(AttackStats attackStats, Character targetCharacter, Transform target, float damage, bool hasPassiveSkill, float blastRadius, float stunDuration, IAttackable attackableTarget)
    {
        base.SetTarget(attackStats, targetCharacter, target, damage, hasPassiveSkill, blastRadius, stunDuration, attackableTarget);

        targetPoint = target.position;
        explosionRadius = blastRadius;
        this.stunDuration = stunDuration;
        Launch(transform.position, targetPoint);
    }

    protected override void HitTarget()
    {
        if (!isHitted)
        {
            isHitted = true;
            impactEffect.gameObject.SetActive(true);

            if (stunDuration == 0)
            {
                ExplodeWithoutStunEffect();
            }
            else
            {
                Explode();
            }

            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }

            coroutine = StartCoroutine(ImpactEffectDuration());
        }
    }

    protected override void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.TryGetComponent<BaseMonster>(out var monster))
            {
                monster.TakeDamage(attackStats);
                monster.GetStun(stunDuration);
            }
        }
    }

    private void ExplodeWithoutStunEffect()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.TryGetComponent<BaseMonster>(out var monster))
            {
                monster.TakeDamage(attackStats);
            }
        }
    }
}
