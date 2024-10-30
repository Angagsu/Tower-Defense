using System;
using System.Collections.Generic;
using UnityEngine;

public class GeneralMonsterFactory : MonsterFactory
{
    public override event Action<BaseMonster> PoolChanged;
    public override BaseMonster GetFactoryType { get => factorieType; }

    public List<BaseMonster> GeneralMonsters;
    public int Capacity = 3;

    private GeneralMonster factorieType;
    private Pool<BaseMonster> pool;

    

    private void Awake()
    {
        factorieType = GeneralMonsters[0].GetComponent<GeneralMonster>();
        pool = new Pool<BaseMonster>(GeneralMonsters, Capacity, transform);
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
