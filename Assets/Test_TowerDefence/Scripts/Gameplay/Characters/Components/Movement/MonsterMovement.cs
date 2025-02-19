using System.Collections.Generic;
using UnityEngine;


public class MonsterMovement : BaseMovement
{
    [SerializeField] private Transform rotatablePart;
    [SerializeField] private BaseMonster baseMonster;
    
    private FactoriesService factoriesService;
    private Transform[] spawnPoints;
    private Transform[] wayPoints;
    private Transform target;

    private int wayCountIndex = 0;
    private float offsetX;
    private float offsetZ;

    private Transform currentSpawnPoint;

    private void Awake()
    {
        factoriesService = GameObject.Find("GameManager").GetComponent<FactoriesService>();
        spawnPoints = factoriesService.SpawnPoints;

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (transform.position == spawnPoints[i].position)
            {
                wayPoints = WaypointsService.Waypoints[i].Waypoint;
                currentSpawnPoint = spawnPoints[i];
            }
        }
    }

    private void OnEnable()
    {
        wayCountIndex = 0;

        for (int i = 0; i < spawnPoints.Length; i++)
        {

             if (transform.position == spawnPoints[i].position)
             {
                 wayPoints = WaypointsService.Waypoints[i].Waypoint;
             }
        }
        
        target = wayPoints[wayCountIndex];
    }

    private void OnDisable()
    {
        isMoves = false;
        wayCountIndex = 0;
        target = wayPoints[0];
    }

    private void Start()
    {
        target = wayPoints[wayCountIndex];
 
        offsetX = Random.Range(-2, 2);
        offsetZ = Random.Range(-2, 2);
    }

    public void Move(Transform target, float moveSpeed, float turnSpeed)
    {
        if (target != null)
        {
            SetTarget(target);
        }

        Vector3 randomDir = new Vector3(this.target.position.x + offsetX, this.target.position.y, this.target.position.z + offsetZ);
        Vector3 direction = randomDir - transform.position;
        transform.Translate(direction.normalized * moveSpeed * Time.deltaTime, Space.World);
        LockOnTarget(this.target, turnSpeed);

        if (Vector3.Distance(randomDir, transform.position) <= 3f && IsMoves)
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
        if (wayPoints.Length - 1 <= wayCountIndex)
        {
            PathEnd();
            return;
        }

        wayCountIndex++;
        target = wayPoints[wayCountIndex];
        offsetX = Random.Range(-2, 2);
        offsetZ = Random.Range(-2, 2);
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    public void SetIsMove(bool tof)
    {
        isMoves = tof;
    }

    private void PathEnd()
    {
        PlayerStats.Lives--;
        FactoriesService.EnemiesAlive--;
        gameObject.SetActive(false);
        baseMonster.SetIsDead(true);
    }

    public void SetWaypointsAndTarget(Transform[] waypoints, Transform currentTransform, Transform targetWaypoint, int waypointIndex)
    {
        wayPoints = waypoints;
        target = targetWaypoint;
        wayCountIndex = waypointIndex;
        transform.position = currentTransform.position;
    }

    public void SetMinionsPositionAndTarget(List<BaseMonster> minions, Transform parentTransform, Quaternion rotation)
    {
        for (int i = 0; i < minions.Count; i++)
        {
            BaseMonster generatedMinion = factoriesService.GenerateMinions(minions[Random.Range(0, minions.Count)], currentSpawnPoint, currentSpawnPoint.rotation);
            generatedMinion.Movement.SetWaypointsAndTarget(wayPoints, parentTransform, target, wayCountIndex);
        }

        
    }

    //private Transform MinionsRandomPosition(Transform parentTransform)
    //{
    //    Transform randomTransform = new Transform()
    //}
}
