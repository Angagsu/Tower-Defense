using UnityEngine;



public class MonsterMovement : BaseMovement
{
    [field: SerializeField] public Transform RotatablePart { get; private set; }
    [SerializeField] private BaseMonster baseMonster;
    
    private Transform target;
    private FactoriesService factoriesService;
    private Transform[] spawnPoints;
    private Transform[] wayPoints;

    private int wayCountIndex = 0;
    private float offsetX;
    private float offsetZ;


    private void Awake()
    {
        factoriesService = GameObject.Find("GameManager").GetComponent<FactoriesService>();
        spawnPoints = factoriesService.SpawnPoints;

        if (transform.position == spawnPoints[0].position)
        {
            wayPoints = WayPoints.wayPoints;
        }
        else if (transform.position == spawnPoints[1].position)
        {
            wayPoints = WayPoints_2.wayPoints_2;
        }
        else if (transform.position == spawnPoints[2].position)
        {
            wayPoints = WayPoints_3.wayPoints_3;
        }

       // isMoves = true;
    }

    private void OnEnable()
    {
        wayCountIndex = 0;
        


        if (transform.position == spawnPoints[0].position)
        {
            wayPoints = WayPoints.wayPoints;
        }
        else if (transform.position == spawnPoints[1].position)
        {
            wayPoints = WayPoints_2.wayPoints_2;
        }
        else if (transform.position == spawnPoints[2].position)
        {
            wayPoints = WayPoints_3.wayPoints_3;
        }
        target = wayPoints[0];
        isMoves = true;
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
        Vector3 rotation = Quaternion.Lerp(RotatablePart.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        RotatablePart.rotation = Quaternion.Euler(0, rotation.y, 0);
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
    }
}
