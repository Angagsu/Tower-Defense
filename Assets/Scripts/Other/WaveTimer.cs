using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class WaveTimer : MonoBehaviour
{
    private int moneyGain;
    private float remainingDuration;
    private Coroutine coroutine;

    [SerializeField] private WaveSpawner waveSpawner;
    [Space(10f)]
    [SerializeField] private Button waypoints_1WavesCallButton;
    [SerializeField] private Button waypoints_2WavesCallButton;
    [SerializeField] private Button waypoints_3WavesCallButton;
    [Space(10f)]
    [SerializeField] private Image[] timerCircleFill;
    [SerializeField] private Text[] timerText;

    void Start()
    {
        ActivateWaveTimerButtons();
        SetRemainingDuration();
    }

    public void WavesCallingWithButton()
    {
        OnEnd();
    }

    private void SetRemainingDuration()
    {
        remainingDuration = waveSpawner.timeBetweenWaves;
    }

    public IEnumerator UpdateTimer()
    {
        while (remainingDuration >= 0)
        {
            for (int i = 0; i < timerCircleFill.Length; i++)
            {
                if (timerCircleFill[i].isActiveAndEnabled)
                {
                    timerText[i].text = $"{remainingDuration / 60:00} : {remainingDuration % 60:00}";
                    timerCircleFill[i].fillAmount = remainingDuration / waveSpawner.timeBetweenWaves;
                }
                
            }
            remainingDuration -= Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        OnEnd();
    }

    public void OnEnd()
    {
        DeActivateTimerButtons();
        CalculateMoneyForQuickStartNextWave();
        SetRemainingDuration();
        for (int i = 0; i < timerCircleFill.Length; i++)
        {
            timerText[i].text = $"{remainingDuration / 60:00} : {remainingDuration % 60:00}";
            timerCircleFill[i].fillAmount = 1;
        }

        waveSpawner.StartSpawningNextWave();
    }
    public void ActivateWaveTimerButtons()
    {
        if (waveSpawner.WaveIndex < waveSpawner.waypoints_1Waves.Length && waveSpawner.waypoints_1Waves[waveSpawner.WaveIndex].countOfEnemyToSpawn != 0)
        {
            waypoints_1WavesCallButton.gameObject.SetActive(true);
        }
        if (waveSpawner.WaveIndex < waveSpawner.waypoints_2Waves.Length && waveSpawner.waypoints_2Waves[waveSpawner.WaveIndex].countOfEnemyToSpawn != 0)
        {
            waypoints_2WavesCallButton.gameObject.SetActive(true);
        }
        if (waveSpawner.WaveIndex < waveSpawner.waypoints_3Waves.Length && waveSpawner.waypoints_3Waves[waveSpawner.WaveIndex].countOfEnemyToSpawn != 0)
        {
            waypoints_3WavesCallButton.gameObject.SetActive(true);
        }

        if (waypoints_1WavesCallButton.isActiveAndEnabled || waypoints_2WavesCallButton.isActiveAndEnabled || waypoints_3WavesCallButton.isActiveAndEnabled)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            coroutine = StartCoroutine(UpdateTimer());
        }

    }
    public void DeActivateTimerButtons()
    {
        StopCoroutine(coroutine);
        waypoints_1WavesCallButton.gameObject.SetActive(false);
        waypoints_2WavesCallButton.gameObject.SetActive(false);
        waypoints_3WavesCallButton.gameObject.SetActive(false);
    }

    private void CalculateMoneyForQuickStartNextWave()
    {
        moneyGain = (int)(remainingDuration) * (2 + waveSpawner.WaveIndex);
        Debug.Log("Money Gain " + moneyGain);
        PlayerStats.Money += moneyGain;
    }
}
