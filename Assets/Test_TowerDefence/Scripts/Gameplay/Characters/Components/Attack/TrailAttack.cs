using Assets.Scripts.Tower;
using UnityEngine;


public class TrailAttack : BaseAttack
{
    [SerializeField] private BaseTower baseTower;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private ParticleSystem lightningImpactEffect;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private Material lightningMaterial;
    [SerializeField] private float shockSlowAmount;
    [SerializeField] private float randomWidthOffsetMax = 4f;
    [SerializeField] private float randomWidthOffsetMin = 3f;

    private BaseMonster targetMonster;
    private BaseMonster previousTargetMonster;

    public BaseMonster SlowedMonster => targetMonster;


    private void Update()
    {
        if (GameController.IsGameOver)
        {
            enabled = false;

            if (lineRenderer.enabled)
            {
                lineRenderer.enabled = false;
                lightningImpactEffect.Stop();
            }
            return;
        }
    }

    public override void AttackTarget(Transform target, float damage)
    {
        LightningAttack(target, damage);
    }

    private void LightningAttack(Transform target, float damage)
    {
        if (!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
            lightningImpactEffect.Play();
            audioSource.Play();
        }

        previousTargetMonster = targetMonster;

        targetMonster.SetAttackedTower(baseTower);
        float damageMultiplier = damage * Time.deltaTime;
        targetMonster.TakeDamage(damageMultiplier);
        targetMonster.Slow(shockSlowAmount);

        lineRenderer.SetPosition(0, attackPoint.position);
        lineRenderer.SetPosition(1, target.localPosition);

        lightningMaterial.mainTextureOffset = new Vector2(Random.Range(0f, 1f), 0);
        lineRenderer.startWidth = RandomWidthOffset();
        lineRenderer.endWidth = RandomWidthOffset();

        Vector3 direction = attackPoint.position - target.position;
        lightningImpactEffect.transform.position = target.position + direction.normalized;
        lightningImpactEffect.transform.rotation = Quaternion.LookRotation(direction);
    }

    public void SetTargetMonsterDefaultSpeed(Transform target)
    {
        targetMonster = target.GetComponent<BaseMonster>();

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
            lineRenderer.enabled = false;
            lightningImpactEffect.Stop();
            audioSource.Pause();
        }
    }
}
