using UnityEngine;

namespace Assets.Scripts.Tower
{
    public class ArrowTower : BaseTower
    {
        private float criticalDamage;
        private float criticalDamageChance;
        private float[] passiveSkills;
        
 
        public override void SetParams(TowerSO towerSO)
        {
            base.SetParams(towerSO);

            passiveSkills = new float[] { criticalDamage, criticalDamageChance };

            passiveSkillsList = new()
            {
                criticalDamage,
                criticalDamageChance
            };

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
            base.AttackTarget(target);
        }

        protected override void Crash()
        {
            base.Crash();
        }

        protected override void DetectTarget()
        {
            base.DetectTarget();
        }
    }
}