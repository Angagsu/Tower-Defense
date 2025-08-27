using UnityEngine;
using Assets.Scripts;
using Assets.Scripts.Tower;
using System.Collections;


public class BaseMonster : Character, IAttackableMonster
{
    public GameplayPlayerDataHandler GameplayPlayerDataHandler { get; private set; }
    public MonstersFactoriesService MonstersFactoriesService { get; private set; }
    public MonsterMovement Movement { get; private set; }
    public BaseMonsterAnimation Anim => anim;

    public float AttackRange => attackRange;
    public BaseHero CurrentAttackerHero => currentAttackerHero;
    public BaseHero CurrentTargetedHero => currentTargetedHero;

    [SerializeField] protected BaseMonsterAnimation anim;
    [SerializeField] protected BaseAttack attack;
    [SerializeField] protected BaseDetection detect;
    [SerializeField] protected BaseMovement move;
    [SerializeField] protected float startHealth;
    [SerializeField] protected int moneyGain = 50;
    [SerializeField] protected float dyingAnimationDuration;

    protected GameplayStates gameplayStates;
    protected Coroutine coroutine;
    protected BaseTower tower;
    protected Transform target;

    protected BaseHero currentTargetedHero;
    protected BaseHero currentAttackerHero;
    protected float startMoveSpeed;
    protected float attackCountdown = 0f;
    protected bool isSelected;
    protected bool canAttack;
    protected bool isPaused;



    public virtual void Construct(GameplayPlayerDataHandler gameplayPlayerDataHandler,
        GameplayStates gameplayStates, MonstersFactoriesService monstersFactoriesService,
        ProjectilesFactoriesService projectilesFactoriesService)
    {
        this.gameplayStates = gameplayStates;
        this.MonstersFactoriesService = monstersFactoriesService;
        this.GameplayPlayerDataHandler = gameplayPlayerDataHandler;

        Movement.Construct();
        ConstructAttackType(projectilesFactoriesService);

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
        Movement = move as MonsterMovement; 
        health = startHealth;
        instancingEnabler.gameObject.SetActive(false);
        canAttack = true;
        
        SetStartMoveSpeed();
    }

    protected virtual void OnEnable()
    {
        OnRevive();
    }

    protected void OnGameplayUnpause()
    {
        isPaused = false;
        anim.Unpause();
    }

    protected void OnGameplayPause()
    {
        isPaused = true;
        anim.Pause();
    }

    protected virtual void OnDisable()
    {
        gameplayStates.Paused -= OnGameplayPause;
        gameplayStates.Unpaused -= OnGameplayUnpause;
    }

    protected virtual void Update()
    {
        if (isDead || isPaused)
        {
            return;
        }

        DetectTarget();

        if (!Movement.IsMoves)
        {
            return;
        }

        Move(target);
    }

    protected override void DetectTarget()
    {
        if (currentAttackerHero && currentAttackerHero.CanAttack && Vector3.Distance(target.position, transform.position) <= attackRange + 1)
        {
            Movement.LockOnTarget(target, turnSpeed);

            if (attackCountdown <= 0)
            {
                attackCountdown = 1 / attackRate;
        
                Attack(target);

                anim.SetAttackAnimation(true);
            } 
        }
        else 
        {
            anim.SetAttackAnimation(false);
        }

        attackCountdown -= Time.deltaTime;
    }

    protected override void Attack(Transform target)
    {
        attack.AttackTarget(target, damage, CurrentTargetedHero);
    }

    public override void TakeDamage(float amount)
    {
        instancingEnabler.gameObject.SetActive(true);
        health -= amount;

        instancingEnabler.SetMaterialProperty((health / startHealth));

        if (health <= 0 && !IsDead)
        {
            OnDie();
        }
    }

    protected override void OnDie()
    {
        isDead = true;
        MonstersFactoriesService.ReduceAliveEnemiesAmount();
        CalculateMoneyForKillingEnemy();

        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        target = null;
        currentAttackerHero = null;
        currentTargetedHero = null;

        coroutine = StartCoroutine(DyingAnimationDuration());
    }
    
    protected virtual IEnumerator DyingAnimationDuration()
    {
        anim.SetDeadAnimation(true);
        float timer = dyingAnimationDuration;

        while (timer > 0)
        {
            while(isPaused) yield return null;

            timer -= Time.deltaTime;

            yield return null;
        }

        gameObject.SetActive(false);
    }

    protected override void Move(Transform target)
    {
        Movement.Move(target, startMoveSpeed, turnSpeed);
    }    

    private void CalculateMoneyForKillingEnemy()
    {
        GameplayPlayerDataHandler.IncreaseMoney(moneyGain);
    }

    public override void OnRevive()
    { 
        isDead = false;
        health = startHealth;

        instancingEnabler.SetMaterialProperty((health / startHealth));
        canAttack = true;

        SetDefaultMoveSpeed();
    }

    public void SetStartMoveSpeed()
    {
        startMoveSpeed = moveSpeed;
    }

    public void SetDefaultMoveSpeed()
    {
        moveSpeed = startMoveSpeed;
    }

    public void SetAttackedTower(BaseTower tower)
    {
        this.tower = tower;
    }

    public void SetIsMoves(bool tof)
    {
        Movement.SetIsMove(tof);
    }

    public void SetIsDead(bool tof)
    {
        isDead = tof;
    }

    public void Slow(float amount)
    {
        moveSpeed = startMoveSpeed * (1 - amount);
    }

    public void SetCurrentAttackerHero(BaseHero currentAttackerHero)
    {
        this.currentAttackerHero = currentAttackerHero;
        currentTargetedHero = currentAttackerHero;
        target = this.currentAttackerHero.PartForTargeting;
    }

    public void RejectCurrentAttackerHero()
    {
        target = null;
        currentTargetedHero = null;
        currentAttackerHero = null;
        Movement.SetIsMove(true);
        anim.SetMoveAnimation(true);
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.blue;
        //Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
