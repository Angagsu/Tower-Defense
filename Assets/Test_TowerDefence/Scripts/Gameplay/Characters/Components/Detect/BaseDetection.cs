using Assets.Scripts;
using Assets.Scripts.Tower;
using UnityEngine;

public abstract class BaseDetection : MonoBehaviour
{
    public BaseMonster DetectedMonster { get; protected set; }
    public BaseHero DetectedHero { get; protected set; }
    public BaseTower DetectedTower { get;  set; }
    public IAttackable AttackableTarget { get; protected set; }

    public virtual void ResetDetectedTarget() { }
    public abstract Transform DetectTarget(float attackRange, bool isDead);
}
