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
        
        if (canDetect)
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
}
