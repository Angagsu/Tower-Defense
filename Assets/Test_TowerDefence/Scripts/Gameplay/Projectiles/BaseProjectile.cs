using Assets.Scripts;
using System.Collections;
using UnityEngine;

public class BaseProjectile : MonoBehaviour
{
    public ProjectileType UpgradeLevel => upgradLevel;

    [SerializeField] protected float speed = 70f;
    [SerializeField] protected float explosionRadius = 0f;
    [SerializeField] protected ParticleSystem impactEffect;
    [SerializeField] protected float impactEffectDuration = 0.2f;
    [Space(15)]
    [SerializeField] protected ProjectileType upgradLevel;

    protected GameplayStates gameplayStates;
    protected bool isHitted = false;
    protected Transform target;
    protected Coroutine coroutine;
    protected Character targetCharacter;
    protected IAttackable attackableTarget;
    protected float damage;

    protected bool isPaused;
    protected bool hasPassiveSkill;
    protected float passiveSkill_1;
    protected float passiveSkill_2;

    protected readonly Collider[] overlapResults = new Collider[32];
    protected AttackStats attackStats;
    protected WaitForSeconds waitForSeconds;
    


    public virtual void Construct(GameplayStates gameplayStates)
    {
        this.gameplayStates = gameplayStates;

        this.gameplayStates.Paused += OnGameplayPause;
        this.gameplayStates.Unpaused += OnGameplayUnpause;
    }

    public virtual void SetTarget(AttackStats attackStats, Character targetCharacter, Transform target, float damage, IAttackable attackableTargert = null)
    {
        this.target = target;
        this.damage = damage;
        this.attackableTarget = attackableTargert;
        this.targetCharacter = targetCharacter;
        this.attackStats = attackStats;
    }

    public virtual void SetTarget(AttackStats attackStats, Character targetCharacter, Transform target, float damage, bool hasPassiveSkill, float passiveSkill_1, float passiveSkill_2, IAttackable attackableTarget = null)
    {
        this.target = target;
        this.damage = damage;
        this.attackableTarget = attackableTarget;
        this.targetCharacter = targetCharacter;
        this.attackStats = attackStats;

        if (hasPassiveSkill)
        {
            this.hasPassiveSkill = hasPassiveSkill;
            this.passiveSkill_1 = passiveSkill_1;
            this.passiveSkill_2 = passiveSkill_2;
        }     
    }

    private void OnGameplayPause() => isPaused = true;

    private void OnGameplayUnpause() => isPaused = false;


    protected virtual void Start()
    {
        waitForSeconds = new WaitForSeconds(impactEffectDuration);
    }

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

            if (hasPassiveSkill)
            {
                DoDamageWithPassiveSkill(attackStats, targetCharacter, target, damage, passiveSkill_1, passiveSkill_2);
            }
            else
            {
                ApplyDamage(attackStats);
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
        int hitCount = Physics.OverlapSphereNonAlloc(transform.position, explosionRadius, overlapResults);

        for (int i = 0; i < hitCount; i++)
        {
            var collider = overlapResults[i];

            if (collider != null && collider.gameObject.TryGetComponent<BaseMonster>(out var monster))
            {
                monster.TakeDamage(attackStats);
            }
        }
    }

    protected virtual void ApplyDamage(AttackStats attackStats)
    {
        attackableTarget?.TakeDamage(attackStats);   
    }

    protected virtual void DoDamageWithPassiveSkill(AttackStats attackStats, Character targetCharacter, Transform target, float damage, float passiveSkill_1, float passiveSkill_2)
    {
        attackableTarget?.TakeDamage(attackStats);
    }

    protected void OnDisable()
    {
        gameplayStates.Paused -= OnGameplayPause;
        gameplayStates.Unpaused -= OnGameplayUnpause;
    }
}

public enum ProjectileType
{
    EntryLevel,
    Upgraded,
    SecondaryUpgraded,
    ThriceUpgraded,
    Monster
}
