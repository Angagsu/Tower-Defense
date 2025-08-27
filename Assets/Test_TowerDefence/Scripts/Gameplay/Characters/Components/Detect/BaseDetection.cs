using Assets.Scripts;
using UnityEngine;

public abstract class BaseDetection : MonoBehaviour
{
    public BaseMonster DetectedMonster { get; set; }
    public BaseHero DetectedHero { get; set; }
    public abstract Transform DetectTarget(float attackRange, bool isDead);
}
