using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private Transform enemyPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Text countdownText;
    [SerializeField] private float timeBetweenWaves = 5f;
    private float countdown = 2f;
    private int waveIndex = 1;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (countdown <= 0)
        {
            StartCoroutine(SpawnWaves());
            countdown = timeBetweenWaves;
        }

        countdown -= Time.deltaTime;
        countdownText.text = Mathf.Round(countdown).ToString();
    }

    private IEnumerator SpawnWaves()
    {
        for (int i = 0; i < waveIndex; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(0.5f);
        }
        waveIndex++;
    }

    private void SpawnEnemy()
    {
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}
