using UnityEngine;

public class MeleeAttack : BaseAttack
{
    public override void AttackTarget(Transform target, float damage)
    {
        if (target.TryGetComponent<IAttackable>(out var enemy))
        {
            enemy.TakeDamage(damage);
        }
    }
}
