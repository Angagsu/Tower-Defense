using UnityEngine;
using Assets.Scripts;
using Assets.Scripts.Tower;
using System.Collections;

public class BaseMonster : Character, IAttackableMonster
{
    [SerializeField] protected BaseMonsterAnimation anim;
    [SerializeField] protected BaseAttack attack;
    [SerializeField] protected BaseDetection detect;
    [SerializeField] protected BaseMovement move;
    [SerializeField] protected float startHealth;
    [SerializeField] protected int moneyGain = 50;
    [SerializeField] protected float dyingAnimationDuration;

    protected Coroutine coroutine;
    protected BaseTower tower;
    protected Transform target;

    protected float startMoveSpeed;
    protected float attackCountdown = 0f;
    protected bool isSelected;
    protected bool canAttack;

    private MonsterMovement movement;



    protected virtual void Awake()
    {
        movement = move as MonsterMovement; 
        health = startHealth;
        healthBar.enabled = false;
        canAttack = true;
        SetStartMoveSpeed();
    }

    protected virtual void OnEnable()
    {
        OnRevive();
    }

    protected virtual void Update()
    {
        if (GameController.IsGameOver || isDead)
        {
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
            }
        }
        else
        {
            movement.SetIsMove(true);
            anim.SetMoveAnimation(true);
        }

        attackCountdown -= Time.deltaTime;
    }

    protected override void OnDie()
    {
        isDead = true;
        FactoriesService.EnemiesAlive--;
        CalculateMoneyForKillingEnemy();

        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        
        coroutine = StartCoroutine(DyingAnimationDuration());
    }

    protected IEnumerator DyingAnimationDuration()
    {
        anim.SetDeadAnimation(isDead);

        yield return new WaitForSeconds(dyingAnimationDuration);

        gameObject.SetActive(false);
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
            OnDie();
        }
    }

    private void CalculateMoneyForKillingEnemy()
    {
        PlayerStats.Money += moneyGain;
    }

    public override void OnRevive()
    { 
        isDead = false;
        health = startHealth;
        healthBar.enabled = false;
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
        movement.SetIsMove(tof);
    }

    public void SetIsDead(bool tof)
    {
        isDead = tof;
    }

    public void Slow(float amount)
    {
        moveSpeed = startMoveSpeed * (1 - amount);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
