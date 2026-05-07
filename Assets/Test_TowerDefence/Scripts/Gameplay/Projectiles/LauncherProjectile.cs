using Assets.Scripts;
using System.Collections;
using UnityEngine;



public class LauncherProjectile : BaseProjectile
{
    [SerializeField] protected MeshRenderer meshRenderer;
    
    protected Vector3 targetPoint = Vector3.zero;
    protected Vector3 velocity;

    protected float gravity = -9.81f;
    

    public override void SetTarget(AttackStats attackStats, Character targetCharacter, Transform target, float damage, bool hasPassiveSkill, float blastRadius, float stunDuration, IAttackable attackableTarget)
    {
        base.SetTarget(attackStats, targetCharacter, target, damage, hasPassiveSkill, blastRadius, stunDuration, attackableTarget);

        targetPoint = target.position;
        explosionRadius = blastRadius;

        Launch(transform.position, targetPoint);
    }

    protected override void Update()
    {
        if (isPaused || isHitted)
        {
            return;
        }

        if (Vector3.Distance(targetPoint, transform.position) < 0.3f)
        {
            HitTarget();
        }

        velocity.y += gravity * Time.deltaTime;
        transform.position += velocity * Time.deltaTime;
    }

    public void Launch(Vector3 start, Vector3 target)
    {
        transform.position = start;

        Vector3 displacement = target - start;

        Vector3 horizontal = new Vector3(displacement.x, 0f, displacement.z);
        float distance = horizontal.magnitude;

        // Time based on speed
        float flightTime = (distance / speed);

        // Horizontal velocity
        Vector3 vXZ = horizontal / flightTime;

        // Vertical velocity (ballistic equation)
        float vY = (displacement.y - 0.5f * gravity * flightTime * flightTime) / flightTime;

        velocity = vXZ + Vector3.up * vY;
    }

    protected override void HitTarget()
    {
        if (!isHitted)
        {
            isHitted = true;
            impactEffect.gameObject.SetActive(true);

            if (!hasPassiveSkill)
            {
                Explode();
            }
            else
            {
                ExplodeWithPassiveEffect();
            }

            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }

            coroutine = StartCoroutine(ImpactEffectDuration());
        }
    }

    protected override void Explode() { }

    protected virtual void ExplodeWithPassiveEffect() { }

    protected override IEnumerator ImpactEffectDuration()
    {
        meshRenderer.enabled = false;

        yield return waitForSeconds;

        impactEffect.gameObject.SetActive(false);
        isHitted = false;
        meshRenderer.enabled = true;
        gameObject.SetActive(false);
    }
}
