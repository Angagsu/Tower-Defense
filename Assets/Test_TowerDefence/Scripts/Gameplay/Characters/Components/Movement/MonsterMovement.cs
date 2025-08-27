using System.Collections.Generic;
using UnityEngine;


public class MonsterMovement : BaseMovement
{
    [SerializeField] private Transform rotatablePart;
    [SerializeField] private BaseMonster baseMonster;
    
    private List<Way> spawnPoints;
    private Transform[] wayPoints;
    private Transform target;

    private int waypointIndex = 0;

    private Transform currentSpawnPoint;

    private int currentWayesIndex;



    public void Construct()
    {
        spawnPoints = baseMonster.MonstersFactoriesService.SpawnPoints;

        for (int i = 0; i < spawnPoints.Count; i++)
        {
            for (int j = 0; j < spawnPoints[i].Waypoints.Count; j++)
            {
                if (transform.position == spawnPoints[i].Waypoints[j].gameObject.transform.position)
                {
                    currentWayesIndex = i;
                    wayPoints = spawnPoints[i].Waypoints[j].Waypoint;
                    currentSpawnPoint = spawnPoints[i].Waypoints[j].gameObject.transform;
                }
            }
        }

        target = wayPoints[waypointIndex];
        isMoves = true;
    }

    private void OnEnable()
    {
        isMoves = true;
        baseMonster.Anim.SetMoveAnimation(true);
        waypointIndex = 0;

        if (spawnPoints != null)
        {
            for (int i = 0; i < spawnPoints.Count; i++)
            {
                for (int j = 0; j < spawnPoints[i].Waypoints.Count; j++)
                {
                    if (transform.position == spawnPoints[i].Waypoints[j].transform.position)
                    {
                        currentWayesIndex = i;
                        wayPoints = spawnPoints[i].Waypoints[j].Waypoint;
                        currentSpawnPoint = spawnPoints[i].Waypoints[j].gameObject.transform;
                    }
                }
            }

            target = wayPoints[waypointIndex];
        } 
    }

    private void OnDisable()
    {
        baseMonster.Anim.SetMoveAnimation(false);
        isMoves = false;
        waypointIndex = 0;
        target = wayPoints[0];
    }

    public void Move(Transform target, float moveSpeed, float turnSpeed)
    {
        if (target != null)
        {
            SetTargetHero(target);
        }

        Vector3 direction = this.target.position - transform.position;
        transform.Translate(direction.normalized * moveSpeed * Time.deltaTime, Space.World);
        LockOnTarget(this.target, turnSpeed);

        if (Vector3.Distance(this.target.position, transform.position) <= 3f && IsMoves)
        {
            GetNextWayPoint();
        }
    }

    public void LockOnTarget(Transform target, float turnSpeed)
    {
        Vector3 direction = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Vector3 rotation = Quaternion.Lerp(rotatablePart.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        rotatablePart.rotation = Quaternion.Euler(0, rotation.y, 0);
    }

    private void GetNextWayPoint()
    {
        if (wayPoints.Length - 1 <= waypointIndex)
        {
            PathEnd();
            return;
        }

        waypointIndex++;
        target = wayPoints[waypointIndex];
    }

    public void SetTargetHero(Transform target)
    {
        this.target = target;
    }

    public void SetIsMove(bool tof)
    {
        isMoves = tof;
    }

    private void PathEnd()
    {
        baseMonster.GameplayPlayerDataHandler.ReduceLives(1);
        baseMonster.MonstersFactoriesService.ReduceAliveEnemiesAmount();

        gameObject.SetActive(false);
        baseMonster.SetIsDead(true);
    }

    public void SetWaypointsAndTarget(Transform[] waypoints, Transform currentTransform, Transform targetWaypoint, int waypointIndex)
    {
        wayPoints = waypoints;
        target = targetWaypoint;
        this.waypointIndex = waypointIndex;
        transform.position = currentTransform.position;
    }

    public void SetMinionsPositionAndTarget(List<BaseMonster> minions, Transform parentTransform, Quaternion rotation)
    {
        for (int i = 0; i < minions.Count; i++)
        {
            var randomWaypointIndex = Random.Range(0, 3);

            wayPoints = spawnPoints[currentWayesIndex].Waypoints[randomWaypointIndex].Waypoint;
            currentSpawnPoint = spawnPoints[currentWayesIndex].Waypoints[randomWaypointIndex].gameObject.transform;

            BaseMonster generatedMinion = baseMonster.MonstersFactoriesService.GenerateMinions(minions[Random.Range(0, minions.Count)], currentSpawnPoint, currentSpawnPoint.rotation);
            generatedMinion.Movement.SetWaypointsAndTarget(wayPoints, parentTransform, target, waypointIndex);
        }  
    }
}
