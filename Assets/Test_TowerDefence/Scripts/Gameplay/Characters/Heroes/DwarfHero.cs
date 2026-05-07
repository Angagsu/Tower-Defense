using System;
using UnityEngine;

public class DwarfHero : BaseHero
{
    [SerializeField] DwarfHeroAreaPenetration passivePenetration;
    private float physicalPenetration;
    private float impactRadius;



    protected override void Awake()
    {
        base.Awake();       
    }

    protected override void Update()
    {
        if (isPaused) return;
        
        if ((!move.IsMoves && !IsDead))
        {
            DetectTarget();
        }
        else if (currentTargetedMonster)
        {
            target = null;
            currentTargetedMonster.RejectCurrentAttackerHero();
            currentTargetedMonster = null;
        }
    }

    public override void SetHeroParams(HeroSO heroSO)
    {
        base.SetHeroParams(heroSO);

        passiveSkillsList = new()
        {
            physicalPenetration,
            impactRadius
        };

        if (hasPassiveSkill)
        {
            for (int i = 0; i < upgradeLevel.PassiveSkillsAccessibility.Count; i++)
            {
                passiveSkillsList[i] = upgradeLevel.PassiveSkillsValues[i];
            }

            passivePenetration.SetHeroStats(this, upgradeLevel.PassiveSkillsValues[0], upgradeLevel.PassiveSkillsValues[1]);
        }
        else
        {
            passivePenetration.enabled = false;
        } 
    }

    protected override void Attack(Transform target)
    {
        baseSFX.PlayAttackSFX();
        attack.AttackTarget(AttackStats,target, AttackStats.DamageData.DamageAmount, currentTargetedMonster);
    }

    public override void SuperAttack(Action skillCooldown)
    {
        if (movement.IsMoves) return;

        Debug.Log($"Hero: {this} Super Attack");
        superAttack.AreaEffect(AttackStats, superAttackRadius, superAttackEffectDuration);
        skillCooldown?.Invoke();
    }
}
