using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseProjectileFactory : MonoBehaviour  
{
    public virtual event Action<BaseProjectile> PoolChanged;

    [field: SerializeField] public ProjectileUpgradeLevel ProjectileUpgradeLevel {  get; private set; }

    [SerializeField] protected List<BaseProjectile> Projectiles;
    [SerializeField] protected int Capacity = 3;

    protected Pool<BaseProjectile> pool;



    protected virtual void Awake()
    {
        pool = new Pool<BaseProjectile>(Projectiles, Capacity, transform);
        pool.PoolChanged += OnPoolChanged;
        pool.AutoExpand = true;
    }

    public virtual T GetProjectileByType<T>(T type, Transform transform, Quaternion rotation) where T : BaseProjectile
    {
        return (T)pool.GetFreeElement(type, transform, rotation);
    }

    protected void OnPoolChanged<T>(T type) where T : BaseProjectile
    {
        PoolChanged?.Invoke(type);
    }

    protected void OnEnable()
    {
        pool.PoolChanged -= OnPoolChanged;
    }
}
