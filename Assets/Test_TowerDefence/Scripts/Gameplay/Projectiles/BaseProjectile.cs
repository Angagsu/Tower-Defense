using System.Collections;
using UnityEngine;

public class BaseProjectile : MonoBehaviour
{
    [SerializeField] protected float speed = 70f;
    [SerializeField] protected float explosionRadius = 0f;
    [SerializeField] protected ParticleSystem impactEffect;
    

    protected bool isHitted = false;
    private Transform target;
    private Coroutine coroutine;
    private float damage;
    

    public virtual void SetTarget(Transform target, float damage)
    {
        this.target = target;
        this.damage = damage;
    }


   protected virtual void Update()
    {
        if (target == null)
        {
            gameObject.SetActive(false);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.LookAt(target);
    }

    protected virtual void HitTarget()
    {
        if (!isHitted)
        {
            isHitted = true;
            impactEffect.gameObject.SetActive(true);

            if (explosionRadius > 0f)
            {
                Explode();
            }
            else
            {
                Damage(target);
            }

            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }

            coroutine = StartCoroutine(ImpactEffectDuration());
        }
    }

    protected virtual IEnumerator ImpactEffectDuration()
    {
        return default;
    }

    protected virtual void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.TryGetComponent<IAttackable>(out var target))
            {
                target.TakeDamage(damage);
            }
        }
    }

    protected virtual void Damage(Transform target)
    {
        if (target.TryGetComponent<IAttackable>(out var tower))
        {
            tower.TakeDamage(damage);
        }
    }
}
