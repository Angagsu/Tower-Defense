using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSavableDataHandler : MonoBehaviour , ISavablePlayerData
{
    public int[] achievements;
    public int coinScore;
    public float health;
    public Vector3 Position;

    public void LoadData(PlayerSavableData data)
    {
        achievements = data.Achievements;
        coinScore = data.CoinScore;
        health = data.Health;
        transform.position = data.Position;
        Position = data.Position;
    }

    public void SaveData(ref PlayerSavableData data)
    {
        data.Achievements = achievements;
        data.CoinScore = coinScore;
        data.Health = health;
        data.Position = transform.position;
    }

}
