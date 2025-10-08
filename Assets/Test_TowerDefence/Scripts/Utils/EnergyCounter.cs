using System;
using System.Collections;
using UnityEngine;


[Serializable]
public class EnergyCounter 
{
    public event Action<int> EnergyAmountChanged;
    public event Action<TimeSpan> EnergyTimerTicked;
    public int CurrentEnergy => currentEnergy;

    private const string KEY_LAST_TS = "Energy.LastTsUtc"; 
    private const string KEY_CARRY_SECONDS = "Energy.CarrySec";

    private GamePlayerDataHandler playerDataHandler;

    private int currentEnergy;
    private int maxEnergy;
    private int energyPerTenMinutes = 1; 

    private float timer;


    private int IntervalSec => Mathf.Max(1, energyPerTenMinutes * 600);

    public EnergyCounter(GamePlayerDataHandler playerDataHandler, int currentEnergy, int maxEnergy)
    {
        this.playerDataHandler = playerDataHandler;
        this.currentEnergy = currentEnergy;
        this.maxEnergy = maxEnergy;

        this.playerDataHandler.PlayerDataChanged += OnPlayerDataChange;
    }

    private void OnPlayerDataChange(GamePlayerData playerData)
    {
        currentEnergy = playerData.Energy;
    }

    public void SaveEnergy()
    {
        PlayerPrefs.SetString("LastPlayTime", DateTime.Now.ToBinary().ToString());
        PlayerPrefs.Save();
        Debug.Log("PlayerPrefs_Energy is saved");
    }

    public void SaveEnergyState()
    {
        long nowUtc = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        PlayerPrefs.SetString(KEY_LAST_TS, nowUtc.ToString());

        float carryToSave = (currentEnergy >= maxEnergy) ? 0f : Mathf.Clamp(timer, 0f, IntervalSec - 0.001f);

        PlayerPrefs.SetFloat(KEY_CARRY_SECONDS, carryToSave);
        PlayerPrefs.Save();
    }

    public void LoadSavedEnergy()
    {
        if (PlayerPrefs.HasKey("LastPlayTime"))
        {
            long temp = Convert.ToInt64(PlayerPrefs.GetString("LastPlayTime"));
            DateTime lastPlayTime = DateTime.FromBinary(temp);

            TimeSpan timePassed = DateTime.Now - lastPlayTime;
            int minutesPassed = (int)timePassed.TotalMinutes;

            int energyToAdd = minutesPassed / energyPerTenMinutes;


            currentEnergy = Mathf.Min(currentEnergy + energyToAdd, maxEnergy);

            EnergyAmountChanged?.Invoke(currentEnergy);

            timer = (float)(timePassed.TotalSeconds % (energyPerTenMinutes * 60));

            UpdateTimerValues();
        }
    }

    public void ApplyOfflineProgress()
    {
        string tsStr = PlayerPrefs.GetString(KEY_LAST_TS, "");
        float savedCarry = PlayerPrefs.GetFloat(KEY_CARRY_SECONDS, 0f);

        if (!string.IsNullOrEmpty(tsStr) && long.TryParse(tsStr, out long lastTs))
        {
            long nowUtc = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            long deltaSec = Mathf.Max(0, (int)(nowUtc - lastTs));

            long totalSec = deltaSec + Mathf.RoundToInt(savedCarry);

            int add = (int)(totalSec / IntervalSec);
            int remainder = (int)(totalSec % IntervalSec);

            int newEnergy = Mathf.Min(currentEnergy + add, maxEnergy);
            currentEnergy = newEnergy;
            EnergyAmountChanged?.Invoke(currentEnergy);
            timer = (currentEnergy >= maxEnergy) ? 0f : remainder;
        }
        else
        {
            if (currentEnergy >= maxEnergy) timer = 0f;
        }
    }


    public IEnumerator RegenerateEnergy()
    {
        while (true)
        {
            if (currentEnergy < maxEnergy)
            {
                timer += Time.deltaTime;

                if (timer >= energyPerTenMinutes * 60)
                {
                    currentEnergy++;
                    EnergyAmountChanged?.Invoke(currentEnergy);
                    timer = 0f;
                }

                UpdateTimerValues();
            }

            yield return null;
        }
    }

    public IEnumerator Regenerate()
    {
        while (true)
        {
            if (currentEnergy < maxEnergy)
            {
                timer += Time.unscaledDeltaTime;

                if (timer >= IntervalSec)
                {
                    int gained = (int)(timer / IntervalSec);  
                    timer = timer % IntervalSec;

                    currentEnergy = Mathf.Min(currentEnergy + gained, maxEnergy);
                    EnergyAmountChanged?.Invoke(currentEnergy);
                    if (currentEnergy >= maxEnergy)
                        timer = 0f;
                }

                UpdateUI();
            }

            yield return null;
        }
    }

    private void UpdateTimerValues()
    {
        float timeRemaining = energyPerTenMinutes * 60 - timer;
        TimeSpan ts = TimeSpan.FromSeconds(timeRemaining);
        EnergyTimerTicked?.Invoke(ts);
    }

    private void UpdateUI()
    {
        float timeRemaining = Mathf.Clamp(IntervalSec - timer, 0f, IntervalSec);
        var ts = TimeSpan.FromSeconds(timeRemaining);
        EnergyTimerTicked?.Invoke(ts);
    }
}
