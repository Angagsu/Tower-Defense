using UnityEngine;

namespace Assets.Test_TowerDefence.Scripts.Monster.Captain
{
    public class SpeedsterCaptainMonster : CaptainMonster
    {
        protected override void Awake()
        {
            base.Awake();

            IsTowerAttacker = true;
            IsKamikaze = true;
        }

        protected override void DetectTarget()
        {
            if (target = detect.DetectTarget(detectionRange, IsDead))
            {

                Movement.LockOnTarget(target, turnSpeed);

                float sqrDistance = (transform.position - target.position).sqrMagnitude;

                if (sqrDistance <= attackRange * attackRange)
                {
                    Movement.SetIsMove(false);
                    Attack(target);

                    if (!isDead)
                    {
                        OnDie();
                    }
                }
            }
            else
            {
                anim.SetAttackAnimation(false);
                RejectCurrentAttackerHero();
            }

            attackCountdown -= Time.deltaTime;
        }

        protected override void Attack(Transform target)
        {
            attack.AttackTarget(AttackStats, target, AttackStats.DamageData.DamageAmount, CurrentTargetedHero, detect.AttackableTarget);
        }
    }
}