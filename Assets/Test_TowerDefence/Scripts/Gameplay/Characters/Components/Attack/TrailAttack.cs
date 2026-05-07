using Assets.Scripts;
using Assets.Scripts.Tower;
using System.Collections;
using UnityEngine;


public class TrailAttack : BaseAttack
{
    public BaseMonster SlowedMonster => targetMonster;

    [SerializeField] private BaseTower baseTower;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private ParticleSystem lightningImpactEffect;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private Material lightningMaterial;
    [SerializeField] private float shockSlowAmount;
    [SerializeField] private float randomWidthOffsetMax = 2f;
    [SerializeField] private float randomWidthOffsetMin = 1f;
    [SerializeField] private float maxDamageThreshold;

    private BaseMonster targetMonster;
    private BaseMonster previousTargetMonster;
    private Coroutine coroutine;

    private float attackRate;
    private bool isAttacking;
    

    private void Awake()
    {
        DisableTrail();
    }

    public override void AttackTarget(AttackStats stats, Transform target, float damage, Character targetCharacter = null, IAttackable attackableTarget = null)
    {
        throw new System.NotImplementedException();
    }

    public override void AttackWithPassiveSkill(AttackStats stats, Transform target, float damage, bool hasPassiveSkill = false, float decelerationMultiplier = 0, float overTimeDamageMultiplier = 0, Character targetCharacter = null, IAttackable attackableTarget = null)
    {
        if (isAttacking)
        {
            return;
        }

        if (coroutine != null)
        {
            coroutine = null;
        }

        coroutine = StartCoroutine(AttackWithPassive(target, damage, hasPassiveSkill, decelerationMultiplier, overTimeDamageMultiplier, targetCharacter));
    }

    private IEnumerator AttackWithPassive(Transform target, float damage, bool hasPassiveSkill, float decelerationMultiplier = 0, float overTimeDamageMultiplier = 0, Character targetCharacter = null)
    {
        isAttacking = true;
        EnableTrail();

        previousTargetMonster = targetMonster;

        targetMonster.SetAttackedTower(baseTower);
        targetMonster.Slow(decelerationMultiplier);

        maxDamageThreshold = hasPassiveSkill? maxDamageThreshold : damage;

        float overTimeIncreasedDamage = 1;
        float damageMultiplier;
        float defaultDamageCoiffiecient = 1; 
        float timer = 1;
        float timeStep = 0;
        float finalDamage = 1;

        damageMultiplier = Mathf.Max(overTimeDamageMultiplier, defaultDamageCoiffiecient);

        while (targetMonster)
        {
            if(timeStep <= 0)
            {
                timeStep = attackRate;
                overTimeIncreasedDamage += (timer * damageMultiplier);
                finalDamage = Mathf.Clamp(overTimeIncreasedDamage, damage, maxDamageThreshold);

                finalDamage = DoDamage.CalculateDamage(targetMonster.AttackStats, baseTower.AttackStats.DamageData, finalDamage);

                targetMonster.TakePureDamage(finalDamage);
            }

            timer += Time.deltaTime;
            timeStep -= Time.deltaTime;

            lineRenderer.SetPosition(0, attackPoint.position);
            lineRenderer.SetPosition(1, targetMonster.PartForTargeting.position);

            lightningMaterial.mainTextureOffset = new Vector2(Random.Range(0f, 1f), 0);
            lineRenderer.startWidth = RandomWidthOffset();
            lineRenderer.endWidth = RandomWidthOffset();

            Vector3 direction = attackPoint.position - targetMonster.PartForTargeting.position;
            lightningImpactEffect.transform.position = targetMonster.PartForTargeting.position + direction.normalized;
            lightningImpactEffect.transform.rotation = Quaternion.LookRotation(direction);

            yield return null;
        }

        isAttacking = false;
    }

    public void SetTargetMonsterDefaultSpeed(BaseMonster target)
    {
        targetMonster = target;

        if (previousTargetMonster && previousTargetMonster != targetMonster)
        {
            previousTargetMonster.SetDefaultMoveSpeed();
        }
    }

    private float RandomWidthOffset()
    {
        return Random.Range(randomWidthOffsetMin, randomWidthOffsetMax);
    }

    public void DisableTrail()
    {
        if (lineRenderer.enabled)
        {
            targetMonster = null;
            lineRenderer.enabled = false;
            lightningImpactEffect.Stop();
            audioSource.Pause();
        }
    }

    private void EnableTrail()
    {
        if (!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
            lightningImpactEffect.Play();
            audioSource.Play();
        }
    }

    public void SetAttackRate(float rate)
    {
        attackRate = rate;
    }
}
