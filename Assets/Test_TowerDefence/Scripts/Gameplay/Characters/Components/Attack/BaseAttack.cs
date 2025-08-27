using Assets.Scripts;
using UnityEngine;

public abstract class BaseAttack : MonoBehaviour
{
    public abstract void AttackTarget(Transform target, float damage, Character targetCharacter = null);

    public virtual void Costruct(ProjectilesFactoriesService projectilesFactoriesService) { }
}
