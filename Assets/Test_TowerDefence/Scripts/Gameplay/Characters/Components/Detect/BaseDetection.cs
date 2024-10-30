using UnityEngine;

public abstract class BaseDetection : MonoBehaviour
{
    public abstract Transform DetectTarget(float attackRange, bool isDead);
}
