using System.Collections.Generic;
using UnityEngine;

public class DetectionFixedly : BaseDetection
{
    private Transform target;
    BaseMonster detectedMonster = null;
    BaseMonster targetedMonster = null;
    private float distance;

    private List<BaseMonster> monsters = new();

    private DetectionHelper detectionHelper;

    private void Start()
    {
        detectionHelper = DetectionHelper.Instance;
        monsters = detectionHelper.Monsters;
    }

    public override Transform DetectTarget(float attackRange, bool isDead)
    {
        foreach (var monster in monsters)
        {
            if (monster.isActiveAndEnabled)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, monster.transform.position);

                if (distanceToEnemy < attackRange)
                {
                    distance = distanceToEnemy;
                    targetedMonster = monster;
                }
            }        
        }
        
        if (detectedMonster && !detectedMonster.IsDead)
        {
            distance = Vector3.Distance(transform.position, detectedMonster.transform.position);
        }

        if (distance <= attackRange)
        {
            if (!detectedMonster && targetedMonster && !targetedMonster.IsDead)
            {
                detectedMonster = targetedMonster;
                return target = detectedMonster.transform;
            }
            else if (detectedMonster && !detectedMonster.IsDead)
            {
                return target = detectedMonster.transform;
            }
            else
            {
                detectedMonster = null;
                return target = null;
            }
        }
        else
        {
            detectedMonster = null;
            return target = null;
        }  
    }
}
