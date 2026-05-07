using Assets.Scripts;
using UnityEngine;

public class MeleeAttackDefender : MeleeAttack
{
    private float stunChance;
    private float defaultStunDuration = 1f;
    private bool isStunned;


    public override void AttackWithPassiveSkill(AttackStats stats, Transform target, float damage, bool hasPassiveSkill = false, float stunChance = 0, float stunDuration = 0, Character targetCharacter = null, IAttackable attackableTarget = null)
    {
        if (hasPassiveSkill)
        {
            this.stunChance = stunChance / 100;
            if (stunDuration == 0)
            {
                stunDuration = defaultStunDuration;
            }

            isStunned = Random.value <= this.stunChance;

            if (isStunned)
            {
                targetCharacter.TakeDamage(stats);
                targetCharacter.GetStun(stunDuration);
            }
            else
            {
                targetCharacter.TakeDamage(stats);
            }
        }
        else
        {
            targetCharacter.TakeDamage(stats);
        }
    }
}
