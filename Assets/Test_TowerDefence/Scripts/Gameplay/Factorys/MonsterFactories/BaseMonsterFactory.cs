using UnityEngine;

public abstract class BaseMonsterFactory : MonoBehaviour
{
    public virtual BaseMonster GetFactoryType { get; private set; }

    public virtual T GetMonsterByType<T> (T type, Transform transform, Quaternion rotation) where T : BaseMonster { return default; }
}
