using Assets.Scripts.Tower;
using UnityEngine;

namespace Assets.Test_TowerDefence.Scripts.Monster.Captain
{
    public class StrongAttackerCaptainMonster : CaptainMonster
    {
        private IAttackable attackable;

        protected override void Awake()
        {
            base.Awake();

            IsTowerAttacker = true;
            IsKamikaze = false;
        }

        protected override void DetectTarget()
        {
            attackCountdown -= Time.deltaTime;

            if (currentTargetedHero)
            {
                if (detect.DetectedTower)
                {
                    detect.DetectedTower.ClearCurrentAttackerMonster(this);
                    detect.DetectedTower = null;
                }
                
                target = currentTargetedHero.PartForTargeting;
                attackable = currentTargetedHero;
                Movement.LockOnTarget(target, turnSpeed);
            
                if (attackCountdown <= 0 && currentTargetedHero.CanAttack)
                {
                    attackCountdown = attackRate;
            
                    Attack(target);
                    anim.SetAttackAnimation(true);
                }
            
                
                anim.SetMoveAnimation(false);
                return;
            }
            else
            {
                anim.SetAttackAnimation(false);
                target = null;
            }

            if (target = detect.DetectTarget(detectionRange, IsDead))
            {
                attackable = detect.AttackableTarget;
                Movement.LockOnTarget(target, turnSpeed);

                if (attackCountdown <= 0)
                {
                    attackCountdown = attackRate;

                    Attack(target);
                }

                anim.SetAttackAnimation(true);
                anim.SetMoveAnimation(false);
            }
            else
            {
                anim.SetAttackAnimation(false);
                anim.SetMoveAnimation(true);
            }
           
            
        }

        protected override void Attack(Transform target)
        {
            attack.AttackTarget(AttackStats, target, AttackStats.DamageData.DamageAmount, CurrentTargetedHero, attackable);
        }

        public override void TakePureDamage(float amount)
        {
            base.TakePureDamage(amount);
        }
    }
}