using System;
using System.Collections.Generic;
using UnityEngine;

public class BossMonsterFactory : MonsterFactory
{
    public override event Action<BaseMonster> PoolChanged;
    public override BaseMonster GetFactoryType => factorieType;

    public List<BaseMonster> BossMonsters;
    public int Capacity = 3;

    private BossMonster factorieType;
    private Pool<BaseMonster> pool;

    

    private void Awake()
    {
        factorieType = BossMonsters[0].GetComponent<BossMonster>();
        pool = new Pool<BaseMonster>(BossMonsters, Capacity, transform);
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
