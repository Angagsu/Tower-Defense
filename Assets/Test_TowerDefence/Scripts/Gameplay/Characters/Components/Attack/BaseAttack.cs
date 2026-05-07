using Assets.Scripts;
using UnityEngine;

public abstract class BaseAttack : MonoBehaviour
{
    public virtual void Costruct(ProjectilesFactoriesService projectilesFactoriesService) { }

    public abstract void AttackTarget(AttackStats stats, Transform target, float damage, Character targetCharacter = null, IAttackable attackableTarget = null);

    public virtual void AttackWithPassiveSkill(AttackStats stats, Transform target, float damage, bool hasPassiveSkill = default, float passiveValue_1 = default, float passiveValue_2 = default,  Character targetCharacter = null, IAttackable attackableTarget = null) { }

    public virtual void AttackWithPassiveSkillAndDifferentHands(AttackStats stats, Transform target, float damage, bool hasPassiveSkill = default, float passiveValue_1 = default, float passiveValue_2 = default, Character targetCharacter = null, bool isRightHandAttack = default, IAttackable attackableTarget = null) { }

}
