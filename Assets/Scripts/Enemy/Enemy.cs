using UnityEngine.UI;
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
    private EnemyMovement enemyMovement;
    private Transform targetArcher, targetKnight;
    private Hero targetArcherHero, targetKnightHero;
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
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    
    private void UpdateTarget()
    {
        
        float shortestDistanceToArcher = Mathf.Infinity;
        float shortestDistanceToKnight = Mathf.Infinity;
        GameObject nearestArcherHero = null;
        GameObject nearestKnightHero = null;
        float distanceToArcherHero = Vector3.Distance(transform.position, archerHero.transform.position);
        float distanceToKnightHero = Vector3.Distance(transform.position, knightHero.transform.position);


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
            if (IsEnemySwordAttack)
            {
                EnemySwordAttack();
            }
            else
            {
                EnemyArcherAttack(targetArcher);
            }
        }
        else
        {
            targetArcher = null;
            nearestArcherHero = null;
            enemyMovement.isEnemyStoppedMove = false;
        }


        if (distanceToKnightHero < shortestDistanceToKnight )
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
            if (IsEnemySwordAttack)
            {
                EnemySwordAttack();
            }
            else
            {
                EnemyArcherAttack(targetKnight);
            }   
        }
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

    private void EnemySwordAttack()
    {
        Debug.Log("Sword Enemy Attacked !!!");
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

    public void Slow(float amount)
    {
        enemySpeed = startSpeed * (1 - amount);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
