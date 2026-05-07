using UnityEngine;

public class BaseTowerAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private int isCrashed;
    private int isLeftAttack;
    private int isRightAttack;
   // private int isAttacking;
   // private int attackSpeed;


    private void Awake()
    {
        isCrashed = Animator.StringToHash("isDead");
        isLeftAttack = Animator.StringToHash("isLeftAttack");
        isRightAttack = Animator.StringToHash("isRightAttack");
        //isAttacking = Animator.StringToHash("isAttacking");
        //attackSpeed = Animator.StringToHash("attackSpeed");
       
    }

    public void SetAttackAnimationsSpeed(float speed)
    {
        animator.speed = speed; 
    }

    public void SetCrashedAnimation(bool tof)
    {
        animator.SetBool(isCrashed, tof);
    }

    public void SetLeftHandAttackAnimation(bool tof)
    {
        animator.SetBool(isLeftAttack, tof);
    }

    public void SetRightHandAttackAnimation(bool tof)
    {
        animator.SetBool(isRightAttack, tof);
    }

    public void SetAnimationSpeed(float speed)
    {
        //animator.SetFloat(attackSpeed, speed);
    }

    public void SetAllAnimationsFalse()
    {
        animator.SetBool(isLeftAttack, false);
        animator.SetBool(isRightAttack, false);
        animator.SetBool(isCrashed, false);
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
