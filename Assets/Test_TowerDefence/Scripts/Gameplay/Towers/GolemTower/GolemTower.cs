using UnityEngine;

namespace Assets.Scripts.Tower
{
    public class GolemTower : BaseTower
    {
        [SerializeField] private BaseTowerAnimation anim;
        private float blastRadius;
        private float stunDuration;
        private float[] passiveSkills;
        private float animationSpeed;
        private bool isRightHandAttack;

        public override void SetParams(TowerSO towerSO)
        {
            base.SetParams(towerSO);

            
            attackCountdown = attackRate;
            animationSpeed = 1 / attackRate;
            anim.SetAttackAnimationsSpeed(animationSpeed);

            passiveSkills = new float[] { blastRadius, stunDuration };
            passiveSkillsList = new();
            passiveSkillsList.Add(blastRadius);
            passiveSkillsList.Add(stunDuration);

            if (hasPassiveSkill)
            {
                for (int i = 0; i < upgradeLevel.PassiveSkillsAccessibility.Count; i++)
                {
                    passiveSkillsList[i] = upgradeLevel.PassiveSkillsValues[i];
                }
            }
        }

        public override void Repair()
        {
            base.Repair();
        }

        protected override void AttackTarget(Transform target)
        {
            attack.AttackWithPassiveSkillAndDifferentHands(AttackStats, target, damage, hasPassiveSkill,
                passiveValue_1: passiveSkillsList[0], passiveValue_2: passiveSkillsList[1], TargetedMonster, isRightHandAttack, TargetedMonster);
        }

        protected override void Crash()
        {
            base.Crash();
            anim.SetAllAnimationsFalse();
        }

        // attack starts from animation event
        public void GolemAttack()
        {
            if (!target) return;
            AttackTarget(TargetedMonster.transform);
        }
        protected override void DetectTarget()
        {
            if ((target = detect.DetectTarget(attackRange, IsCrashed)))
            {
                LockOnTarget();

                targetedMonster = detect.DetectedMonster;

                if (attackCountdown <= 0)
                {
                    attackCountdown = attackRate;

                    if (!isRightHandAttack)
                    {
                        isRightHandAttack = true;
                        anim.SetRightHandAttackAnimation(true);
                        anim.SetLeftHandAttackAnimation(false);
                    }
                    else
                    {
                        isRightHandAttack = false;
                        anim.SetRightHandAttackAnimation(false);
                        anim.SetLeftHandAttackAnimation(true);
                    }
                }
            }
            else
            {
                isRightHandAttack = false;
                anim.SetRightHandAttackAnimation(false);
                anim.SetLeftHandAttackAnimation(false);
            }

            attackCountdown -= Time.deltaTime;
        }


        protected override void OnGameplayPause()
        {
            isPaused = true;
            anim.Pause();
        }

        protected override void OnGameplayPlay()
        {
            isPaused = false;
            anim.Unpause();
            anim.SetAnimationSpeed(animationSpeed); 
        }
    }
}