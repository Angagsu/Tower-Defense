using UnityEngine;
using Assets.Scripts;
using Assets.Scripts.Tower;

public class BaseMonster : Character, IAttackableMonster
{
    [SerializeField] protected BaseMonsterAnimation anim;
    [SerializeField] protected BaseAttack attack;
    [SerializeField] protected BaseDetect detect;
    [SerializeField] protected BaseMovement move;
    [SerializeField] protected float startHealth;
    [SerializeField] protected int moneyGain = 50;

    protected BaseTower tower;
    protected Transform target;

    protected float startMoveSpeed;
    protected float attackCountdown = 0f;
    protected bool isSelected;
    protected bool canAttack;

    private MonsterMovement movement;

    [field: SerializeField] protected int monsterID;
    public int MonsterID => monsterID;


    private Pool<BaseMonster> pool;

    
    protected virtual void Awake()
    {
        movement = move as MonsterMovement; 
        health = startHealth;
        healthBar.enabled = false;
        canAttack = true;
        SetStartMoveSpeed();
    }

    protected void OnEnable()
    {
        isDead = false;
        health = startHealth;
        healthBar.enabled = false;
        canAttack = true;
        SetDefaultMoveSpeed();
    }

    protected virtual void Update()
    {
        if (GameController.IsGameOver || isDead)
        {
            enabled = false;
            return;
        }

        DetectTarget();

        if (!move.IsMoves)
        {
            return;
        }

        Move(target);
    }

    protected override void Attack(Transform target)
    {
        attack.AttackTarget(target, damage);
    }

    protected override void DetectTarget()
    {
        if ((target = detect.DetectTarget(attackRange, IsDead)) != null)
        {
            movement.LockOnTarget(target, turnSpeed);

            if (attackCountdown <= 0)
            {
                attackCountdown = 1 / attackRate;

                Attack(target);

                anim.SetMoveAnimation(false);

                Debug.Log("Monster Sword Attack"); 
            }
        }
        else
        {
            movement.SetIsMove(true);
            anim.SetMoveAnimation(true);
        }

        attackCountdown -= Time.deltaTime;
    }

    protected override void Die()
    {
        isDead = true;
        CalculateMoneyForKillingEnemy();
        //move.enabled = false;
        anim.SetDeadAnimation(isDead);
        gameObject.SetActive(false);
        //Destroy(gameObject, 3f);
        FactoriesService.EnemiesAlive--;
    }

    protected override void Move(Transform target)
    {
        movement.Move(target, moveSpeed, turnSpeed);
    }

    public override void TakeDamage(float amount)
    {
        healthBar.enabled = true;
        health -= amount;
        healthBar.fillAmount = health / startHealth;

        if (health <= 0 && !IsDead)
        {
            Die();
        }
    }

    private void CalculateMoneyForKillingEnemy()
    {
        PlayerStats.Money += moneyGain;
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
        movement.SetIsMove(tof);
    }

    public void Slow(float amount)
    {
        moveSpeed = startMoveSpeed * (1 - amount);
    }

    public void SetPool(Pool<BaseMonster> pool)
    {
        this.pool = pool;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
