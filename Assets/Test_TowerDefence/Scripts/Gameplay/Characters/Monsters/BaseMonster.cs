using UnityEngine;
using Assets.Scripts;
using Assets.Scripts.Tower;
using System.Collections;


public class BaseMonster : Character
{
    public GameplayPlayerDataHandler GameplayPlayerDataHandler { get; private set; }
    public MonstersFactoriesService MonstersFactoriesService { get; private set; }
    public MonsterMovement Movement { get; private set; }
    public BaseMonsterAnimation Anim => anim;

    public float AttackRange => attackRange;
    public BaseHero CurrentAttackerHero => currentAttackerHero;
    public BaseHero CurrentTargetedHero => currentTargetedHero;

    public bool IsTowerAttacker { get; protected set; }
    public bool IsKamikaze { get; protected set; }
    public bool IsMoving => Movement.IsMoves;

    [SerializeField] protected BaseMonsterAnimation anim;
    [SerializeField] protected BaseAttack attack;
    [SerializeField] protected BaseDetection detect;
    [SerializeField] protected BaseMovement move;
    [SerializeField] protected float startHealth;
    [SerializeField] protected int moneyGain = 50;
    [SerializeField] protected float dyingAnimationDuration;
    [SerializeField] protected float detectionRange;

    protected GameplayStates gameplayStates;
    protected Coroutine coroutine;
    protected Coroutine stunCoroutine;
    protected Coroutine periodicDamageCoroutine;
    protected BaseTower tower;
    protected Transform target;

    protected BaseHero currentTargetedHero;
    protected BaseHero currentAttackerHero;
    protected float startMoveSpeed;
    protected float attackCountdown = 0f;
    protected bool isSelected;
    protected bool canAttack;
    protected bool isPaused;
    protected bool isStunned;

    protected AttackStats currentAttackerAttackStats;
    protected float magicResistStartValue;
    protected float physicResistStartValue;

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
        healthBarInstance.gameObject.SetActive(false);
        canAttack = true;

        magicResistStartValue = AttackStats.MagicalResistance;
        physicResistStartValue = AttackStats.PhysicalResistance;
        
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
        if (isDead || isPaused || isStunned)
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
        if (currentTargetedHero && currentTargetedHero.CanAttack)
        {
            Movement.LockOnTarget(target, turnSpeed);

            if (attackCountdown <= 0)
            {
                attackCountdown =  attackRate;
        
                Attack(target); 
            }

            anim.SetAttackAnimation(true);
        }
        else 
        {
            anim.SetAttackAnimation(false);
        }

