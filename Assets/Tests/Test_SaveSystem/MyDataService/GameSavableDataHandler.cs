using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSavableDataHandler : MonoBehaviour, ISavableGameData
{
    [SerializeField] private int gold;
    [SerializeField] private int rubin;
    [SerializeField] private int diamond;
    [SerializeField] private float money;

    public void LoadData(GameSavableData data)
    {
        gold = data.Gold;
        rubin = data.Rubin;
        diamond = data.Diamond;
        money = data.Money;
    }

    public void SaveData(ref GameSavableData data)
    {
        data.Gold = gold;
        data.Rubin = rubin;
        data.Diamond = diamond;
        data.Money = money;
    }

    
}
