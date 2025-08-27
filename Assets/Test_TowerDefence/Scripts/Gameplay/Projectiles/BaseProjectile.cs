using Assets.Scripts;
using System.Collections;
using UnityEngine;

public class BaseProjectile : MonoBehaviour
{
    public ProjectileUpgradeLevel UpgradeLevel => upgradLevel;

    [SerializeField] protected float speed = 70f;
    [SerializeField] protected float explosionRadius = 0f;
    [SerializeField] protected ParticleSystem impactEffect;
    [Space(15)]
    [SerializeField] protected ProjectileUpgradeLevel upgradLevel;

    protected GameplayStates gameplayStates;
    protected bool isHitted = false;
    private Transform target;
    private Coroutine coroutine;
    private Character targetCharacter;
    private float damage;

    protected bool isPaused;

    public virtual void Construct(GameplayStates gameplayStates)
    {
        this.gameplayStates = gameplayStates;

        this.gameplayStates.Paused += OnGameplayPause;
        this.gameplayStates.Unpaused += OnGameplayUnpause;
    }

    public virtual void SetTarget(Character targetCharacter, Transform target, float damage)
    {
        this.target = target;
        this.damage = damage;
        this.targetCharacter = targetCharacter; 
    }

    private void OnGameplayPause() => isPaused = true;

    private void OnGameplayUnpause() => isPaused = false;


    protected virtual void Update()
    {
        if (isPaused)
        {
            return;
        }

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
        targetCharacter.TakeDamage(damage);
    }

    protected void OnDisable()
    {
        gameplayStates.Paused -= OnGameplayPause;
        gameplayStates.Unpaused -= OnGameplayUnpause;
    }
}

public enum ProjectileUpgradeLevel
{
    EntryLevel,
    Upgraded,
    SecondaryUpgraded,
    ThriceUpgraded
}
