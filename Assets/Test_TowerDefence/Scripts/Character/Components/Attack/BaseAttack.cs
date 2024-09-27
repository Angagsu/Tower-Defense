using UnityEngine;

public abstract class BaseAttack : MonoBehaviour
{
    public abstract void AttackTarget(Transform target, float damage);
}
