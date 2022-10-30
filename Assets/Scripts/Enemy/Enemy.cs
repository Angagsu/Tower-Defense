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
    private bool isDead = false;

    [SerializeField] private Transform bulletInstPoint;
    [SerializeField] private GameObject bulletPrefab;
    
    [SerializeField] private float attackRate = 1f;
    [SerializeField] private int damage = 40;

    private EnemyMovement enemyMovement;
    private Transform targetArcher, targetKnight, targetDefender;
    private Hero targetArcherHero, targetKnightHero;
    private SworderDefender targetSworderDefender;
    private float attackCountdown = 0;
    private GameObject archerHero, knightHero;

    
    public bool IsEnemySwordAttack;
    private void Start()
    {
        archerHero = GameObject.FindGameObjectWithTag("ArcherHero");
        knightHero = GameObject.FindGameObjectWithTag("KnightHero");
        enemyMovement = GetComponent<EnemyMovement>();
        enemySpeed = startSpeed;
        health = startHealth;
        //InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    private void Update()
    {
        //UpdateTargetDefender();
        UpdateTargetHero();


    }

    private void UpdateTargetHero()
    {
        GameObject[] defenders = GameObject.FindGameObjectsWithTag("Defender");
        float shortestDistance = Mathf.Infinity;
        GameObject nearestDefender = null;

        float shortestDistanceToArcher = Mathf.Infinity;
        float shortestDistanceToKnight = Mathf.Infinity;
        GameObject nearestArcherHero = null;
        GameObject nearestKnightHero = null;
        float distanceToArcherHero = Vector3.Distance(transform.position, archerHero.transform.position);
        float distanceToKnightHero = Vector3.Distance(transform.position, knightHero.transform.position);

        foreach (GameObject defender in defenders)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, defender.transform.position);

            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestDefender = defender;
            }

        }

        /*if (nearestDefender != null && shortestDistance <= range)
        {
            targetDefender = nearestDefender.transform;
            targetSworderDefender = nearestDefender.GetComponent<SworderDefender>();
            enemyMovement.isEnemyStoppedMove = true;
            enemyMovement.LockOnTarget(targetDefender);
            if (!targetSworderDefender.isDefenderDead)
            {
                if (attackCountdown <= 0)
                {
                    attackCountdown = 1 / attackRate;
                    if (IsEnemySwordAttack)
                    {
                        EnemySwordAttackToDefenders(targetDefender);
                        Debug.Log("Sword Attack");
                    }
                    else
                    {
                        EnemyArcherAttack(targetDefender);
                    }
                }
            }
            else
            {
                enemyMovement.isEnemyStoppedMove = false;
            }
        }
        else
        {
            targetDefender = null;
            targetSworderDefender = null;
            enemyMovement.isEnemyStoppedMove = false;
        }*/

        if (distanceToArcherHero < shortestDistanceToArcher )
        {
            shortestDistanceToArcher = distanceToArcherHero;
            nearestArcherHero = archerHero;
        }
        if (nearestArcherHero != null && shortestDistanceToArcher <= range && nearestKnightHero == null)
        {
            targetArcher = nearestArcherHero.transform;
            targetArcherHero = nearestArcherHero.GetComponent<Hero>();
            enemyMovement.isEnemyStoppedMove = true;
            enemyMovement.LockOnTarget(targetArcher);
            if (!targetArcherHero.isHeroDead)
            {
                if (attackCountdown <= 0)
                {
                    attackCountdown = 1 / attackRate;
                    if (IsEnemySwordAttack)
                    {
                        EnemySwordAttackToHeroes(targetArcher);
                    }
                    else
                    {
                        EnemyArcherAttack(targetArcher);
                    }
                }
            }
            else
            {
                enemyMovement.isEnemyStoppedMove = false;
            }
        }
        else
        {
            targetArcher = null;
            nearestArcherHero = null;
            enemyMovement.isEnemyStoppedMove = false;
        }


        if (distanceToKnightHero < shortestDistanceToKnight)
        {
            shortestDistanceToKnight = distanceToKnightHero;
            nearestKnightHero = knightHero;
        }
        else
        {
            targetKnight = null;
            nearestKnightHero = null;
            enemyMovement.isEnemyStoppedMove = false;
        }
        if (nearestKnightHero != null && shortestDistanceToKnight <= range && nearestArcherHero == null)
        {
            targetKnight = nearestKnightHero.transform;
            targetKnightHero = nearestKnightHero.GetComponent<Hero>();
            enemyMovement.isEnemyStoppedMove = true;
            enemyMovement.LockOnTarget(targetKnight);
            if (!targetKnightHero.isHeroDead)
            {
                if (attackCountdown <= 0)
                {
                    attackCountdown = 1 / attackRate;
                    if (IsEnemySwordAttack)
                    {
                        EnemySwordAttackToHeroes(targetKnight);
                    }
                    else
                    {
                        EnemyArcherAttack(targetKnight);
                    }
                }
                
            }
            else
            {
                enemyMovement.isEnemyStoppedMove = false;
            }  
        }
        attackCountdown -= Time.deltaTime;
    }


    private void EnemyArcherAttack(Transform target)
    {
        GameObject bulletObj = Instantiate(bulletPrefab, bulletInstPoint.position, bulletInstPoint.rotation);
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        if (bullet != null)
        {
            bullet.BulletSeek(target);
        }
    }

    private void EnemySwordAttackToHeroes(Transform hero)
    {
        Hero h = hero.GetComponent<Hero>();
        h.AmountOfDamagetoHero(damage);
    }

    public void EnemySwordAttackToDefenders(Transform defender)
    {
        SworderDefender def = defender.GetComponent<SworderDefender>();
        def.AmountOfDamagetoDefender(damage);
        
    }

    public void AmountOfDamagetoEnemy(float amount)
    {
        health -= amount;
        healthBar.fillAmount = health / startHealth;
        
        if (health <= 0 && !isDead)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        CalculateMoneyForKillingEnemy();
        Destroy(gameObject);
        WaveSpawner.EnemiesAlive--;
    }

    private void CalculateMoneyForKillingEnemy()
    {
        PlayerStats.Money += moneyGain;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    public void Slow(float amount)
    {
        enemySpeed = startSpeed * (1 - amount);
    }
}
