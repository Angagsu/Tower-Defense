using Assets.Scripts;
using UnityEngine;

public class MeleeAttack : BaseAttack
{
    public override void AttackTarget(Transform target, float damage, Character targetCharacter = null)
    {
        if (target.TryGetComponent<IAttackable>(out var enemy))
        {
            enemy.TakeDamage(damage);
        }
        else 
        {
            var monster = target.GetComponentInParent<IAttackable>();

            if (monster != null)
            {
                monster.TakeDamage(damage);
            }
        }
    }
}
