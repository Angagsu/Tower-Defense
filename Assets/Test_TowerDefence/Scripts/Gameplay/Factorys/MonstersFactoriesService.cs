using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonstersFactoriesService : MonoBehaviour
{
    public event Action WaveComplited;
    public event Action<BaseMonster> MonsterCountIncreased;
    public event Action<BaseMonster> MonsterCountDecreased;

    public List<Wayes> Wayes { get; private set; }

    [field: SerializeField] public LevelConfigSO LevelConfig { get; private set; }
    
    [field: SerializeField] public float TimeBetweenWaves { get; private set; }

    [field: SerializeField] public List<Way> SpawnPoints { get; private set; }

    [Space(10f)]
    [SerializeField] private BaseMonsterFactory[] monstersFactories;

    [Space(10f)]
    [SerializeField] private GameOverHandler gameOverHandler;
    [SerializeField] private WavesTimersUI factoriesTimerHandler;
    [SerializeField] private GameplayPlayerDataHandler playerDataHandler;
    [SerializeField] private GameplayStates gameplayStates;
    [SerializeField] private MonsterDependenciesInjecter monsterDependencyInjecter;

    public int WaveIndex => waveIndex;

    
    private Coroutine coroutine;

    private float rateOfSpawn;
    private int waveIndex;
    private int currentCountOfEnemyToSpawn = 0;
    private int enemiesAlive;

    private bool isPaused;

    


    private void Awake()
    {
        Wayes = LevelConfig.Wayes; 
    }

    private void OnEnable()
    {
        gameplayStates.Paused += OnGameplayPause;
        gameplayStates.Unpaused += OnGameplayPlay;
    }
 
    private void OnGameplayPlay() => isPaused = false;

    private void OnGameplayPause() => isPaused = true;

    public void DecreaseMonsterCount(BaseMonster monster)
    {
        enemiesAlive--;
        MonsterCountDecreased?.Invoke(monster);
    }

    private void OnDisable()
    {
        gameplayStates.Paused -= OnGameplayPause;
        gameplayStates.Unpaused -= OnGameplayPlay;
    }

    private void Start()
    {
        waveIndex = 0;
        enemiesAlive = 0;
    }

    private void Update()
    {
        if (enemiesAlive > 0)
        {
            return;
        }

        if (waveIndex == Wayes[0].Waves.Count)
        {
            gameOverHandler.LevelComplete();

            factoriesTimerHandler.DeActivateTimerButtons();

            enabled = false;
        }
    }

    private IEnumerator SpawnWaves()
    {
        playerDataHandler.IncreaseWaves(1);

        WaveComplited?.Invoke();

        SetCountOfMonstersToSpawnOnEachWave();

        for (int i = 0; i < currentCountOfEnemyToSpawn; i++)
        {
            for (int j = 0; j < Wayes.Count; j++)
            {
                rateOfSpawn = Wayes[j].Waves[waveIndex].RateOfSpawn;

                for (int k = 0; k < Wayes[j].Waves[waveIndex].Patways.Count; k++)
                {
                    if (Wayes[j].Waves[waveIndex].Patways[k].EnemyPrefab.Length != 0 && Wayes[j].Waves[waveIndex].Patways[k].EnemyPrefab.Length > i && Wayes[j].Waves[waveIndex].Patways[k].EnemyPrefab[i] != null)
                    {
                        Wayes[j].Waves[waveIndex].Patways[k].EnemyPrefab[i].TryGetComponent(out BaseMonster monsterPrefab);

                        for (int u = 0; u < monstersFactories.Length; u++)
                        {
                            if (monstersFactories[u].GetFactoryType.GetType().BaseType == monsterPrefab.GetType().BaseType)
                            {
                                while (isPaused) yield return null;

                                var generatedMinion = monstersFactories[u].GetMonsterByType(monsterPrefab, SpawnPoints[j].Waypoints[k].transform, SpawnPoints[j].Waypoints[k].transform.rotation);
                                monsterDependencyInjecter.SetMonsterDependencies(generatedMinion);
                                enemiesAlive++;
                                MonsterCountIncreased?.Invoke(generatedMinion);
                            }
                        }
                    }
                }
            }

            yield return new WaitForSeconds(rateOfSpawn);
        } 
        
        waveIndex++;

        if (gameplayStates.State != GameplayState.Defeat || gameplayStates.State != GameplayState.Complete) 
        {
            while (isPaused) yield return null;
            
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
            for (int j = 0; j < Wayes[i].Waves[waveIndex].Patways.Count; j++)
            {
                currentCountOfMonstersToSpawnList.Add(Wayes[i].Waves[waveIndex].CountOfEnemyToSpawn);
            }
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

    public BaseMonster GenerateMinions(BaseMonster minion, Transform transform, Quaternion rotation)
    {
        for (int i = 0; i < monstersFactories.Length; i++)
        {
            if (monstersFactories[i].GetFactoryType.GetType().BaseType == minion.GetType().BaseType)
            {
                var generatedMinion = monstersFactories[i].GetMonsterByType(minion, transform, transform.rotation);
                monsterDependencyInjecter.SetMonsterDependencies(generatedMinion);
                enemiesAlive++;
                MonsterCountIncreased?.Invoke(generatedMinion);
                return generatedMinion;
            }
        }
        
        return default;
    } 
}

[Serializable]
public class Wayes
{
    public List<Wave> Waves;
}
