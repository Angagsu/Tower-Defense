using Assets.Scripts;
using System.Collections;
using UnityEngine;

public class ArrowProjectile : BaseProjectile
{
    [SerializeField] private MeshRenderer meshRenderer;

    private float criticalDamage;
    private float criticalDamageChance;
    private bool isCriticalHit;
    private float crit;


    public override void SetTarget(AttackStats attackStats, Character targetCharacter, Transform target, float damage, bool hasPassiveSkill, float passiveSkill_1, float passiveSkill_2, IAttackable attackableTarget = null)
    {
        base.SetTarget(attackStats, targetCharacter, target, damage, hasPassiveSkill, passiveSkill_1, passiveSkill_2, attackableTarget);
        criticalDamage = passiveSkill_1 / 100;
        criticalDamageChance = passiveSkill_2 / 100;
    }

    protected override void DoDamageWithPassiveSkill(AttackStats attackStats, Character targetCharacter, Transform target, float damage, float passiveSkill_1, float passiveSkill_2)
    {
        float critDamage = CalculateCriticalDamage(passiveSkill_1, passiveSkill_2);

        critDamage = DoDamage.CalculateDamage(targetCharacter.AttackStats, attackStats.DamageData, critDamage);

        targetCharacter.TakePureDamage(critDamage);
    }

    private float CalculateCriticalDamage(float criticalDamage, float criticalDamageChance)
    {      
        isCriticalHit = Random.value <= this.criticalDamageChance;

        if (isCriticalHit)
        {
            crit = attackStats.DamageData.DamageAmount * this.criticalDamage;
            Debug.Log($"Critical Hit! Damage: {crit}");
            return crit;
        }
        else
        {
            return attackStats.DamageData.DamageAmount;
        }
    }

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
