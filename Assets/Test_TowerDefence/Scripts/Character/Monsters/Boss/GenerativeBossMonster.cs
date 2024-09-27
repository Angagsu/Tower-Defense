using UnityEngine;

namespace Assets.Test_TowerDefence.Scripts.Monster.Boss
{
    public class GenerativeBossMonster : BossMonster
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
}