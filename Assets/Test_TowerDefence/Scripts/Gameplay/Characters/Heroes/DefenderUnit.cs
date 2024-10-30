using System.Collections;
using UnityEngine;

public class DefenderUnit : BaseHero
{
    [SerializeField] DefenderMovement defendersMove;
   

    protected override void Update()
    {
        if (!defendersMove.IsMoves && !IsDead)
        {
            DetectTarget();
        }

        if (!defendersMove.IsMoves && target)
        {
            LockOnTarget();
        }
    }

    protected override void DetectTarget()
    {
        base.DetectTarget();
    }

    protected override void Attack(Transform target)
    {
        base.Attack(target);
    }

    protected override void Move(Transform target)
    {
        
    }

    public override void OnRevive()
    {
        CurrentAttackerMonster = null;
        health = startHealth;
        healthBar.fillAmount = 1f;
        isDead = false;
        Anim.SetDeadAnimation(false);
    }

    protected override IEnumerator OnReviveAnimationEnded()
    {
        return base.OnReviveAnimationEnded();
    }

    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);
    }

    
}
