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


    private void Awake()
    {
        factoriesService = GameObject.Find("GameManager").GetComponent<FactoriesService>();
        spawnPoints = factoriesService.SpawnPoints;

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (transform.position == spawnPoints[i].position)
            {
                wayPoints = WaypointsService.Waypoints[i].Waypoint;
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
        
        target = wayPoints[0];
    }

    private void OnDisable()
    {
        isMoves = false;
        wayCountIndex = 0;
        target = wayPoints[0];
    }

    private void Start()
    {
        
        target = wayPoints[0];
 
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
}
