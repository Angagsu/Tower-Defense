using System;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilesFactory : MonoBehaviour  
{
    public event Action<BaseProjectile> PoolChanged;
    public BaseMonster GetFactoryType { get => factorieType; }

    public List<BaseProjectile> Projectiles;
    public int Capacity = 3;

    private SoldierMonster factorieType;
    private Pool<BaseProjectile> pool;


    private void Awake()
    {
        factorieType = Projectiles[0].GetComponent<SoldierMonster>();
        pool = new Pool<BaseProjectile>(Projectiles, Capacity, transform);
        pool.PoolChanged += OnPoolChanged;
        pool.AutoExpand = true;
    }

    public T GetProjectileByType<T>(T type, Transform transform, Quaternion rotation) where T : BaseProjectile
    {
        return (T)pool.GetFreeElement(type, transform, rotation);
    }

    private void OnPoolChanged<T>(T type) where T : BaseProjectile
    {
        PoolChanged?.Invoke(type);
    }

    private void OnDestroy()
    {
        pool.PoolChanged -= OnPoolChanged;
    }
}
