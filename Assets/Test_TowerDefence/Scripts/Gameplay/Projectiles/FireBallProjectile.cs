using Assets.Scripts;
using System.Collections;
using UnityEngine;

public class FireBallProjectile : BaseProjectile
{
    private float periodicDamage;
    private float periodicDamageChance;


    public override void SetTarget(AttackStats attackStats, Character targetCharacter, Transform target, float damage, bool hasPassiveSkill, float passiveSkill_1, float passiveSkill_2, IAttackable attackableTarget = null)
    {
        base.SetTarget(attackStats, targetCharacter, target, damage, hasPassiveSkill, passiveSkill_1, passiveSkill_2, attackableTarget);
 
        periodicDamage = passiveSkill_1 / 100;
        periodicDamageChance = passiveSkill_2 / 100;
    }

    protected override void DoDamageWithPassiveSkill(AttackStats attackStats, Character targetCharacter, Transform target, float damage, float passiveSkill_1, float passiveSkill_2)
    {
        base.DoDamageWithPassiveSkill(attackStats, targetCharacter, target, damage, passiveSkill_1, passiveSkill_2);
        targetCharacter.TakePeriodicDamage(attackStats, passiveSkill_1, passiveSkill_2);
    }

    protected override IEnumerator ImpactEffectDuration()
    {
        yield return waitForSeconds;

        impactEffect.gameObject.SetActive(false);
        isHitted = false;
        gameObject.SetActive(false);
    }
}
