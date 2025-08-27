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

        if (DetectedMonster != null && shortestDistance <= attackRange && !isDead)
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
