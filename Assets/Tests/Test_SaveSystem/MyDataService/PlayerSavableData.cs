using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerSavableData : ISavable
{
    public int CoinScore;
    public float Health;
    public int[] Achievements;
    public Vector3 Position;

    public PlayerSavableData()
    {
        CoinScore = 99;
        Health = 3;
        Achievements = new int[0];
        Position = Vector3.zero;
    }
}
