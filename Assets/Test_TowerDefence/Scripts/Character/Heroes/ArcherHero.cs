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
            base.DetectTarget();
        }

        protected override void Attack(Transform target)
        {
            base.Attack(target);
        }

        protected override void Move(Transform target)
        {
            base.Move(target);
        }

        public override void Revive()
        {
            base.Revive();
        }

        public override void TakeDamage(float amount)
        {
            base.TakeDamage(amount);
        }
    }
}