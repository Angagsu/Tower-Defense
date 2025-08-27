using Assets.Scripts;
using System;
using System.Collections;
using UnityEngine;


public class BaseHero : Character, IAttackableHero
{
    public event Action Died;
    public event Action<BaseHero, float, float> DamageTaked;

    [field: SerializeField] public int ReviveTimer { get; private set; }
    [field: SerializeField] public Transform RotatPart { get; private set; }
    [field: SerializeField] public BaseHeroAnimation Anim { get; private set; }

    public BaseMonster CurrentTargetedMonster => currentTargetedMonster;
    public BaseMonster CurrentAttackerMonster => currentAttackerMonster;

    public bool IsSelected => isSelected;
    public bool CanAttack => canAttack;
    public float AttackRange => attackRange;
    public bool IsPaused => isPaused;

    [SerializeField] protected BaseAttack attack;
    [SerializeField] protected BaseDetection detect;
    [SerializeField] protected BaseMovement move;
    [SerializeField] protected BaseSFXHandler baseSFX;

    [SerializeField] protected float reviveAnimationDuration;
    [SerializeField] protected float dyingAnimationDuration;
    [SerializeField] protected float startHealth;
    [SerializeField] protected float detectionRange;
    [SerializeField] protected GameObject selectionZone;

    protected PlayerInputHandler playerInputHandler;
    protected GameplayStates gameplayStates;
    protected BaseMonster currentTargetedMonster;
    protected BaseMonster currentAttackerMonster;
    protected Transform target;
    protected Coroutine coroutine;
    protected float attackCountdown = 0f;
    protected bool isSelected;
    protected bool isPaused;
    protected bool canAttack;
    protected Vector2 touchPosition;

    protected HeroMovement movement;
        


    public virtual void Construct(PlayerInputHandler playerInputHandler,GameplayStates gameplayStates, ProjectilesFactoriesService projectilesFactoriesService)
    {
        this.playerInputHandler = playerInputHandler;
        this.gameplayStates = gameplayStates;

        ConstructAttackType(projectilesFactoriesService);

        this.playerInputHandler.TouchPressed += OnPlayerInputHandler_Touched;
        this.gameplayStates.Paused += OnGameplayPause;
        this.gameplayStates.Unpaused += OnGameplayUnpause;
    }

    private void ConstructAttackType(ProjectilesFactoriesService projectilesFactoriesService)
    {
        if (attack is RangeAttack)
        {
            attack.Costruct(projectilesFactoriesService);
        }
    }


    protected virtual void Awake()
    {
        movement = move as HeroMovement;
        isSelected = false;
        canAttack = true;
        health = startHealth;
        instancingEnabler.gameObject.SetActive(false);
        selectionZone.SetActive(false);

        //DamageTaked?.Invoke(this, health, startHealth);
    }

    protected virtual void OnEnable() { }

    protected virtual void OnGameplayUnpause()
    {
        isPaused = false;
        Anim.Unpause();
    }

    protected virtual void OnGameplayPause()
    {
        isPaused = true;
        Anim.Pause();
    }

    protected virtual void OnDisable()
    {
        playerInputHandler.TouchPressed -= OnPlayerInputHandler_Touched;

        gameplayStates.Paused -= OnGameplayPause;
        gameplayStates.Unpaused -= OnGameplayUnpause;
    }

    protected void OnPlayerInputHandler_Touched(Vector2 touchPosition)
    {
        this.touchPosition = touchPosition;
        Move(target);
    }

    protected virtual void Update()
    {
        if (isPaused) return;

        if ((!move.IsMoves && !IsDead))
        {
            DetectTarget();
        }
        else if(currentTargetedMonster)
        {
            target = null;
            currentTargetedMonster.RejectCurrentAttackerHero();
            currentTargetedMonster = null;
        }
    }

