using Assets.Scripts;
using Assets.Scripts.Tower;
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

    private BaseMonster targetMonster;
    private BaseMonster previousTargetMonster;



    private void Awake()
    {
        DisableTrail();
    }

    public override void AttackTarget(Transform target, float damage, Character targetCharacter = null)
    {
        LightningAttack(target, damage);
    }

    private void LightningAttack(Transform target, float damage)
    {
        EnableTrail();

        previousTargetMonster = targetMonster;

        targetMonster.SetAttackedTower(baseTower);
        float damageMultiplier = damage * Time.deltaTime;
        targetMonster.TakeDamage(damageMultiplier);
        targetMonster.Slow(shockSlowAmount);

        lineRenderer.SetPosition(0, attackPoint.position);
        lineRenderer.SetPosition(1, targetMonster.PartForTargeting.position);
        
        lightningMaterial.mainTextureOffset = new Vector2(Random.Range(0f, 1f), 0);
        lineRenderer.startWidth = RandomWidthOffset();
        lineRenderer.endWidth = RandomWidthOffset();

        Vector3 direction = attackPoint.position - targetMonster.PartForTargeting.position;
        lightningImpactEffect.transform.position = targetMonster.PartForTargeting.position + direction.normalized;
        lightningImpactEffect.transform.rotation = Quaternion.LookRotation(direction);
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
}
