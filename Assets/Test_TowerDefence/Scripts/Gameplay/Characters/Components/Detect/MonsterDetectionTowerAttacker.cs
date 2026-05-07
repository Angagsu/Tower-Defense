using Assets.Scripts.Tower;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDetectionTowerAttacker : BaseDetection
{
    [SerializeField] private BaseMonster baseMonster;

    private Transform target;
 
    private List<BaseTower> towers;

    private DetectionHelper detectionHelper;


    private void Awake()
    {
        detectionHelper = DetectionHelper.Instance;

        towers = detectionHelper.Towers;
    }

    public override Transform DetectTarget(float attackRange, bool isDead)
    {
        float shortestDistance = Mathf.Infinity;


        if (DetectedTower && !DetectedTower.IsCrashed)
        {
            float sqrDistance = (transform.position - DetectedTower.transform.position).sqrMagnitude;

            if (sqrDistance <= (attackRange * attackRange))
            {
                if (DetectedTower.CurrentAttackerMonster == null || DetectedTower.CurrentAttackerMonster == baseMonster)
                {
                    AttackableTarget = DetectedTower;
                    return target = DetectedTower.PartForTargeting;
                }
                else
                {
                    AttackableTarget = null;
                    DetectedTower = null;
                    target = null;
                }  
            }
            else
            {
                AttackableTarget = null;
                DetectedTower = null;
                target = null;
            }
        }
        else
        {
            AttackableTarget = null;
            DetectedTower = null;
            target = null;
        }

        foreach (BaseTower tower in towers)
        {
            float sqrDistanceToTarget = (transform.position - tower.transform.position).sqrMagnitude;

            if (sqrDistanceToTarget < shortestDistance && !tower.IsCrashed)
            {
                shortestDistance = sqrDistanceToTarget;

                DetectedTower = tower;
            }
        }

        if (DetectedTower && shortestDistance <= (attackRange * attackRange) && !isDead)
        {
            if (DetectedTower.CurrentAttackerMonster == null || DetectedTower.CurrentAttackerMonster == baseMonster)
            {
                DetectedTower.SetCurrentAttackerMonster(baseMonster);

                AttackableTarget = DetectedTower;

                baseMonster.SetIsMoves(false);
                baseMonster.SetAttackedTower(DetectedTower);

                return target = DetectedTower.PartForTargeting;
            }
            else
            {
                baseMonster.SetIsMoves(true);
                DetectedTower = null;
                return target = null;
            }
        }
        else
        {
            baseMonster.SetIsMoves(true);
            DetectedTower = null;
            return target = null;
        }
    }
}
