using Assets.Scripts;
using System;
using System.Collections;
using UnityEngine;


public class BaseHero : Character, IAttackableHero
{
    public event Action Died;

    [field: SerializeField] public int ReviveTimer { get; private set; }
    [field: SerializeField] public Transform RotatPart { get; private set; }
    [field: SerializeField] public BaseHeroAnimation Anim { get; private set; }

    public BaseMonster CurrentAttackerMonster { get; set; }
    public bool IsSelected => isSelected;

    [SerializeField] protected BaseAttack attack;
    [SerializeField] protected BaseDetection detect;
    [SerializeField] protected BaseMovement move;
    [SerializeField] protected float reviveAnimationDuration;
    [SerializeField] protected float startHealth;

    protected PlayerInputHandler playerInputHandler;
    protected Transform target;
    protected Coroutine coroutine;
    protected float attackCountdown = 0f;
    protected bool isSelected;
    protected Vector2 touchPosition;

    private HeroMovement movement; 


    protected virtual void Awake()
    {
        playerInputHandler = PlayerInputHandler.Instance;
        movement = move as HeroMovement;
        isSelected = false;
        health = startHealth;
    }

    private void OnEnable()
    {
        playerInputHandler.TouchPressed += OnPlayerInputHandler_Touched;
    }

    private void OnPlayerInputHandler_Touched(Vector2 touchPosition)
    {
        this.touchPosition = touchPosition;
        Move(target);
    }

    private void OnDisable()
    {
        playerInputHandler.TouchPressed -= OnPlayerInputHandler_Touched;
    }

    protected virtual void Update()
    {   
        if (!move.IsMoves && !IsDead)
        {
            DetectTarget();
        }

        if (!move.IsMoves && target)
        {
            LockOnTarget();
        }
    }

    protected override void DetectTarget()
    {
        if ((target = detect.DetectTarget(attackRange, IsDead)))
        {
            Anim.SetAttackAnimation(true);

            if (attackCountdown <= 0)
            {
                attackCountdown = 1 / attackRate;

                Attack(target);
            }
        }
        else
        {
            CurrentAttackerMonster = null;
            Anim.SetAttackAnimation(false);
        }

        attackCountdown -= Time.deltaTime;
    }

    protected override void Attack(Transform target)
    {
        attack.AttackTarget(target, damage);
    }

    protected override void Move(Transform target)
    {
        movement.MoveToTarget(this, moveSpeed, turnSpeed, touchPosition);
    }

    protected void LockOnTarget()
    {
        Vector3 direction = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Vector3 rotation = Quaternion.Lerp(RotatPart.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        RotatPart.rotation = Quaternion.Euler(0, rotation.y, 0);
    }

    protected override void OnDie()
    {
        isDead = true;
        Anim.SetDeadAnimation(true);
        Died?.Invoke();
    }

    public override void TakeDamage(float amount)
    {
        health -= amount;
        healthBar.fillAmount = health / startHealth;

        if (health <= 0 && !isDead)
        {
            OnDie();
        }
    }

    public override void OnRevive()
    {
        Anim.SetDeadAnimation(false);
        Anim.SetReviveAnimation(true);
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        coroutine = StartCoroutine(OnReviveAnimationEnded());        
    }  

    protected virtual IEnumerator OnReviveAnimationEnded()
    { 
        yield return new WaitForSeconds(reviveAnimationDuration);

        Anim.SetReviveAnimation(false);
        health = startHealth;
        healthBar.fillAmount = 1f;
        isDead = false;
        CurrentAttackerMonster = null;
    }

    public void Select()
    {
        isSelected = true;
    }

    public void Deselect()
    {
        isSelected = false; 
    }

    public void SetCurrentAttackerEnemy(BaseMonster monster)
    {
        CurrentAttackerMonster = monster;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
