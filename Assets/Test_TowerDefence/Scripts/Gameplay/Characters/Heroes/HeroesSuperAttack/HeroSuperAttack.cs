using System;
using System.Collections.Generic;
using UnityEngine;

public class HeroSuperAttack : MonoBehaviour
{
    protected PlayerInputHandler playerInputHandler;
    protected DetectionHelper detectionHelper;
 
    protected List<BaseHero> heroes;
    protected List<BaseHero> affectableHeroes = new();

    protected List<BaseMonster> monsters;
    protected List<BaseMonster> affectableMonsters = new();

    protected BaseHero baseHero;
    protected bool isSuperAttackStarted;
    protected float superAttackValue;

    public virtual void SetDependencies(BaseHero baseHero, PlayerInputHandler playerInputHandler)
    {
        this.baseHero = baseHero;
        this.playerInputHandler = playerInputHandler;
        detectionHelper = DetectionHelper.Instance;
        monsters = detectionHelper.Monsters;
        heroes = detectionHelper.Heroes;

        superAttackValue = baseHero.UpgradeLevel.ActiveSkillValue;
    }

    public virtual void OnSuperAttackPerformed(GameplayHeroActiveSkillUI activeSkillUI = null) { }

    public virtual void AreaEffect(AttackStats stats, float areaRadius, float passiveEffectDuration = 0, float passiveEffect_2 = 0) { }

    protected virtual void ApplyMassiveBuff(AttackStats stats, float areaRadius, float effectDuration, float effectValue) { }
    
    protected virtual void ApplyMassiveDamage(AttackStats attackStats, float areaRadius, float stunDuration, float damage) { }

    protected void DetectMonsters(float detectionRadius)
    {
        foreach (var monster in monsters)
        {
            float sqrDistanceToEnemy = (transform.position - monster.transform.position).sqrMagnitude;

            if (sqrDistanceToEnemy <= (detectionRadius * detectionRadius))
            {
                affectableMonsters.Add(monster);
            }
        }
    }

    protected void DetectHeroes(float detectionRadius)
    {
        foreach (var hero in heroes)
        {
            if (!hero.IsDead)
            {
                float sqrDistanceToEnemy = (transform.position - hero.transform.position).sqrMagnitude;

                if (sqrDistanceToEnemy <= (detectionRadius * detectionRadius))
                {
                    affectableHeroes.Add(hero);
                }
            }
        }
    }

    public void SetIsSuperAttackStarted(bool isSuperAttackStarted) => this.isSuperAttackStarted = isSuperAttackStarted; 
}
