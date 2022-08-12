using System.Collections;
using UnityEngine;


public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    public static int EnemiesAlive;

    [SerializeField] private Wave[] waves;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float timeBetweenWaves = 5f;
    private float countdown = 2f;
    public float CountDown { get { return countdown; } }
    private int waveIndex = 0;

    private void Start()
    {
        EnemiesAlive = 0;
        Debug.Log("EnemiesAlive" + EnemiesAlive);
        Debug.Log(GameController.IsGameOver);
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

        if (waveIndex == waves.Length)
        {
            gameController.LevelComplete();
            this.enabled = false;
        }

        if (countdown <= 0)
        {
            StartCoroutine(SpawnWaves());
            countdown = timeBetweenWaves;
            return;
        }

        countdown -= Time.deltaTime;
        countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);
        
    }

    private IEnumerator SpawnWaves()
    {
        PlayerStats.Waves++;
        Wave wave = waves[waveIndex];
        EnemiesAlive = wave.countOfEnemyToSpawn;
        for (int i = 0; i < wave.countOfEnemyToSpawn; i++)
        {
            SpawnEnemy(wave.enemyPrefab);
            yield return new WaitForSeconds(1 / wave.rateOfSpawn);
        }
        waveIndex++;
    }

    private void SpawnEnemy(GameObject enemyPrefab)
    { 
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}
