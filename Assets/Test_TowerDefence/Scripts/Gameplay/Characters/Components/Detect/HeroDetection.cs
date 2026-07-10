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
            float sqrDistance = (transform.position - DetectedMonster.transform.position).sqrMagnitude;

            if (sqrDistance <= (detectionRange * detectionRange))
            {
                return target = DetectedMonster.PartForTargeting;
            }
            else
            {
                DetectedMonster = null;
                target = null;
            }
        }
        else
        {
            DetectedMonster = null;
            target = null;
        }

        foreach (BaseMonster monster in monsters)
        {
            float sqrDistanceToEnemy = (transform.position - monster.transform.position).sqrMagnitude;

            if (sqrDistanceToEnemy < shortestDistance && monster && !monster.IsDead)
            {
                shortestDistance = sqrDistanceToEnemy;
                DetectedMonster = monster;
            }
        }

        if (DetectedMonster != null && shortestDistance <= (detectionRange * detectionRange) && (DetectedMonster.CurrentAttackerHero == null || DetectedMonster.CurrentAttackerHero == hero))
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
