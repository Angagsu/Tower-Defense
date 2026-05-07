using System.Collections.Generic;


public static class HeroAllStatsNames
{
    public const string Health = "HP";
    public const string Damage = "ATK";   
    public const string MoveSpeed = "MS";
  

    public static List<string> StatsNames = new List<string>
    {
        Health,
        Damage,
        MoveSpeed
    };
}

public static class TowerAllStatsNames
{
    public const string Health = "HP";
    public const string Damage = "ATK";
    public const string AttackRange = "ATK Range";
    public const string AttackRate = "ATK Rate";
    public const string BuyCost = "Cost";
    public const string TroopsHealth = "Troops HP";


    public static List<string> StatsNames = new List<string>
    {
        Health,
        Damage,
        AttackRange,
        AttackRate,
        BuyCost,
        TroopsHealth
    };
}
