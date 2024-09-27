using UnityEngine;

public class BaseMonsterAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private int isMovesHash;
    private int isDeadHash;

    private void Awake()
    {
        isMovesHash = Animator.StringToHash("isMoves");
        isDeadHash = Animator.StringToHash("isDead");
    }

    public void SetDeadAnimation(bool tf)
    {
        animator.SetBool(isDeadHash, tf);
    }

    public void SetMoveAnimation(bool tf)
    {
        animator.SetBool(isMovesHash, tf);
    }
}
