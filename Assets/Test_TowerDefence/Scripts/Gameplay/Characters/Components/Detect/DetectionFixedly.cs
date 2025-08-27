using System.Collections.Generic;
using UnityEngine;

public class DetectionFixedly : BaseDetection
{
    BaseMonster targetedMonster = null;

    private float distance;
    private Transform target;

    private List<BaseMonster> monsters = new();

    private DetectionHelper detectionHelper;

    private void Start()
    {
        detectionHelper = DetectionHelper.Instance;
        monsters = detectionHelper.Monsters;
    }

    public override Transform DetectTarget(float detectionRange, bool isDead)
    {
        foreach (var monster in monsters)
        {
            if (monster.isActiveAndEnabled)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, monster.transform.position);

                if (distanceToEnemy < detectionRange)
                {
                    distance = distanceToEnemy;
                    targetedMonster = monster;
                }
            }
            else
            {
                //targetedMonster = null;
            }        
        }
        
        if (DetectedMonster && !DetectedMonster.IsDead)
        {
            distance = Vector3.Distance(transform.position, DetectedMonster.transform.position);
        }

        if (distance <= detectionRange)
        {
            if (!DetectedMonster && targetedMonster && !targetedMonster.IsDead)
            {
                DetectedMonster = targetedMonster;
                return target = DetectedMonster.transform;
            }
            else if (DetectedMonster && !DetectedMonster.IsDead)
            {
                return target = DetectedMonster.transform;
            }
            else
            {
                DetectedMonster = null;
                return target = null;
            }
        }
        else
        {
            targetedMonster = null;
            DetectedMonster = null;
            return target = null;
        }  
    }
}
