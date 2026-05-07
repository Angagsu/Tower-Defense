using Assets.Scripts.Tower;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDetectionKamikaze : BaseDetection
{
    [SerializeField] private BaseMonster baseMonster;

    private Transform target;

    private List<BaseTower> towers;

    private DetectionHelper detectionHelper;

    private BaseTower detectedTower;

    private void Awake()
    {
        detectionHelper = DetectionHelper.Instance;

        towers = detectionHelper.Towers;
    }

    public override Transform DetectTarget(float detectionRange, bool isDead)
    {
        if (detectedTower && !detectedTower.IsCrashed)
        {
            float sqrDistanceToTarget = (transform.position - detectedTower.transform.position).sqrMagnitude;

            if (sqrDistanceToTarget <= (detectionRange * detectionRange))
            {
                return target = detectedTower.transform;
            }
        }
        else
        {
            detectedTower = null;
            target = null;
        }

        float shortestDistance = Mathf.Infinity;


        foreach (BaseTower tower in towers)
        {
            float sqrDistanceToTarget = (transform.position - tower.transform.position).sqrMagnitude;

            if (sqrDistanceToTarget < shortestDistance && !tower.IsCrashed)
            {
                shortestDistance = sqrDistanceToTarget;
        
                detectedTower = tower;
            }
        }
        
        if (detectedTower && shortestDistance <= (detectionRange * detectionRange) && !isDead)
        {
            if (detectedTower.CurrentAttackerMonster == null || detectedTower.CurrentAttackerMonster == baseMonster)
            {
                detectedTower.SetCurrentAttackerMonster(baseMonster);
        
                AttackableTarget = detectedTower;
        
                baseMonster.SetAttackedTower(detectedTower);
        
                return target = detectedTower.transform;
            }
            else
            {
                baseMonster.SetIsMoves(true);
                detectedTower = null;
                return target = null;
            }
        }
        else
        {
            baseMonster.SetIsMoves(true);
            detectedTower = null;
            return target = null;
        }
    }
}
