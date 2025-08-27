using UnityEngine;


namespace Assets.Scripts.Hero
{
    public class ArcherHero : BaseHero
    {
        protected override void Update()
        {
            base.Update();
        }

        protected override void DetectTarget()
        {
            if ((target = detect.DetectTarget(detectionRange, IsDead)))
            {
                var distance = Vector3.Distance(movement.PreviousPosition, target.position);

                if (distance < detectionRange && !movement.IsMoves)
                {
                    currentTargetedMonster = detect.DetectedMonster;

                    if (currentTargetedMonster.CurrentAttackerHero == null && currentTargetedMonster.AttackRange >= distance)
                    {
                        currentTargetedMonster.SetCurrentAttackerHero(this);
                        currentTargetedMonster.Movement.SetIsMove(false); 
                        currentTargetedMonster.Anim.StopAllAnimations();
                        movement.MoveToTargetMonster(moveSpeed, turnSpeed, target.position);
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

        protected override void Attack(Transform target)
        {
            base.Attack(target);
        }

        protected override void Move(Transform target)
        {
            base.Move(target);
        }

        public override void OnRevive()
        {
            base.OnRevive();
        }

        public override void TakeDamage(float amount)
        {
            base.TakeDamage(amount);
        }
    }
}