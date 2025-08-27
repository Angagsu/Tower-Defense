using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Wave 
{
    public List<Pathway> Patways;
    public int CountOfEnemyToSpawn;
    public float RateOfSpawn;
}

[Serializable]
public class Pathway
{
    public GameObject[] EnemyPrefab;
}
