using System.Collections;
using UnityEngine;


public class WaveSpawner : MonoBehaviour
{
    [Space(10f)]
    [Header("Different Waves Path")]

    [Space(10f)]
    public Wave[] waypoints_1Waves;

    [Space(10f)]
    public Wave[] waypoints_2Waves;

    [Space(10f)]
    public Wave[] waypoints_3Waves;

    [Space(10f)]
    public float timeBetweenWaves;

    [Space(10f)]
    public Transform[] spawnPoints;

    [Space(10f)]
    [SerializeField] private GameController gameController;
    [SerializeField] private WaveTimer waveTimer;

    public float CountDown { get { return countdown; } }
    public int WaveIndex { get { return waveIndex; } }
    public static int EnemiesAlive;


    private int waveIndex;
    private float countdown = 5f;
    private int currentCountOfEnemyToSpawn = 0;
    

    private void Start()
    {
        waveIndex = 0;
        EnemiesAlive = 0;
        Debug.Log("EnemiesAlive" + EnemiesAlive);
        Debug.Log("IsGameOver " + GameController.IsGameOver);
        Debug.Log(Time.timeScale);
    }
    private void Update()
    {
        if (GameController.IsGameOver)
        {
            enabled = false;
            return;
        }

        if (EnemiesAlive > 0)
        {
            return;
        }

        if (waveIndex == waypoints_1Waves.Length)
        {
            gameController.LevelComplete();
            waveTimer.DeActivateTimerButtons();
            this.enabled = false;
        }

        countdown -= Time.deltaTime;
        countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);
    }

    private IEnumerator SpawnWaves()
    {
        PlayerStats.Waves++;
        Wave waypoints_1Wave = waypoints_1Waves[waveIndex];
        Wave waypoints_2Wave = waypoints_2Waves[waveIndex];
        Wave waypoints_3Wave = waypoints_3Waves[waveIndex];
        EnemiesAlive += waypoints_1Wave.countOfEnemyToSpawn + waypoints_2Wave.countOfEnemyToSpawn + waypoints_3Wave.countOfEnemyToSpawn;
        

        if (waypoints_1Wave.countOfEnemyToSpawn >= waypoints_2Wave.countOfEnemyToSpawn && waypoints_1Wave.countOfEnemyToSpawn >= waypoints_3Wave.countOfEnemyToSpawn)
        {
            currentCountOfEnemyToSpawn = waypoints_1Wave.countOfEnemyToSpawn;
        }
        if (waypoints_2Wave.countOfEnemyToSpawn >= waypoints_1Wave.countOfEnemyToSpawn && waypoints_2Wave.countOfEnemyToSpawn >= waypoints_3Wave.countOfEnemyToSpawn)
        {
            currentCountOfEnemyToSpawn = waypoints_2Wave.countOfEnemyToSpawn;
        }
        if (waypoints_3Wave.countOfEnemyToSpawn >= waypoints_1Wave.countOfEnemyToSpawn && waypoints_3Wave.countOfEnemyToSpawn >= waypoints_2Wave.countOfEnemyToSpawn)
        {
            currentCountOfEnemyToSpawn = waypoints_3Wave.countOfEnemyToSpawn;
        }

        for (int i = 0; i < currentCountOfEnemyToSpawn; i++)
        {
            if (waypoints_1Wave.enemyPrefab.Length != 0 && waypoints_1Wave.countOfEnemyToSpawn > i)
            {
                Instantiate(waypoints_1Wave.enemyPrefab[i], spawnPoints[0].position, spawnPoints[0].rotation);
            }
            if (waypoints_2Wave.enemyPrefab.Length != 0 && waypoints_2Wave.countOfEnemyToSpawn > i)
            {
                Instantiate(waypoints_2Wave.enemyPrefab[i], spawnPoints[1].position, spawnPoints[1].rotation);
            }
            if (waypoints_3Wave.enemyPrefab.Length != 0 && waypoints_3Wave.countOfEnemyToSpawn > i)
            {
                Instantiate(waypoints_3Wave.enemyPrefab[i], spawnPoints[2].position, spawnPoints[2].rotation);
            }
            
            yield return new WaitForSeconds(waypoints_1Wave.rateOfSpawn);
        }

        waveIndex++;
        if (!GameController.IsGameOver)
        {
            waveTimer.ActivateWaveTimerButtons();
        }
    }
    public void StartSpawningNextWave()
    {
        StartCoroutine(SpawnWaves());
    }

}
