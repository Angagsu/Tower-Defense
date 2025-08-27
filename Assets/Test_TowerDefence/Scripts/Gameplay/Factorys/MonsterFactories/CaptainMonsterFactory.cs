using System;
using System.Collections.Generic;
using UnityEngine;

public class CaptainMonsterFactory : BaseMonsterFactory
{
    public override event Action<BaseMonster> PoolChanged;

    public override BaseMonster GetFactoryType => factorieType;

    public List<BaseMonster> CaptainMonsters;
    public int Capacity = 3;

    private CaptainMonster factorieType;
    public Pool<BaseMonster> pool;

    private void Awake()
    {
        factorieType = CaptainMonsters[0].GetComponent<CaptainMonster>();
        pool = new Pool<BaseMonster>(CaptainMonsters, Capacity, transform);
        pool.PoolChanged += OnPoolChanged;
        pool.AutoExpand = true;
    }

    public override T GetMonsterByType<T>(T monsterPrefab, Transform transform, Quaternion rotation)
    {
        return (T)pool.GetFreeElement(monsterPrefab, transform, rotation);
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
