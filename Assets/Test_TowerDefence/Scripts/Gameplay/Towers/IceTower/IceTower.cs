using UnityEngine;

namespace Assets.Scripts.Tower
{
    public class IceTower : BaseTower
    {
        private TrailAttack trailAttack;

        protected override void Awake()
        {
            trailAttack = attack as TrailAttack;
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
                targetedMonster = detect.DetectedMonster;
                trailAttack.SetTargetMonsterDefaultSpeed(TargetedMonster);

                //LockOnTarget();

                AttackTarget(target);
            }
            else
            {
                targetedMonster = null;
                trailAttack.DisableTrail();
            }
        }

        protected override void OnGameplayPause()
        {
            base.OnGameplayPause();
            trailAttack.DisableTrail();
        }
    }
}