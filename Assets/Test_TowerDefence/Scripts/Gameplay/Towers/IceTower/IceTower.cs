using UnityEngine;

namespace Assets.Scripts.Tower
{
    public class IceTower : BaseTower
    {
        private TrailAttack trailAttack;

        private float decelerationMultiplier;
        private float overTimeDamageMultiplier;
        private float[] passiveSkills;



        protected override void Awake()
        {
            trailAttack = attack as TrailAttack;
        }

        protected override void Update()
        {
            base.Update();
        }

        public override void SetParams(TowerSO towerSO)
        {
            base.SetParams(towerSO);

            passiveSkills = new float[] { decelerationMultiplier, overTimeDamageMultiplier };
            passiveSkillsList = new();
            passiveSkillsList.Add(decelerationMultiplier);
            passiveSkillsList.Add(overTimeDamageMultiplier);

            if (hasPassiveSkill)
            {
                for (int i = 0; i < upgradeLevel.PassiveSkillsAccessibility.Count; i++)
                {
                    passiveSkillsList[i] = upgradeLevel.PassiveSkillsValues[i];
                }
            }

            trailAttack.SetAttackRate(attackRate);
        }

        public override void Repair()
        {
            base.Repair();
        }

        protected override void AttackTarget(Transform target)
        {
            attack.AttackWithPassiveSkill(AttackStats, target, damage, hasPassiveSkill, passiveValue_1: passiveSkillsList[0], passiveValue_2: passiveSkillsList[1], TargetedMonster);
        }

        protected override void Crash()
        {
            base.Crash();
            trailAttack.DisableTrail();
        }

        protected override void DetectTarget()
        {
            if (target = detect.DetectTarget(attackRange, IsCrashed))
            {
                targetedMonster = detect.DetectedMonster;
                trailAttack.SetTargetMonsterDefaultSpeed(TargetedMonster);

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