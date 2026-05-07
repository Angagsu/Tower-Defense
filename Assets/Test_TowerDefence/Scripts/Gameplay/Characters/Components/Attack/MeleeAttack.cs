using Assets.Scripts;
using UnityEngine;

public class MeleeAttack : BaseAttack
{
    public override void AttackTarget(AttackStats attackStats, Transform target, float damage, Character targetCharacter = null, IAttackable attackableTarget = null)
    {
        if (attackableTarget != null)
        {
            attackableTarget.TakeDamage(attackStats);
        }
        else if (targetCharacter != null)
        {
            targetCharacter.TakeDamage(attackStats);
        }
        else
        {
            Debug.Log("Target is missed !!!");
        }
    }

    public override void AttackWithPassiveSkill(AttackStats attackStats, Transform target, float damage, bool hasPassiveSkill = false, float passiveValue_1 = 0, float passiveValue_2 = 0, Character targetCharacter = null, IAttackable attackableTarget = null)
    {
        if (targetCharacter != null)
        {
            targetCharacter.TakeDamage(attackStats);
        }
        else
        {
            Debug.Log("Target is missed !!!");
        }
    }
}
