using Assets.Scripts.Tower;
using UnityEngine;

public class FireTower : BaseTower
{
    [SerializeField] private float impactRadius;

    private float periodicDamage;
    private float periodicDamageDuration;
    private float[] passiveSkills;


    public override void SetParams(TowerSO towerSO)
    {
        base.SetParams(towerSO);

        passiveSkills = new float[] { periodicDamage, periodicDamageDuration };
        passiveSkillsList = new();
        passiveSkillsList.Add(periodicDamage);
        passiveSkillsList.Add(periodicDamageDuration);

        if (hasPassiveSkill)
        {
            for (int i = 0; i < upgradeLevel.PassiveSkillsAccessibility.Count; i++)
            {
                passiveSkillsList[i] = upgradeLevel.PassiveSkillsValues[i];
            }
        }
    }

    protected override void Update()
    {
        base.Update();
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
