using System;
using UnityEngine;

public abstract class BaseMonsterFactory : MonoBehaviour
{
    public abstract event Action<BaseMonster> PoolChanged;

    public virtual BaseMonster GetFactoryType { get; private set; }

    public virtual T GetMonsterByType<T> (T type, Transform transform, Quaternion rotation) where T : BaseMonster { return default; }
}
