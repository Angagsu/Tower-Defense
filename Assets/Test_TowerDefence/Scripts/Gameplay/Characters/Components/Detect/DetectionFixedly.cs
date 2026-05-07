using System.Collections.Generic;
using UnityEngine;

public class DetectionFixedly : BaseDetection
{
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
        if (DetectedMonster && DetectedMonster.IsDead == false)
        {
            float sqrDistance = (transform.position - DetectedMonster.transform.position).sqrMagnitude;

            if (sqrDistance <= detectionRange * detectionRange)
            {
                return target = DetectedMonster.transform;
            }
        }
        else
        {
            DetectedMonster = null;
            target = null;
        }

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
        
        if (DetectedMonster != null && shortestDistance <= (detectionRange * detectionRange) && DetectedMonster.CurrentAttackerHero == null)
        {
            target = DetectedMonster.transform;
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
