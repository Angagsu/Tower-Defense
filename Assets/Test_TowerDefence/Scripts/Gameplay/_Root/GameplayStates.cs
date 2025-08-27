using System;
using UnityEngine;

[DefaultExecutionOrder(-20)]
public class GameplayStates : MonoBehaviour
{
    public event Action Paused;
    public event Action Unpaused;
    //public event Action<GameplayState> StateChanged;

    public GameplayState State { get; private set; }

    private bool isPaused;


    private void Start()
    {
        isPaused = true;
        SetStateStart();
    }

    public void SetStateStart()
    {
        if (isPaused)
        {
            State = GameplayState.Start;
            Unpaused?.Invoke();
            isPaused = false;
        }
        else 
        {
            State = GameplayState.Start;
            Paused?.Invoke();
            isPaused = true;
        } 
    }

    public void SetStatePlay()
    {
        State = GameplayState.Play;
        Unpaused?.Invoke();
    }

    public void SetStateComplete()
    {
        State = GameplayState.Complete;
        Paused?.Invoke();
    }

    public void SetStateDefeat()
    {
        State = GameplayState.Defeat;
        Paused?.Invoke();
    }

    public void SetStatePause()
    {
        State = GameplayState.Pause;
        Paused?.Invoke();
    }

    public void SetGameplaySpeed(float timeScale)
    {
        Time.timeScale = timeScale;
    }
}

public enum GameplayState
{
    Start,
    Play,
    Pause,
    Defeat,
    Complete
}
