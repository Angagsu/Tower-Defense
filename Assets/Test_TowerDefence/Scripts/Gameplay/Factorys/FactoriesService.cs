using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoriesService : MonoBehaviour
{
    public static int EnemiesAlive;

    public event Action WaveComplited;
    public event Action<BaseMonster> MonsterCountChanged;

    public List<Wayes> Wayes { get; private set; }

    [field: SerializeField] public LevelConfigSO LevelConfig { get; private set; }
    
    [field: SerializeField] public float TimeBetweenWaves { get; private set; }

    [field: SerializeField] public Transform[] SpawnPoints { get; private set; }

    [Space(10f)]
    [SerializeField] private MonsterFactory[] monstersFactories;

    [Space(10f)]
    [SerializeField] private GameController gameController;
    [SerializeField] private FactoriesTimerHandler factoriesTimerHandler;
    

    public int WaveIndex => waveIndex;

    private float rateOfSpawn;
    private int waveIndex;
    private int currentCountOfEnemyToSpawn = 0;
    private Coroutine coroutine;
    

    private void Awake()
    {
        Wayes = LevelConfig.Wayes;

        for (int i = 0; i < monstersFactories.Length; i++)
        {
            monstersFactories[i].PoolChanged += OnPoolChanged;
        }
    }

    private void OnPoolChanged(BaseMonster obj)
    {
        MonsterCountChanged?.Invoke(obj);
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

        for (int i = 0; i < currentCountOfEnemyToSpawn; i++)
        {
            for (int j = 0; j < Wayes.Count; j++)
            {
                if (Wayes[j].Waves[waveIndex].EnemyPrefab.Length != 0 && Wayes[j].Waves[waveIndex].CountOfEnemyToSpawn > i)
                {
                    Wayes[j].Waves[waveIndex].EnemyPrefab[i].TryGetComponent(out BaseMonster monsterPrefab);

                    for (int u = 0; u < monstersFactories.Length; u++)
                    {
                        if (monstersFactories[u].GetFactoryType.GetType().BaseType == monsterPrefab.GetType().BaseType) 
                        {
                            monstersFactories[u].GetMonsterByType(monsterPrefab, SpawnPoints[j], SpawnPoints[j].rotation);
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

    private void OnDestroy()
    {
        for (int i = 0; i < monstersFactories.Length; i++)
        {
            monstersFactories[i].PoolChanged -= OnPoolChanged;
        }
    }
}

[Serializable]
public class Wayes
{
    public Wave[] Waves;
}
