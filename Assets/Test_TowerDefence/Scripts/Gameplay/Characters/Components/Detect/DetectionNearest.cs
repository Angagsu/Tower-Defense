using System.Collections.Generic;
using UnityEngine;

public class DetectionNearest : BaseDetection
{
    BaseMonster enemyTarget = null;
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
            float distanceToEnemy = Vector3.Distance(transform.position, monster.transform.position);


            if (distanceToEnemy < shortestDistance && monster && !monster.IsDead)
            {
                shortestDistance = distanceToEnemy;
                enemyTarget = monster;
            } 
        }

        if (enemyTarget != null && shortestDistance <= attackRange && !isDead)
        {
            target = enemyTarget.transform;
            return target;
        }
        else
        {
            enemyTarget = null;
            target = null;
            return null;
        }
    }
}
