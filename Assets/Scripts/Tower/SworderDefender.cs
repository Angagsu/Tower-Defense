using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SworderDefender : MonoBehaviour
{
    private string enemyTag = "Enemy";
    private Transform target;
    private EnemyMovement targetEnemy;
    private Enemy enemy;
    private float attackCountdown = 0f;
    private float health;

    public bool isDefenderDead;

    [SerializeField] private float range = 5f;
    [SerializeField] private float attackRate = 1f;
    [SerializeField] private float damage = 100f;
    [SerializeField] public float turnSpeed = 10f;
    [SerializeField] public Transform defenderRotatPart;
    [SerializeField] private Image healthBar;
    [SerializeField] private float startHealth = 1000f;
    
    
    void Start()
    {
        health = startHealth;
        isDefenderDead = false;
    }

    
    void Update()
    {
        UpdateTarget();

        if (target != null)
        {
            LockOnTarget();
        }
    }

    private void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range && !isDefenderDead)
        {
            target = nearestEnemy.transform;
            targetEnemy = nearestEnemy.GetComponent<EnemyMovement>();
            enemy = nearestEnemy.GetComponent<Enemy>();
            targetEnemy.isEnemyStoppedMove = true;
            if (attackCountdown <= 0)
            {
                attackCountdown = 1 / attackRate;
                if (enemy.IsEnemySwordAttack)
                {
                    
                    enemy.EnemySwordAttackToDefenders(this.transform);
                    DefenderSwordAttack(target);
                    Debug.Log("Defender Attacked");
                }
                
            }

            attackCountdown -= Time.deltaTime;
        }
        else
        {
            target = null;
        }
    }

    private void DefenderSwordAttack(Transform target)
    {
        Enemy enemy = target.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.AmountOfDamagetoEnemy(damage);
        }
    }

    public void AmountOfDamagetoDefender(float amount)
    {
        health -= amount;
        healthBar.fillAmount = health / startHealth;

        if (health <= 0 )
        {
            DefenderDie();
        }
    }

    private void DefenderDie()
    {
        isDefenderDead = true;
    }

    public void ReviveDefender()
    {
        health = startHealth;
        healthBar.fillAmount = 1f;
        isDefenderDead = false;
        
    }
    
    private void LockOnTarget()
    {
        Vector3 direction = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Vector3 rotation = Quaternion.Lerp(defenderRotatPart.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        defenderRotatPart.rotation = Quaternion.Euler(0, rotation.y, 0);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, range);
    }

}
