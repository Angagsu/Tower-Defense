using UnityEngine;

public abstract class BaseDetect : MonoBehaviour
{
    public abstract Transform DetectTarget(float attackRange, bool isDead);
}
