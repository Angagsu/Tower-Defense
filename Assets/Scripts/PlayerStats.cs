using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static int Money;
    public static int Lives;
    [SerializeField] private int startMoney = 400;
    [SerializeField] private int startLives = 20;


    void Start()
    {
        Money = startMoney;
        Lives = startLives;
    }

    
}
