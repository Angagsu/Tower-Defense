using System;
using UnityEngine;

public class MageHero : BaseHero
{
    private float periodicDamage;
    private float periodicDamageDuration;


    public override void SetHeroParams(HeroSO heroSO)
    {
        base.SetHeroParams(heroSO);

        passiveSkillsList = new()
        {
            periodicDamage,
            periodicDamageDuration
        };

        if (hasPassiveSkill)
        {
            for (int i = 0; i < upgradeLevel.PassiveSkillsAccessibility.Count; i++)
            {
                passiveSkillsList[i] = upgradeLevel.PassiveSkillsValues[i];
            }
        }
    }

    protected override void DetectTarget()
    {
        if ((target = detect.DetectTarget(detectionRange, IsDead)))
        {
            var distance = (movement.PreviousPosition - target.position).sqrMagnitude;

            if (distance < (detectionRange * detectionRange) && !movement.IsMoves)
            {
                currentTargetedMonster = detect.DetectedMonster;

                if(currentTargetedMonster.CurrentAttackerHero == null || currentTargetedMonster.CurrentAttackerHero == this)
                {
                    currentTargetedMonster.SetCurrentAttackerHero(this);
                }

                if (!currentTargetedMonster.IsKamikaze && currentTargetedMonster.IsMoving && (currentTargetedMonster.AttackRange * currentTargetedMonster.AttackRange) >= distance)
                {
                    currentTargetedMonster.SetCurrentTargetedHero();
                    currentTargetedMonster.Movement.SetIsMove(false);
                    currentTargetedMonster.Anim.StopAllAnimations();
                    movement.MoveToTargetMonster(currentTargetedMonster, moveSpeed, turnSpeed, target.position);
                }

                LockOnTarget();

                if (canAttack)
                {
                    Anim.SetAttackAnimation(true);

                    if (attackCountdown <= 0)
                    {
                        attackCountdown = 1 / attackRate;

                        Attack(target);
                    }
                }
                else
                {
                    currentTargetedMonster = null;
                    Anim.SetAttackAnimation(false);
                }
            }
            else
            {
                if (currentTargetedMonster != null)
                {
                    currentTargetedMonster.RejectCurrentAttackerHero();
                }

                currentTargetedMonster = null;
                Anim.SetAttackAnimation(false);

                if (!movement.IsMoves && !movement.IsOnPreviosPosition)
                {
                    movement.ReturnToPreviousPosition(moveSpeed, turnSpeed, movement.PreviousPosition);
                }
            }
        }
        else
        {
            if (currentTargetedMonster != null)
            {
                currentTargetedMonster.RejectCurrentAttackerHero();
            }

            currentTargetedMonster = null;
            Anim.SetAttackAnimation(false);

            if (!movement.IsMoves && !movement.IsOnPreviosPosition)
            {
                movement.ReturnToPreviousPosition(moveSpeed, turnSpeed, movement.PreviousPosition);
            }
        }

        attackCountdown -= Time.deltaTime;
    }

    public override void SuperAttack(Action skillCooldown)
    {
        if (movement.IsMoves) return;

        superAttack.SetIsSuperAttackStarted(true);
        superAttack.OnSuperAttackPerformed(skillCooldown);
    }
}
