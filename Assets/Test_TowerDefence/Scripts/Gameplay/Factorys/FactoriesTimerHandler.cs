using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FactoriesTimerHandler : MonoBehaviour
{
    [SerializeField] private FactoriesService factoriesService;

    [Space(20f)]
    [SerializeField] private List<Button> waveCallButtons;

    [Space(15f)]
    [SerializeField] private Image[] timerCircleFill;

    [Space(15f)]
    [SerializeField] private Text[] timerText;


    private Coroutine coroutine;
    private int moneyGain;
    private float remainingDuration;

    private void Start()
    {
        ActivateWaveTimerButtons();
    }

    public void WavesCallingWithButton()
    {
        OnEnd();
    }

    private void SetRemainingDuration()
    {
        remainingDuration = factoriesService.TimeBetweenWaves;
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
                    timerCircleFill[i].fillAmount = remainingDuration / factoriesService.TimeBetweenWaves;
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

        factoriesService.StartSpawningNextWave();
    }
    public void ActivateWaveTimerButtons()
    {
        SetRemainingDuration();

        for (int i = 0; i < waveCallButtons.Count; i++)
        {
            if (factoriesService.WaveIndex < factoriesService.Wayes[i].Waves.Length && factoriesService.Wayes[i].Waves[factoriesService.WaveIndex].CountOfEnemyToSpawn != 0)
            {
                waveCallButtons[i].gameObject.SetActive(true);
            }

            if (waveCallButtons[i].isActiveAndEnabled)
            {
                if (coroutine != null)
                {
                    StopCoroutine(coroutine);
                }
                coroutine = StartCoroutine(UpdateTimer());
            }
        }
    }
    public void DeActivateTimerButtons()
    {
        StopCoroutine(coroutine);

        for (int i = 0; i < waveCallButtons.Count; i++)
        {
            waveCallButtons[i].gameObject.SetActive(false);
        }
    }

    private void CalculateMoneyForQuickStartNextWave()
    {
        moneyGain = (int)(remainingDuration) * (2 + factoriesService.WaveIndex);
        PlayerStats.Money += moneyGain;
    }
}
