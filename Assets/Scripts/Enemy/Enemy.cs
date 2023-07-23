using UnityEngine.UI;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
public class Enemy : MonoBehaviour
{
    [HideInInspector]
    public float enemySpeed = 10f;
    public float startSpeed = 10f;
    
    [SerializeField] private Image healthBar;
    [SerializeField] private int moneyGain = 50;
    [SerializeField] private float startHealth = 200f;
    [SerializeField] private float range = 10;
    private float health;
    public bool IsDead = false;

    [SerializeField] private Transform bulletInstPoint;
    [SerializeField] private GameObject bulletPrefab;
    
    [SerializeField] private float attackRate = 1f;
    [SerializeField] private int damage = 40;

    private EnemyMovement enemyMovement;
    private Transform targetPos;
    private Hero targetHero;

    private SworderDefender targetSworderDefender;
    private DefendersMovement targetDefenderMovement;

    
    private GameObject archerHero, knightHero;
    private GameObject nearestArcherHero, nearestKnightHero;
    private GameObject nearestDefender;
    private TowerDetection tower;

    private float attackCountdown = 0;
    private float shortestDistanceToArcher, shortestDistanceToKnight, shortestDistanceToDefender;


    private Animator animator;
    private int isEnemyStopedMoveHash;
    private int isDeadHash;

    public bool IsEnemySwordAttack;

    public bool CanAttack = true;

    private void Start()
    {
        archerHero = GameObject.FindGameObjectWithTag("ArcherHero");
        knightHero = GameObject.FindGameObjectWithTag("KnightHero");
        enemyMovement = GetComponent<EnemyMovement>();
        SetEnemyStartSpeed();
        health = startHealth;
        healthBar.enabled = false;
        animator = GetComponentInChildren<Animator>();
        isEnemyStopedMoveHash = Animator.StringToHash("isEnemyStopedMove");
        isDeadHash = Animator.StringToHash("isDead");
        CanAttack = true;
    }

    private void Update()
    {
        if (tower != null)
        {
            if (tower.slowedEnemy == null || tower.slowedEnemy != this)
            {
                SetEnemyStartSpeed();
            }
        }
        
        UpdateTarget();
    }
    
