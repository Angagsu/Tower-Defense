using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoriesService : MonoBehaviour
{
    public event Action WaveComplited;
    public LevelConfigSO levelConfig;
    [field: SerializeField] public List<Wayes> Wayes { get; private set; }

    [field: SerializeField] public Transform[] SpawnPoints { get; private set; }

    [Space(10f)]
    [SerializeField] private GameController gameController;
    [SerializeField] private FactoriesTimerHandler factoriesTimerHandler;

    public float timeBetweenWaves;

    public int WaveIndex => waveIndex;

    public static int EnemiesAlive;

    private float rateOfSpawn;
    private int waveIndex;
    private int currentCountOfEnemyToSpawn = 0;
    private Coroutine coroutine;


    public MonsterFactory[] monstersFactories;

    private void Awake()
    {
        Wayes = levelConfig.Wayes;
    }

    private void Start()
    {
        
        waveIndex = 0;
        EnemiesAlive = 0;
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

        if (waveIndex == Wayes[0].Waves.Length)
        {
            gameController.LevelComplete();
            factoriesTimerHandler.DeActivateTimerButtons();
            enabled = false;
        }
    }

    private IEnumerator SpawnWaves()
    {
        PlayerStats.Waves++;
        WaveComplited?.Invoke();
        SetCountOfMonstersToSpawnOnEachWave();

        rateOfSpawn = Wayes[0].Waves[waveIndex].RateOfSpawn;

       //for (int i = 0; i < currentCountOfEnemyToSpawn; i++)
       //{
       //    for (int j = 0; j < Wayes.Count; j++)
       //    {
       //        if (Wayes[j].Waves[waveIndex].EnemyPrefab.Length != 0 && Wayes[j].Waves[waveIndex].CountOfEnemyToSpawn > i)
       //        {
       //            Instantiate(Wayes[j].Waves[waveIndex].EnemyPrefab[i], SpawnPoints[j].position, SpawnPoints[j].rotation);
       //        }  
       //    }
       //
       //    yield return new WaitForSeconds(rateOfSpawn);
       //}

        for (int i = 0; i < currentCountOfEnemyToSpawn; i++)
        {
            for (int j = 0; j < Wayes.Count; j++)
            {
                if (Wayes[j].Waves[waveIndex].EnemyPrefab.Length != 0 && Wayes[j].Waves[waveIndex].CountOfEnemyToSpawn > i)
                {
                    Wayes[j].Waves[waveIndex].EnemyPrefab[i].TryGetComponent(out BaseMonster monster);

                    for (int u = 0; u < monstersFactories.Length; u++)
                    {
                        if (monstersFactories[u].GetFactoryType.GetType().BaseType == monster.GetType().BaseType) 
                        {
                            monstersFactories[u].GetMonsterTypeByID(monster, SpawnPoints[j], SpawnPoints[j].rotation);
                        }
                    }
                }
            }

            yield return new WaitForSeconds(rateOfSpawn);
        }

        waveIndex++;

        if (!GameController.IsGameOver)
        {
            factoriesTimerHandler.ActivateWaveTimerButtons();
        }
    }
    public void StartSpawningNextWave()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(SpawnWaves());
    }

    private void SetCountOfMonstersToSpawnOnEachWave()
    {
        List<int> currentCountOfMonstersToSpawnList = new List<int>();

        for (int i = 0; i < Wayes.Count; i++)
        {
            EnemiesAlive += Wayes[i].Waves[waveIndex].CountOfEnemyToSpawn;
            currentCountOfMonstersToSpawnList.Add(Wayes[i].Waves[waveIndex].CountOfEnemyToSpawn);
        }

        int intermediatCountOfMonstersForSpawn = default;

        foreach (var eachWaveMonstersCountForSpawn in currentCountOfMonstersToSpawnList)
        {
            if (eachWaveMonstersCountForSpawn >= intermediatCountOfMonstersForSpawn)
            {
                intermediatCountOfMonstersForSpawn = eachWaveMonstersCountForSpawn;
            }
        }

        currentCountOfEnemyToSpawn = intermediatCountOfMonstersForSpawn;
    }
}

[Serializable]
public class Wayes
{
    public WaveNew[] Waves;
}
