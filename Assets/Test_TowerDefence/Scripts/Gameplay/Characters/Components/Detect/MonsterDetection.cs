using System.Collections.Generic;
using UnityEngine;

public class MonsterDetection : BaseDetection
{
    [SerializeField] private BaseMonster baseMonster;

    private Transform target;

    private List<BaseHero> heroes = new();

    private DetectionHelper detectionHelper;



    private void Awake()
    {
        detectionHelper = DetectionHelper.Instance;
        heroes = detectionHelper.Heroes;
    }

    public override Transform DetectTarget(float attackRange, bool isDead)
    {
        float shortestDistance = Mathf.Infinity;
        Transform nearestHero = null;
        
        foreach (BaseHero hero in heroes)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, hero.transform.position);

            if (distanceToEnemy < shortestDistance && hero != null && !hero.IsDead)
            {
                shortestDistance = distanceToEnemy;
                nearestHero = hero.transform;

                if (nearestHero != null && shortestDistance <= attackRange && !isDead)
                {
                    if (hero.CurrentAttackerMonster == null || hero.CurrentAttackerMonster == baseMonster)
                    {
                        hero.SetCurrentAttackerMonster(baseMonster);
                        DetectedHero = hero;
                        baseMonster.SetIsMoves(false);
                        target = hero.PartForTargeting;
                        return target;
                    }
                    else
                    {
                        baseMonster.SetIsMoves(true);
                        target = null;
                        return target;
                    }
                } 
            }
        }

        return null;
    }
}
