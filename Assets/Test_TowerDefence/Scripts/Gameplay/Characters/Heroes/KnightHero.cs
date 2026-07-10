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

        public override void SuperAttack()
        {
            if (movement.IsMoves) return;

            if (!activeSkillUI.IsSelected)
            {
                activeSkillUI.Select();
                superAttack.SetIsSuperAttackStarted(true);
                superAttack.OnSuperAttackPerformed(activeSkillUI);

                Anim.SetAllAnimationsFalse();
                canAttack = false;
            }
            else
            {
                Anim.SetSupperAttackAnimation(true);
            }
        }

        public override void OnSuperAttackAnimationTriggerStart()
        {
            canDetect = false;
            canMove = false;
 
            superAttack.AreaEffect(AttackStats, superAttackRadius, superAttackEffectDuration);
            activeSkillUI.StartCooldown();
            activeSkillUI.Deselect();
        }

        public override void OnSuperAttackAnimationTriggerEnd()
        {
            Anim.SetSupperAttackAnimation(false);
            canAttack = true;
            canDetect = true;
            canMove = true;
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