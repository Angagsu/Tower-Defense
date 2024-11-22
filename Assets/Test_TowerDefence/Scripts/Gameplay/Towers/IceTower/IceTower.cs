using UnityEngine;

namespace Assets.Scripts.Tower
{
    public class IceTower : BaseTower
    {
        private TrailAttack trailAttack;

        protected override void Awake()
        {
            trailAttack = attack as TrailAttack;
            base.Awake();
        }
        protected override void Update()
        {
            base.Update();
        }

        public override void ReBuild()
        {
            base.ReBuild();
        }

        public override void TakeDamage(float damageAmount)
        {
            base.TakeDamage(damageAmount);
        }

        protected override void AttackTarget(Transform target)
        {
            attack.AttackTarget(target, damage);
        }

        protected override void Crash()
        {
            base.Crash();
        }

        protected override void DetectTarget()
        {
            if (target = detect.DetectTarget(attackRange, IsDead))
            {
                trailAttack.SetTargetMonsterDefaultSpeed(target);
                //LockOnTarget();
                AttackTarget(target);
            }
            else
            {
                trailAttack.DisableTrail();
            }
        }

    }
}