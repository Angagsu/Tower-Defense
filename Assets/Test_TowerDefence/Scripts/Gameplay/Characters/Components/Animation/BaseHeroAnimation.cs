using UnityEngine;

public class BaseHeroAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private int isMovesHash;
    private int isDeadHash;
    private int isAttackHash;
    private int isSuperAttackHash;
    private int isReviveHash;

    private void Awake()
    {
        isMovesHash = Animator.StringToHash("isMoves");
        isDeadHash = Animator.StringToHash("isDead");
        isAttackHash = Animator.StringToHash("isAttack");
        isSuperAttackHash = Animator.StringToHash("isSuperAttack");
        isReviveHash = Animator.StringToHash("isRevive");
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

    public void SetSupperAttackAnimation(bool tof)
    {
        animator.SetBool(isSuperAttackHash, tof);
    }

    public void SetReviveAnimation(bool tof)
    {
        animator.SetBool(isReviveHash, tof);
    }

    public void SetAllAnimationsFalse()
    {
        animator.SetBool(isAttackHash, false);
        animator.SetBool(isDeadHash, false);
        animator.SetBool(isMovesHash, false);
        animator.SetBool(isSuperAttackHash, false);
        animator.SetBool(isReviveHash, false);
    }
}
