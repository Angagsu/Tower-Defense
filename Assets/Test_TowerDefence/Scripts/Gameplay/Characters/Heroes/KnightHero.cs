using System;
using UnityEngine;

namespace Assets.Scripts.Hero
{
    public class KnightHero : BaseHero
    {
        private float blastAttack;
        private float stunAttack;



        public override void SetHeroParams(HeroSO heroSO)
        {
            base.SetHeroParams(heroSO);

            passiveSkillsList = new()
            {
                blastAttack,
                stunAttack
            };

            if (hasPassiveSkill)
            {
                for (int i = 0; i < upgradeLevel.PassiveSkillsAccessibility.Count; i++)
                {
                    passiveSkillsList[i] = upgradeLevel.PassiveSkillsValues[i];
                }
            }
        }

        public override void SuperAttack(Action skillCooldown)
        {
            if (movement.IsMoves) return;

            Debug.Log($"Hero: {this} Super Attack");
            superAttack.AreaEffect(AttackStats, superAttackRadius, superAttackEffectDuration);
            skillCooldown?.Invoke();
        }
 
        protected override void Awake()
        {
            base.Awake();
        }

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

        public override void OnRevive()
        {
            base.OnRevive();
        }
    }
}