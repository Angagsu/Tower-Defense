using System.Collections;
using UnityEngine;

public class SuperAttackProjectile : LauncherProjectile
{
    [SerializeField] GameObject baseEffectObj;

    protected float superAttackDamage;
    

    public virtual void Setup(BaseHero baseHero, bool hasPassiveSkill)
    {
        this.attackStats = baseHero.AttackStats;
        this.hasPassiveSkill = hasPassiveSkill;
        superAttackDamage = baseHero.UpgradeLevel.ActiveSkillValue;

        if (hasPassiveSkill)
        {
            passiveSkill_1 = baseHero.UpgradeLevel.PassiveSkillsValues[0];
            passiveSkill_2 = baseHero.UpgradeLevel.PassiveSkillsValues[1];
        }
    }

    public void SetTarget(Vector3 targetPoint)
    {
        this.targetPoint = targetPoint;
        baseEffectObj.SetActive(true);
        Launch(transform.position, targetPoint);
    }

    public void Activate(Vector3 launchPosition)
    {
        transform.position = launchPosition;
        gameObject.SetActive(true);
    }

    protected override IEnumerator ImpactEffectDuration()
    {
        baseEffectObj.SetActive(false);

        yield return waitForSeconds;

        impactEffect.gameObject.SetActive(false);
        isHitted = false;

        gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
