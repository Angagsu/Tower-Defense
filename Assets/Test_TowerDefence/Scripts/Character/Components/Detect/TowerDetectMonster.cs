using UnityEngine;

public class TowerDetectMonster : BaseDetect
{
    private const string enemyTag = "Monster";
    private Transform target;
    BaseMonster detectedMonster = null;
    BaseMonster targetedMonster = null;
    private float distance;

    public override Transform DetectTarget(float attackRange, bool isDead)
    {
        BaseMonster[] monsters = FindObjectsByType<BaseMonster>(FindObjectsSortMode.InstanceID);
        float shortestDistance = Mathf.Infinity;

        foreach (var monster in monsters)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, monster.transform.position);
            

            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                targetedMonster = monster;
                distance = shortestDistance;
            }         
        }

        if (detectedMonster)
        {
            distance = Vector3.Distance(transform.position, detectedMonster.transform.position);
        }

        if (distance <= attackRange && !isDead)
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
                targetedMonster = null;
                return target = null;
            }
        }
        else
        {
            targetedMonster = null;
            detectedMonster = null;
            return target = null;
        }
    }
}