    private void UpdateTarget()
    {
        GameObject[] defenders = GameObject.FindGameObjectsWithTag("Defender");

        shortestDistanceToArcher = Mathf.Infinity;
        shortestDistanceToKnight = Mathf.Infinity;
        shortestDistanceToDefender = Mathf.Infinity;

        nearestArcherHero = null;
        nearestKnightHero = null;
        nearestDefender = null;

        float distanceToArcherHero = Vector3.Distance(transform.position, archerHero.transform.position);
        float distanceToKnightHero = Vector3.Distance(transform.position, knightHero.transform.position);

        if (distanceToArcherHero < shortestDistanceToArcher )
        {
            shortestDistanceToArcher = distanceToArcherHero;
            nearestArcherHero = archerHero;
        }
        

        if (distanceToKnightHero < shortestDistanceToKnight)
        {
            shortestDistanceToKnight = distanceToKnightHero;
            nearestKnightHero = knightHero;
        }

        foreach (var defender in defenders)
        {
            float distanceToDefender = Vector3.Distance(transform.position, defender.transform.position);


            if (distanceToDefender < shortestDistanceToDefender)
            {
                shortestDistanceToDefender = distanceToDefender;
                nearestDefender = defender;
            }
        }

        AttackTarget();
    }
    private void AttackTarget()
    {
        float currentShortestDistance = 0f;

        if (shortestDistanceToArcher < shortestDistanceToKnight && shortestDistanceToArcher < shortestDistanceToDefender)
        {
            currentShortestDistance = shortestDistanceToArcher;
            targetPos = nearestArcherHero.transform;
            targetHero = nearestArcherHero.GetComponent<Hero>();
        }
        
        if (shortestDistanceToArcher > shortestDistanceToKnight && shortestDistanceToKnight < shortestDistanceToDefender)
        {
            currentShortestDistance = shortestDistanceToKnight;
            targetPos = nearestKnightHero.transform;
            targetHero = nearestKnightHero.GetComponent<Hero>();
        }

        if (shortestDistanceToDefender < shortestDistanceToArcher && shortestDistanceToDefender < shortestDistanceToKnight )
        {
            currentShortestDistance = shortestDistanceToDefender;
            targetPos = nearestDefender.transform;
            targetSworderDefender = nearestDefender.GetComponent<SworderDefender>();
            targetDefenderMovement = nearestDefender.GetComponentInParent<DefendersMovement>();
            
        }

        if (currentShortestDistance <= range && CanAttack)
        {
            CheckCurrentAttackerOnDefender();

            if (targetHero != null && !targetHero.isHeroDead && targetHero.gameObject.activeSelf ||
                targetDefenderMovement != null && targetDefenderMovement.isDefendersStoppedMove)
            {
                enemyMovement.isEnemyStoppedMove = true;
                enemyMovement.LockOnTarget(targetPos);

                animator.SetBool(isEnemyStopedMoveHash, true);

                

                if (attackCountdown <= 0)
                {
                    attackCountdown = attackRate;
                    if (IsEnemySwordAttack && targetHero != null)
                    {
                        EnemySwordAttackToHeroes(targetPos);
                    }
                    else if (IsEnemySwordAttack && targetSworderDefender != null)
                    {
                        
                        EnemySwordAttackToDefenders(targetPos);
                    }
                    else
                    {
                        EnemyArcherAttack(targetPos);
                    }
                }
            }
            else
            {
                targetPos = null;
                targetHero = null;
                targetSworderDefender = null;
                targetDefenderMovement = null;
                enemyMovement.isEnemyStoppedMove = false;
                animator.SetBool(isEnemyStopedMoveHash, false);
            }

        }
        else
        {
            targetPos = null;
            targetHero = null;
            targetSworderDefender = null;
            targetDefenderMovement = null;
            enemyMovement.isEnemyStoppedMove = false;
            animator.SetBool(isEnemyStopedMoveHash, false);
        }

        attackCountdown -= Time.deltaTime;
    }
    private void CheckCurrentAttackerOnDefender()
    {
        if (targetSworderDefender != null)
        {
            if (targetSworderDefender.currentAttackerEnemy == null || targetSworderDefender.currentAttackerEnemy == this)
            {
                targetSworderDefender.SetCurrentAttackerEnemy(this);
                CanAttack = true;
            }
            else
            {
                CanAttack = false;
            }
        }
        else
        {
            CanAttack = true;
        }
    }
    public void EnemyArcherAttack(Transform target)
    {
        GameObject bulletObj = Instantiate(bulletPrefab, bulletInstPoint.position, bulletInstPoint.rotation);
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        if (bullet != null)
        {
            bullet.BulletSeek(target);
        }
        Debug.Log("Enemy Archer Attack ");
    }

    public void EnemySwordAttackToHeroes(Transform hero)
    {
        Hero h = hero.GetComponent<Hero>();
        if (h != null)
        {
            h.AmountOfDamagetoHero(damage);
        }

        Debug.Log("Enemy Attacked To Hero !!! ");
    }

    public void EnemySwordAttackToDefenders(Transform defender)
    {
        SworderDefender def = defender.GetComponent<SworderDefender>();
        if (def != null)
        {
            def.AmountOfDamageToDefender(damage);
        }
        
        Debug.Log("Enemy Attacked To Deffender !!! ");
    }

    public void AmountOfDamagetoEnemy(float amount)
    {
        healthBar.enabled = true;
        health -= amount;
        healthBar.fillAmount = health / startHealth;
        
        if (health <= 0 && !IsDead)
        {
            Die();
        }
    }

    private void Die()
    {
        IsDead = true;
        CalculateMoneyForKillingEnemy();
        enemyMovement.enabled = false;
        animator.SetBool(isDeadHash, true);
        Destroy(gameObject, 3f);
        WaveSpawner.EnemiesAlive--;
    }

    private void CalculateMoneyForKillingEnemy()
    {
        PlayerStats.Money += moneyGain;
    }
    public void Slow(float amount)
    {
        enemySpeed = startSpeed * (1 - amount);
    }
    private void SetEnemyStartSpeed()
    {
        enemySpeed = startSpeed;
    }

    public void SetAttackedTower(TowerDetection tower)
    {
        this.tower = tower;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, range);
    }

}
