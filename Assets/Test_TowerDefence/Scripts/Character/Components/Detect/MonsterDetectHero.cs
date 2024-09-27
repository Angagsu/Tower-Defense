using UnityEngine;

public class MonsterDetectHero : BaseDetect
{
    private Transform target;
    [SerializeField] private BaseMonster baseMonster;

    public override Transform DetectTarget(float attackRange, bool isDead)
    {
        BaseHero[] heroes = FindObjectsByType<BaseHero>(FindObjectsSortMode.InstanceID);

        float shortestDistance = Mathf.Infinity;
        Transform nearestHero = null;
        
        foreach (BaseHero hero in heroes)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, hero.transform.position);

            if (distanceToEnemy < shortestDistance && hero != null && !hero.IsDead)
            {
                shortestDistance = distanceToEnemy;
                nearestHero = hero.transform;

                if (nearestHero != null && shortestDistance <= attackRange && !isDead)
                {
                    if (hero.CurrentAttackerMonster == null || hero.CurrentAttackerMonster == baseMonster)
                    {
                        hero.SetCurrentAttackerEnemy(baseMonster);
                        baseMonster.SetIsMoves(false);
                        target = nearestHero.transform;
                        return target;
                    }
                    else
                    {
                        baseMonster.SetIsMoves(true);
                        target = null;
                        return target;
                    }
                }
                
            }
        }

       //if (nearestHero != null && shortestDistance <= attackRange && !isDead)
       //{
       //    if (nearestHero.TryGetComponent(out BaseHero defenderUnit))
       //    {
       //        if (defenderUnit.currentAttackerMonster == null || defenderUnit.currentAttackerMonster == baseMonster)
       //        {
       //            defenderUnit.SetCurrentAttackerEnemy(baseMonster);
       //            baseMonster.SetIsMoves(false);
       //            target = nearestHero.transform;
       //            return target;
       //        }
       //        else
       //        {
       //            baseMonster.SetIsMoves(true);
       //            target = null;
       //            return target;
       //        }
       //    }
       //    else
       //    {
       //        baseMonster.SetIsMoves(false);
       //        target = nearestHero.transform;
       //        return target;
       //    }
       //}
       //else
       //{
       //    baseMonster.SetIsMoves(true);
       //    target = null;
       //}

        return null;
    }
}