        attackCountdown -= Time.deltaTime;
    }

    protected override void Attack(Transform target)
    {
        attack.AttackTarget(AttackStats,target, AttackStats.DamageData.DamageAmount, CurrentTargetedHero);
    }

    public void ReduceResistance(DamageType damageType, float amount)
    {
        if(damageType == DamageType.Physical)
        {
            float resist = physicResistStartValue;
            float reduceValue = Mathf.Max(0f, resist - amount);
            AttackStats.PhysicalResistance = Mathf.Min(physicResistStartValue, reduceValue);
        }
        else
        {
            float resist = magicResistStartValue;
            float reduceValue = Mathf.Max(0f, resist - amount);
            AttackStats.MagicalResistance = Mathf.Min(magicResistStartValue, reduceValue);
        }
    }

    public void SetDefaultResistence()
    {
        AttackStats.PhysicalResistance = physicResistStartValue;
        AttackStats.MagicalResistance = magicResistStartValue;
    }

    public override void TakeDamage(AttackStats enemy)
    {
        currentAttackerAttackStats = enemy;
        DoDamage.ApplyDamage(AttackStats, enemy.DamageData);

        healthBarInstance.gameObject.SetActive(true);
       // armorBar.gameObject.SetActive(true);
        //health -= stats.Health;
        // Debug.Log("Damage: " + enemy.DamageData.DamageAmount);
        //healthBarInstance.SetMaterialProperty((health / startHealth));
       //armorBar.SetMaterialProperty((AttackStats.Armor / AttackStats.MaxArmor));
        healthBarInstance.SetMaterialProperty((AttackStats.Health / AttackStats.MaxHealth));
        

        if (AttackStats.Health <= 0 && !IsDead)
        {
            OnDie();
        }
    }
    public override void TakePureDamage(float amount)
    {
        //AttackStats.Health -= amount;
        AttackStats.Health = Mathf.Max(0, AttackStats.Health - amount);
        //armorBar.SetMaterialProperty((AttackStats.Armor / AttackStats.MaxArmor));
        healthBarInstance.gameObject.SetActive(true);
        healthBarInstance.SetMaterialProperty((AttackStats.Health / AttackStats.MaxHealth));

        if (AttackStats.Health <= 0 && !IsDead)
        {
            OnDie();
        }
    }

    public override void TakePeriodicDamage(AttackStats attackerStats, float periodicDamage, float periodicDamageDuration)
    {
        if(periodicDamageCoroutine != null)
        {
            StopCoroutine(periodicDamageCoroutine);
        }

        periodicDamageCoroutine = StartCoroutine(ApplyPeriodicDamage(attackerStats, periodicDamage, periodicDamageDuration));
    }

    public override void Decelerate(float passiveSkill_1, float passiveSkill_2)
    {
        
    }

    public override void GetStun(float stunDuration)
    {
        if (stunCoroutine != null)
        {
            StopCoroutine(stunCoroutine);
        }

        stunCoroutine = StartCoroutine(ApplyStun(stunDuration));
    }

    private IEnumerator ApplyStun(float stunDuration)
    {
        isStunned = true;
        anim.SetAttackAnimation(false);
        anim.SetMoveAnimation(false);

        yield return new WaitForSeconds(stunDuration);

        isStunned = false;

        if (target)
        {
            anim.SetAttackAnimation(true);
        }
        else
        {
            anim.SetMoveAnimation(true);
        }  
    }

    private IEnumerator ApplyPeriodicDamage(AttackStats attackerStats, float periodicDamage, float periodicDamageDuration)
    {
        float timer = periodicDamageDuration;
        float second = 0f;
        float finalDamage = DoDamage.CalculateDamage(AttackStats, attackerStats.DamageData, periodicDamage);
        Debug.Log("Periodic Damage: " + finalDamage);

        while (timer > 0 && !isDead)
        {
            if (second <= 0)
            {
                TakePureDamage(finalDamage);
                second = 1f;
            }
            
            timer -= Time.deltaTime;
            second -= Time.deltaTime;

            yield return null;
        }    
    }

    protected override void OnDie()
    {
        isDead = true;
        
        healthBarInstance.gameObject.SetActive(false);
        MonstersFactoriesService.DecreaseMonsterCount(this);

        RejectCurrentAttackerHero();
        CalculateMoneyForKillingEnemy();

        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        //target = null;
        //currentAttackerHero = null;
        //currentTargetedHero = null;

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

        //stunCoroutine = null;
        //coroutine = null;
        //periodicDamageCoroutine = null;
        StopAllCoroutines();

        gameObject.SetActive(false);
    }

    protected override void Move(Transform target)
    {
        Movement.Move(target, moveSpeed, turnSpeed);
    }    

    private void CalculateMoneyForKillingEnemy()
    {
        GameplayPlayerDataHandler.IncreaseMoney(moneyGain);
    }

    public override void OnRevive()
    { 
        isDead = false;
        isStunned = false;
        //health = startHealth;
        //healthBarInstance.SetMaterialProperty((health / startHealth));

        AttackStats.Health = AttackStats.MaxHealth;
        //healthBarInstance.gameObject.SetActive(true);
        healthBarInstance.SetMaterialProperty((AttackStats.Health / AttackStats.MaxHealth));
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
        //currentTargetedHero = currentAttackerHero;
        //target = this.currentAttackerHero.PartForTargeting;
    }

    public void SetCurrentTargetedHero()
    {
        currentTargetedHero = currentAttackerHero;
        target = currentTargetedHero.PartForTargeting;
    }
    public void RejectCurrentAttackerHero()
    {
        target = null;
        currentTargetedHero = null;
        currentAttackerHero = null;

        if (tower)
        {
            tower.ClearCurrentAttackerMonster(this);
            tower = null;
        }

        Movement.SetIsMove(true);
        anim.SetMoveAnimation(true);
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawWireSphere(transform.position, attackRange);
    //}
}
