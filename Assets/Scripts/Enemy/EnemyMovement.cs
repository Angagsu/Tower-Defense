using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    public bool isEnemyStoppedMove = true;

    private Enemy enemy;
    private Transform target;
    private int wayCountIndex = 0;
    private float turnSpeed = 10f;

    [SerializeField] private Transform enemyRotatPart;

    private WaveSpawner waveSpawner;
    private Transform[] spawnPoints;
    private Transform[] wayPoints;

    private float offsetX;
    private float offsetZ;
    private void Awake()
    {
        waveSpawner = GameObject.Find("GameManager").GetComponent<WaveSpawner>();
        enemy = GetComponent<Enemy>();
        enemy.enemySpeed = 0;
        spawnPoints = waveSpawner.spawnPoints;
        if (transform.position == spawnPoints[0].position)
        {
            wayPoints = WayPoints.wayPoints;
        }
        else if(transform.position == spawnPoints[1].position)
        {
            wayPoints = WayPoints_2.wayPoints_2;
        }
        else
        {
            wayPoints = WayPoints_3.wayPoints_3;
        }
        isEnemyStoppedMove = false;
    }
    private void Start()
    {
        target = wayPoints[0];
        enemy.enemySpeed = enemy.startSpeed;
        offsetX = Random.Range(-2, 2);
        offsetZ = Random.Range(-2, 2);
    }

    private void Update()
    {
        if (GameController.IsGameOver)
        {
            enabled = false;
            return;
        }

        if (isEnemyStoppedMove)
        {
            return;
        }
        
        if (!isEnemyStoppedMove)
        {
            EnemyMove();
            LockOnTarget(target);
        }
    }
    private void EnemyMove()
    {
        Vector3 randomDir = new Vector3(target.position.x + offsetX, target.position.y, target.position.z + offsetZ);

        Vector3 direction = randomDir - transform.position;
        transform.Translate(direction.normalized * enemy.enemySpeed * Time.deltaTime, Space.World);

        if (Vector3.Distance(randomDir, transform.position) <= 3f && !isEnemyStoppedMove)
        {
            GetNextWayPoint();
        }
    }

    public void LockOnTarget(Transform target)
    {
        if (!isEnemyStoppedMove)
        {
            this.target = target;
        }

        Vector3 direction = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Vector3 rotation = Quaternion.Lerp(enemyRotatPart.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        enemyRotatPart.rotation = Quaternion.Euler(0, rotation.y, 0);
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
        //offsetX = Random.Range(-2, 2);
        //offsetZ = Random.Range(-2, 2);
    }

    private void PathEnd()
    {
        PlayerStats.Lives--;
        WaveSpawner.EnemiesAlive--;
        Destroy(gameObject);
    }
}