    protected override void DetectTarget()
    {
        if ((target = detect.DetectTarget(detectionRange, IsDead)))
        {
            var distance = Vector3.Distance(movement.PreviousPosition, target.position);

            if (distance < detectionRange && !movement.IsMoves)
            {
                currentTargetedMonster = detect.DetectedMonster;

                if (currentTargetedMonster.CurrentAttackerHero == null)
                {
                    currentTargetedMonster.SetCurrentAttackerHero(this);
                    currentTargetedMonster.Movement.SetIsMove(false);
                    currentTargetedMonster.Anim.StopAllAnimations();
                    movement.MoveToTargetMonster(moveSpeed, turnSpeed, target.position);
                }

                LockOnTarget();

                if (canAttack && !currentTargetedMonster.Movement.IsMoves)
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
                    currentTargetedMonster = null;
                    Anim.SetAttackAnimation(false);
                }
            }
            else
            {
                if (currentTargetedMonster != null)
                {
                    currentTargetedMonster.RejectCurrentAttackerHero();
                }

                currentTargetedMonster = null;
                Anim.SetAttackAnimation(false);

                if (!movement.IsMoves && !movement.IsOnPreviosPosition)
                {
                    movement.ReturnToPreviousPosition(moveSpeed, turnSpeed, movement.PreviousPosition);
                }
            }
        }
        else
        {
            if (currentTargetedMonster != null)
            {
                currentTargetedMonster.RejectCurrentAttackerHero();
            }

            currentTargetedMonster = null;
            Anim.SetAttackAnimation(false);

            if (!movement.IsMoves && !movement.IsOnPreviosPosition)
            {
                movement.ReturnToPreviousPosition(moveSpeed, turnSpeed, movement.PreviousPosition);
            }
        }

        attackCountdown -= Time.deltaTime;
    }

    protected override void Attack(Transform target)
    {
        baseSFX.PlayAttackSFX();
        attack.AttackTarget(target, damage, currentTargetedMonster);
    }

    protected override void Move(Transform target)
    {
        movement.MoveToTarget(moveSpeed, turnSpeed, touchPosition);
    }

    protected void LockOnTarget()
    {
        Vector3 direction = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction.normalized);
        Vector3 rotation = Quaternion.Lerp(RotatPart.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        RotatPart.rotation = Quaternion.Euler(0, rotation.y, 0);
    } 

    public override void TakeDamage(float amount)
    {
        instancingEnabler.gameObject.SetActive(true);
        health -= amount;
        instancingEnabler.SetMaterialProperty((health / startHealth));
        DamageTaked?.Invoke(this, health, startHealth);

        if (health <= 0 && !isDead)
        {
            OnDie();
        }
    }

    protected override void OnDie()
    {
        instancingEnabler.gameObject.SetActive(false);
        isDead = true;
        Anim.SetAttackAnimation(false);
        Anim.SetMoveAnimation(false);
        Anim.SetDeadAnimation(true);
        Died?.Invoke();

        if (currentTargetedMonster)
        {
            currentTargetedMonster.RejectCurrentAttackerHero();
            currentTargetedMonster = null;
            target = null;
        }

        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        coroutine = StartCoroutine(OnDyingAnimationStarted());
    }

    public override void OnRevive()
    {
        Anim.SetDeadAnimation(false);
        Anim.SetReviveAnimation(true);

        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        coroutine = StartCoroutine(OnRevivingAnimationStarted());        
    }      

    protected virtual IEnumerator OnDyingAnimationStarted()
    {
        yield return new WaitForSeconds(dyingAnimationDuration);

        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        coroutine = StartCoroutine(TimerBeforeRevive());
    }

    protected virtual IEnumerator TimerBeforeRevive()
    {
        yield return new WaitForSeconds(ReviveTimer);

        OnRevive();
    }

    protected virtual IEnumerator OnRevivingAnimationStarted()
    { 
        yield return new WaitForSeconds(reviveAnimationDuration);

        Anim.SetReviveAnimation(false);
        health = startHealth;

        instancingEnabler.gameObject.SetActive(true);
        instancingEnabler.SetMaterialProperty((health / startHealth));
        DamageTaked?.Invoke(this, health, startHealth);
        isDead = false;  
    }

    public void SetCanAttack(bool tof)
    {
        canAttack = tof;
    }

    public virtual void Select()
    {
        isSelected = true;
        selectionZone.SetActive(true);
    }

    public virtual void Deselect()
    {
        isSelected = false;
        selectionZone.SetActive(false);
    }

    public void SetCurrentAttackerMonster(BaseMonster monster)
    {
        currentAttackerMonster = monster;
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.green;
        //Gizmos.DrawWireSphere(transform.position, attackRange);
        //Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
