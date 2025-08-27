using UnityEngine;

public class BaseMonsterAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    
    private int isMovesHash;
    private int isDeadHash;
    private int isAttackHash;

    private void Awake()
    {
        isMovesHash = Animator.StringToHash("isMoves");
        isDeadHash = Animator.StringToHash("isDead");
        isAttackHash = Animator.StringToHash("isAttack");
    }


    public void SetDeadAnimation(bool tof)
    {
        animator.SetBool(isDeadHash, tof);
    }

    public void SetMoveAnimation(bool tof)
    {
        animator.SetBool(isMovesHash, tof);
    }

    public void SetAttackAnimation(bool tof)
    {
        animator.SetBool(isAttackHash, tof);
    }

    public void StopAllAnimations()
    {
        animator.SetBool(isDeadHash, false);
        animator.SetBool(isAttackHash, false);
        animator.SetBool(isMovesHash, false);
    }

    public void Pause()
    {
        animator.speed = 0;
    }

    public void Unpause()
    {
        animator.speed = 1;
    }
}
