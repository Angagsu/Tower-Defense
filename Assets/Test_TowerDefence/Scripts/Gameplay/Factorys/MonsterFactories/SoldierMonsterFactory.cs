using System;
using System.Collections.Generic;
using UnityEngine;

public class SoldierMonsterFactory : MonsterFactory
{
    public override event Action<BaseMonster> PoolChanged;
    public override BaseMonster GetFactoryType { get => factorieType; }

    public List<BaseMonster> SoldierMonsters;
    public int Capacity = 3;

    private SoldierMonster factorieType;
    private Pool<BaseMonster> pool;

    
    private void Awake()
    {
        factorieType = SoldierMonsters[0].GetComponent<SoldierMonster>();
        pool = new Pool<BaseMonster>(SoldierMonsters, Capacity, transform);
        pool.PoolChanged += OnPoolChanged;
        pool.AutoExpand = true;
    }

    public override T GetMonsterByType<T>(T monster, Transform transform, Quaternion rotation)
    {
        return (T)pool.GetFreeElement(monster, transform, rotation);     
    }

    private void OnPoolChanged<T>(T type) where T : BaseMonster
    {
        PoolChanged?.Invoke(type);
    }

    private void OnDestroy()
    {
        pool.PoolChanged -= OnPoolChanged;
    }
}

   

