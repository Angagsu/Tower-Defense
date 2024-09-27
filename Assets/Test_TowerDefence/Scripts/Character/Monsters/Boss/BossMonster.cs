using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonster : BaseMonster
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void Attack(Transform target)
    {
        base.Attack(target);
    }

    protected override void DetectTarget()
    {
        base.DetectTarget();
    }

    protected override void Die()
    {
        base.Die();
    }

    protected override void Move(Transform target)
    {
        base.Move(target);
    }

    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);
    }
}
