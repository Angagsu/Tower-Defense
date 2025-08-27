using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WavesTimersUI : MonoBehaviour
{
    [SerializeField] private MonstersFactoriesService monstersFactoriesService;
    [SerializeField] private GameplayPlayerDataHandler playerDataHandler;
    [SerializeField] private GameplayStates gameplayStateHandler;

    [Space(20f)]
    [SerializeField] private List<Button> waveCallButtons;

    [Space(15f)]
    [SerializeField] private Image[] timerCircleFill;

    [Space(15f)]
    [SerializeField] private RectTransform[] arrowImages;
    [SerializeField] private Transform[] posOffset;
    [Space(10)]
    [SerializeField] private Vector2 screenBoundsPadding = new Vector2(150, 150);

    private Camera mainCamera;
    private Coroutine coroutine;
    private int moneyGain;
    private float remainingDuration;
    private bool isGamePaused;




    private void OnEnable()
    {
        for (int i = 0; i < waveCallButtons.Count; i++)
        {
            waveCallButtons[i].onClick.AddListener(OnClickTimerButton);
        }

        gameplayStateHandler.Paused += OnGameplayPause;
        gameplayStateHandler.Unpaused += OnGameplayPlay;
    }

    private void OnGameplayPlay()
    {
        if (gameplayStateHandler.State == GameplayState.Start && isGamePaused)
        {
            isGamePaused = true;
            return;
        }

        isGamePaused = false;
    }

    private void OnGameplayPause()
    {
        isGamePaused = true;
    }

    private void Start()
    {
        mainCamera = Camera.main;
        isGamePaused = true;
        ActivateWaveTimerButtons();
    }

    private void OnDisable()
    {
        for (int i = 0; i < waveCallButtons.Count; i++)
        {
            waveCallButtons[i].onClick.RemoveListener(OnClickTimerButton);
        }

        gameplayStateHandler.Paused -= OnGameplayPause;
        gameplayStateHandler.Unpaused -= OnGameplayPlay;
    }

    public void OnClickTimerButton()
    {
        if (gameplayStateHandler.State == GameplayState.Start)
        {
            gameplayStateHandler.SetStatePlay();
        }

        OnEnd();
    }

    private void SetRemainingDuration()
    {
        remainingDuration = monstersFactoriesService.TimeBetweenWaves;
    }

    

    private void OnEnd()
    {
        DeActivateTimerButtons();
        CalculateMoneyForQuickStartNextWave();
        SetRemainingDuration();

        for (int i = 0; i < timerCircleFill.Length; i++)
        {
            timerCircleFill[i].fillAmount = 1;
        }

        monstersFactoriesService.StartSpawningNextWave();
    }
    public void ActivateWaveTimerButtons()
    {
        SetRemainingDuration();

        for (int i = 0; i < waveCallButtons.Count; i++)
        {
            if (monstersFactoriesService.WaveIndex < monstersFactoriesService.Wayes[i].Waves.Count &&
                monstersFactoriesService.Wayes[i].Waves[monstersFactoriesService.WaveIndex].CountOfEnemyToSpawn != 0)
            {
                waveCallButtons[i].gameObject.SetActive(true);

                SetTimerPositionOnScreen(i);
            }

            if (waveCallButtons[i].isActiveAndEnabled)
            {
                if (coroutine != null)
                {
                    StopCoroutine(coroutine);
                }
                coroutine = StartCoroutine(UpdateTimer());
                StartCoroutine(UpdateTimerPosition());
            }
        }
    }

    private IEnumerator UpdateTimer()
    {
        while (remainingDuration >= 0)
        {
            while (isGamePaused)
            {
                yield return null;
            }

            for (int i = 0; i < timerCircleFill.Length; i++)
            {
                if (timerCircleFill[i].isActiveAndEnabled)
                {
                    timerCircleFill[i].fillAmount = remainingDuration / monstersFactoriesService.TimeBetweenWaves;
                }
            }

            remainingDuration -= Time.deltaTime;
            yield return null;
        }

        OnEnd();
    }

    private IEnumerator UpdateTimerPosition()
    {
        while (remainingDuration >= 0)
        {
            for (int i = 0; i < waveCallButtons.Count; i++)
            {
                if (waveCallButtons[i].isActiveAndEnabled)
                {
                    SetTimerPositionOnScreen(i);
                }
            }

            yield return null;
        }
    }

    public void DeActivateTimerButtons()
    {
        if (coroutine != null) 
        {
            StopCoroutine(coroutine);
        }
        

        for (int i = 0; i < waveCallButtons.Count; i++)
        {
            waveCallButtons[i].gameObject.SetActive(false);
        }
    }

    private void CalculateMoneyForQuickStartNextWave()
    {
        if (monstersFactoriesService.WaveIndex == 0)
        {
            return;
        }

        moneyGain = (int)(remainingDuration) * (2 + monstersFactoriesService.WaveIndex);
        playerDataHandler.IncreaseMoney(moneyGain);
    }

    private void SetTimerPositionOnScreen(int index)
    {
        Vector3 worldPos = monstersFactoriesService.SpawnPoints[index].Waypoints[0].transform.position + posOffset[index].position;
        Vector3 screenPos = mainCamera.WorldToScreenPoint(worldPos);
        Vector3 clampedScreenPos = screenPos;

        if (screenPos.z < 0)
        {
            screenPos *= -1f; 
        }
        
        Vector2 screenSize = new Vector2(Screen.width, Screen.height);
        
        clampedScreenPos.x = Mathf.Clamp(screenPos.x, screenBoundsPadding.x, screenSize.x - screenBoundsPadding.x);
        clampedScreenPos.y = Mathf.Clamp(screenPos.y, screenBoundsPadding.y, screenSize.y - screenBoundsPadding.y);
        
        waveCallButtons[index].transform.position = clampedScreenPos;


        Vector2 rotationCenter = arrowImages[index].position;
        Vector2 direction = -((Vector2)screenPos - rotationCenter).normalized;
  
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        arrowImages[index].localRotation = Quaternion.Euler(0, 0, angle);
    }
}
