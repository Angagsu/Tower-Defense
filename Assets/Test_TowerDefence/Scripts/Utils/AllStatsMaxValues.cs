using System.Collections.Generic;
using UnityEngine;

public static class HeroAllStatsNames
{
    public const string Health = "HP";
    public const string Damage = "ATK";   
    public const string MoveSpeed = "MS";

    public static List<string> StatsNames;


    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        StatsNames = new List<string>
        {
            Health,
            Damage,
            MoveSpeed
        };
    }
}

public static class TowerAllStatsNames
{
    public const string Health = "HP";
    public const string Damage = "ATK";
    public const string AttackRange = "ATK Range";
    public const string AttackRate = "ATK Rate";
    public const string BuyCost = "Cost";
    public const string TroopsHealth = "Troops HP";

    public static List<string> StatsNames;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        StatsNames = new List<string>
        {
            Health,
            Damage,
            AttackRange,
            AttackRate,
            BuyCost,
            TroopsHealth
        };
    }
}

public static class MonsterAllStatsNames
{
    public const string Health = "HP";
    public const string Damage = "ATK";
    public const string MoveSpeed = "MS";
    public const string AttackRange = "ATK Range";
    public const string UniqueSkill = "Unique Skill";

    public static List<string> StatsNames;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        StatsNames = new List<string>
        {
            Health,
            Damage,
            MoveSpeed,
            AttackRange,
        };
    }
}
