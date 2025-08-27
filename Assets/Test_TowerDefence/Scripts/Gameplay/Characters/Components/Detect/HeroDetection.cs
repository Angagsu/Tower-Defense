using System.Collections.Generic;
using UnityEngine;

public class HeroDetection : BaseDetection
{
    private Transform target;

    private List<BaseMonster> monsters = new();

    private DetectionHelper detectionHelper;

    public BaseHero hero;

    private void Start()
    {
        detectionHelper = DetectionHelper.Instance;
        monsters = detectionHelper.Monsters;
    }

    public override Transform DetectTarget(float detectionRange, bool isDead)
    {
        float shortestDistance = Mathf.Infinity;

        if (DetectedMonster != null && DetectedMonster.IsDead == false && DetectedMonster.CurrentAttackerHero == hero)
        {
            if (Vector3.Distance(transform.position, DetectedMonster.transform.position) < detectionRange)
            {
                
                return target = DetectedMonster.PartForTargeting;
            }
        }
        else
        {
            DetectedMonster = null;
            target = null;
        }

        foreach (BaseMonster monster in monsters)
        {
            if (monster.isActiveAndEnabled)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, monster.transform.position);

                if (distanceToEnemy < shortestDistance && monster && !monster.IsDead)
                {
                    shortestDistance = distanceToEnemy;
                    DetectedMonster = monster;
                }
            }
        }

        if (DetectedMonster != null && shortestDistance <= detectionRange && DetectedMonster.CurrentAttackerHero == null)
        {
            target = DetectedMonster.PartForTargeting;
            return target;
        }
        else
        {
            DetectedMonster = null;
            target = null;
            return null;
        }
    }
}
