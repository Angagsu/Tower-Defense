using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BaseHero : Character
{
    public event Action<bool> Died;
    public event Action<BaseHero, float, float> DamageTaked;
    public event Action<bool> CanUseSuperAttack;

    [field: SerializeField] public int ReviveTimer { get; private set; }
    [field: SerializeField] public Transform RotatPart { get; private set; }
    [field: SerializeField] public BaseHeroAnimation Anim { get; private set; }
    [field: SerializeField] public float ActiveSkillCooldown { get; private set; }
    public BaseMonster CurrentTargetedMonster => currentTargetedMonster;
    public BaseMonster CurrentAttackerMonster => currentAttackerMonster;

    public bool IsSelected => isSelected;
    public bool CanAttack => canAttack;    
    public bool IsMoves => movement.IsMoves;
    public bool CanDetect => canDetect;
    public bool CanMove => canMove;
    public bool IsPaused => isPaused;
    public float AttackRange => attackRange;

    [SerializeField] protected HeroSuperAttack superAttack;
    [SerializeField] protected BaseAttack attack;
    [SerializeField] protected BaseDetection detect;
    [SerializeField] protected BaseMovement move;
    [SerializeField] protected BaseSFXHandler baseSFX;

    [SerializeField] protected float reviveAnimationDuration;
    [SerializeField] protected float dyingAnimationDuration;
    [SerializeField] protected float startHealth;
    [SerializeField] protected float detectionRange;
    [SerializeField] protected float superAttackRadius;
    [SerializeField] protected float superAttackEffectDuration;
    [SerializeField] protected GameObject selectionZone;

    protected HeroUpgradeLevelSO upgradeLevel;
    protected PlayerInputHandler playerInputHandler;
    protected GameplayStates gameplayStates;
    protected GameplayHeroActiveSkillUI activeSkillUI;
    protected BaseMonster currentTargetedMonster;
    protected BaseMonster currentAttackerMonster;
    protected Transform target;

    protected Coroutine coroutine;
    protected Coroutine buffCoroutine;
    protected WaitForSeconds waitUntilReviveAnimationEnd;
    protected WaitForSeconds waitUntilRevive;
    protected WaitForSeconds waitUntilDyingAnimationEnd;


    protected float attackCountdown = 0f;
    protected bool isSelected;
    protected bool isPaused;
    protected bool canAttack;
    protected bool canDetect;
    protected bool canMove;

    protected Vector2 touchPosition;
    protected HeroMovement movement;
    protected List<float> passiveSkillsList;
    protected bool hasPassiveSkill;
    protected bool hasActiveSkill;
    protected bool canUseSuperAttack;

    protected float magicResistStartValue;
    protected float physicResistStartValue;

    public HeroUpgradeLevelSO UpgradeLevel => upgradeLevel;
    public List<float> PassiveSkills => passiveSkillsList;
    public GameplayStates GameplayStates => gameplayStates;

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

    public virtual void SetHeroParams(HeroSO heroSO)
    {
        upgradeLevel = heroSO.CurrentUpgradeLevel;

        SetupAttackStats(heroSO.CurrentUpgradeLevel.AttackStats);

        moveSpeed = upgradeLevel.MoveSpeed;

        if (upgradeLevel.PassiveSkillsAccessibility.Capacity != 0)
        {
            hasPassiveSkill = true;
        }
        if (upgradeLevel.ActiveSkillAccessibility == true)
        {
            hasActiveSkill = true;
        }

        superAttack.SetDependencies(this, playerInputHandler);
    }

    protected virtual void SetupAttackStats(AttackStats attackStats)
    {
        AttackStats.Health = attackStats.Health;
        AttackStats.MaxHealth = attackStats.MaxHealth;

        AttackStats.DamageData = attackStats.DamageData;

        AttackStats.PhysicalResistance = attackStats.PhysicalResistance;
        AttackStats.MagicalResistance = attackStats.MagicalResistance;

        physicResistStartValue = AttackStats.PhysicalResistance;
        magicResistStartValue = AttackStats.MagicalResistance;
    }

    protected virtual void Awake()
    {
        movement = move as HeroMovement;

        isSelected = false;
        canAttack = true;
        canUseSuperAttack = true;
        canDetect = true;
        canMove = true;

        health = startHealth;
        healthBarInstance.gameObject.SetActive(false);
        selectionZone.SetActive(false);

        DamageTaked?.Invoke(this, health, startHealth);
        CanUseSuperAttack?.Invoke(canUseSuperAttack);

        waitUntilRevive = new WaitForSeconds(ReviveTimer);
        waitUntilReviveAnimationEnd = new WaitForSeconds(reviveAnimationDuration);
        waitUntilDyingAnimationEnd = new WaitForSeconds(dyingAnimationDuration);
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

        if (canDetect)
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
            var distance = (movement.PreviousPosition - target.position).sqrMagnitude;

            if (distance < (detectionRange * detectionRange) && !movement.IsMoves && !detect.DetectedMonster.IsKamikaze)
            {
                currentTargetedMonster = detect.DetectedMonster;

                if (currentTargetedMonster.CurrentAttackerHero == null) 
                {
                    currentAttackerMonster = currentTargetedMonster;

                    currentTargetedMonster.SetCurrentAttackerHero(this);
                    currentTargetedMonster.SetCurrentTargetedHero();

                    currentTargetedMonster.Movement.SetIsMove(false);
                    currentTargetedMonster.Anim.StopAllAnimations();
                    currentTargetedMonster.SetCanAttack(false);
                    movement.MoveToTargetMonster(currentTargetedMonster, moveSpeed, turnSpeed, target.position);
                }

                LockOnTarget();

                if (canAttack)
                {
                    currentAttackerMonster.SetCanAttack(true);
                    attackCountdown -= Time.deltaTime;

                    if (attackCountdown <= 0)
                    {
                        attackCountdown = attackRate;

                        Anim.SetAttackAnimation(true);
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
    }

    public void AttackOnAnimationTrigger()
    {
        if (target == null)
            return;

        Attack(target);
    }

    public virtual void SuperAttack() { }

    public virtual void OnSuperAttackAnimationTriggerStart() { }

    public virtual void OnSuperAttackAnimationTriggerEnd() { }

    public void CanSuperAttack(bool tof)
    {
        canUseSuperAttack = tof;
        CanUseSuperAttack?.Invoke(canUseSuperAttack);
    }

    public void SetActiveSkillButtonUI(GameplayHeroActiveSkillUI activeSkillUI) 
    {
        this.activeSkillUI = activeSkillUI;
    }

    public void IncreaseResistance(DamageType damageType, float amount, float effectDuration)
    {
        if (isDead) return;

        if (damageType == DamageType.Physical)
        {
            float resist = physicResistStartValue;
            float increaseValue = resist + amount;
            AttackStats.PhysicalResistance = Mathf.Max(physicResistStartValue, increaseValue);
        }
        else
        {
            float resist = magicResistStartValue;
            float increaseValue = resist + amount;
            AttackStats.MagicalResistance = Mathf.Max(magicResistStartValue, increaseValue);
        }

        buffCoroutine = StartCoroutine(BuffDurationCounter(effectDuration));
    }

    private IEnumerator BuffDurationCounter(float effectDuration)
    {
        yield return new WaitForSeconds(effectDuration);

        SetDefaultResistence();
    }

    public void SetDefaultResistence()
    {
        AttackStats.PhysicalResistance = physicResistStartValue;
        AttackStats.MagicalResistance = magicResistStartValue;
    }

    protected override void Attack(Transform target)
    {
        baseSFX.PlayAttackSFX();

        attack.AttackWithPassiveSkill(AttackStats,target, AttackStats.DamageData.DamageAmount, hasPassiveSkill, passiveValue_1: passiveSkillsList[0],
            passiveValue_2: passiveSkillsList[1], currentTargetedMonster, currentTargetedMonster);
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


    public override void TakeDamage(AttackStats enemy)
    {
        DoDamage.ApplyDamage(AttackStats, enemy.DamageData);

        healthBarInstance.gameObject.SetActive(true);
        //health -= amount;
        //healthBarInstance.SetMaterialProperty((health / startHealth));
        //DamageTaked?.Invoke(this, health, startHealth);

        healthBarInstance.SetMaterialProperty((AttackStats.Health / AttackStats.MaxHealth));
        DamageTaked?.Invoke(this, AttackStats.Health, AttackStats.MaxHealth);

        if (AttackStats.Health <= 0 && !isDead)
        {
            OnDie();
        }
    }

    protected override void OnDie()
    {
        healthBarInstance.gameObject.SetActive(false);
        isDead = true;
        canDetect = false;
        canUseSuperAttack = false;

        Anim.SetAttackAnimation(false);
        Anim.SetMoveAnimation(false);
        Anim.SetDeadAnimation(true);

        Died?.Invoke(isDead);
        CanUseSuperAttack?.Invoke(canUseSuperAttack);

        if (currentTargetedMonster)
        {
            currentTargetedMonster.RejectCurrentAttackerHero();
            currentTargetedMonster = null;
        }

        if (currentAttackerMonster)
        {
            currentAttackerMonster.RejectCurrentAttackerHero();
            currentAttackerMonster = null;
        }

        target = null;

        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        coroutine = StartCoroutine(OnDyingAnimationStarted());

        if(buffCoroutine  != null)
        {
            StopCoroutine(buffCoroutine);
        }
        
        SetDefaultResistence();
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
        yield return waitUntilDyingAnimationEnd;

        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        coroutine = StartCoroutine(TimerBeforeRevive());
    }

    protected virtual IEnumerator TimerBeforeRevive()
    {
        yield return waitUntilRevive;

        OnRevive();
    }

    protected virtual IEnumerator OnRevivingAnimationStarted()
    { 
        yield return waitUntilReviveAnimationEnd;

        Anim.SetReviveAnimation(false);
        AttackStats.Health = AttackStats.MaxHealth;

        healthBarInstance.gameObject.SetActive(true);
        healthBarInstance.SetMaterialProperty((AttackStats.Health / AttackStats.MaxHealth));
        DamageTaked?.Invoke(this, AttackStats.Health, AttackStats.MaxHealth);

        isDead = false;
        canUseSuperAttack = true;
        canDetect = true;

        Died?.Invoke(isDead);
        CanUseSuperAttack?.Invoke(canUseSuperAttack);
    }

    public void SetCanAttack(bool tof)
    {
        canAttack = tof;
    }

    public void SetCanDetect(bool tof)
    {
        canDetect = tof;
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

    public virtual void ResetTargetedMonster()
    {
        if (currentTargetedMonster)
        {
            Debug.Log("Reset Targeted Monster");
            currentTargetedMonster.RejectCurrentAttackerHero();
            Anim.SetAttackAnimation(false);
            currentTargetedMonster = null;
            target = null;
        }
    }

    public void ResetAttackerMoster()
    {
        if (currentAttackerMonster)
        {
            Debug.Log("Reset Attacker Monster");
            currentAttackerMonster.RejectCurrentAttackerHero();
            currentAttackerMonster = null;
            target = null;
        }
    }

    public bool IsMove()
    {
        return movement.IsMoves;
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.green;
    //    Gizmos.DrawWireSphere(transform.position, attackRange);
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, detectionRange);
    //}
}
