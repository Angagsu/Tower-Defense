using UnityEngine;

public class HeroDetectMonster : BaseDetect
{
    private const string enemyTag = "Monster";
    private Transform target;


    public override Transform DetectTarget(float attackRange, bool isDead)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        BaseMonster enemyTarget = null;
    
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            enemyTarget = enemy.GetComponent<BaseMonster>();
    
            if (distanceToEnemy < shortestDistance && enemyTarget != null && !enemyTarget.IsDead)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }
    
        if (nearestEnemy != null && shortestDistance <= attackRange && !isDead)
        {
    
            target = nearestEnemy.transform;
            return target;
        }
        else
        {
            
            target = null;
        }
    
        return null;
    }
}
