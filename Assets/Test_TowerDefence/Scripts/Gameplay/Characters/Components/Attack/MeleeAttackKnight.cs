using Assets.Scripts;
using UnityEngine;

public class MeleeAttackKnight : MeleeAttack
{
    protected readonly Collider[] overlapResults = new Collider[16];

    private float blastAttackChance;
    private float defaultStunDuration = 5f;
    private float blastAttackRadius = 4f;
    private bool isBlastAttacked;
    private Transform target;
    


    public override void AttackWithPassiveSkill(AttackStats attackStats,Transform target, float damage, bool hasPassiveSkill = false, float blastChance = 0, float stunDuration = 0f, Character targetCharacter = null, IAttackable attackableTarget = null)
    {
        if (hasPassiveSkill)
        {
            this.target = target;
            blastAttackChance = blastChance / 100;

            if (stunDuration == 0)
            {
                stunDuration = defaultStunDuration;
            }

            isBlastAttacked = Random.value <= blastAttackChance;

            if (isBlastAttacked)
            {
                BlastAttack(attackStats, target, damage, stunDuration);
            }
            else
            {
                targetCharacter.TakeDamage(attackStats);
            }
        }
        else
        {
            targetCharacter.TakeDamage(attackStats);
        }
    }

    private void BlastAttack(AttackStats attackStats, Transform target, float damage, float stunDuration)
    {
        int hitCount = Physics.OverlapSphereNonAlloc(target.position, blastAttackRadius, overlapResults);

        for (int i = 0; i < hitCount; i++)
        {
            var collider = overlapResults[i];

            if (collider != null && collider.gameObject.TryGetComponent<BaseMonster>(out var monster))
            {
                monster.TakeDamage(attackStats);
                monster.GetStun(stunDuration);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!target) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(target.position, blastAttackRadius);
    }
}
