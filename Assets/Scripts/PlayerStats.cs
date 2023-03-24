
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static int Money;
    public static int Lives;
    public static int Waves;
    [SerializeField] private int startMoney = 400;
    [SerializeField] private int startLives = 20;
    

    void Start()
    {
        Waves = 0;
        Money = startMoney;
        Lives = startLives;
    }
}
