using System.Collections.Generic;
using UnityEngine;

public class DetectionNearest : BaseDetection
{
    private Transform target;

    private List<BaseMonster> monsters = new();

    private DetectionHelper detectionHelper;

    private void Start()
    {
        detectionHelper = DetectionHelper.Instance;
        monsters = detectionHelper.Monsters;
    }

    public override Transform DetectTarget(float attackRange, bool isDead)
    {
        float shortestDistance = Mathf.Infinity;

        foreach (BaseMonster monster in monsters)
        {
            float sqrDistanceToEnemy = (transform.position - monster.transform.position).sqrMagnitude;

            if (sqrDistanceToEnemy < shortestDistance && monster && !monster.IsDead)
            {
                shortestDistance = sqrDistanceToEnemy;
                DetectedMonster = monster;
            }
        }

        if (DetectedMonster != null && shortestDistance <= (attackRange * attackRange) && !isDead)
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
