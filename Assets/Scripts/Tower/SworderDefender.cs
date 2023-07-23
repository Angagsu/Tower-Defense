using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SworderDefender : MonoBehaviour
{
    private string enemyTag = "Enemy";
    public Transform target;
    private float attackCountdown = 0f;
    private float health;

    public bool IsDefenderDead;
    public bool HasTarget;

    [SerializeField] private float range = 5f;
    [SerializeField] private float attackRate = 1f;
    [SerializeField] private float damage = 100f;
    [SerializeField] public float turnSpeed = 10f;
    [SerializeField] public Transform defenderRotatPart;
    [SerializeField] private Image healthBar;
    [SerializeField] private float startHealth = 1000f;

    public Enemy targetEnemy = null;
    public Enemy currentAttackerEnemy = null;
    float distanceToAttackerEnemy;

    #region Animation
    
    private Animator animator;
    private int isDeadHash;
    private int isStopedMoveHash;
    private int isAttackHash;

    #endregion
    void Start()
    {
        health = startHealth;
        IsDefenderDead = false;
        HasTarget = false;

        animator = GetComponentInChildren<Animator>();
        isDeadHash = Animator.StringToHash("isDead");
        isStopedMoveHash = Animator.StringToHash("isStopedMove");
        isAttackHash = Animator.StringToHash("isAttack");
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
        Enemy enemyTarget = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            enemyTarget = enemy.GetComponent<Enemy>();

            if (distanceToEnemy < shortestDistance && enemyTarget != null && !enemyTarget.IsDead)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }
        
        if (nearestEnemy != null && shortestDistance <= range && !IsDefenderDead)
        {
            HasTarget = true;
            targetEnemy = nearestEnemy.GetComponent<Enemy>();
            target = nearestEnemy.transform;

            animator.SetBool(isAttackHash, true);
            
            if (attackCountdown <= 0)
            {
                attackCountdown = attackRate;
                DefenderSwordAttack(target);
                Debug.Log("Defender Attacked");
            }

            attackCountdown -= Time.deltaTime;
        }
        else
        {
            HasTarget = false;
            nearestEnemy = null;
            targetEnemy = null;
            animator.SetBool(isAttackHash, false);
        }

        if (currentAttackerEnemy != null)
        {
            Transform attackerEnemyPos = currentAttackerEnemy.GetComponent<Transform>();
            distanceToAttackerEnemy = Vector3.Distance(transform.position, attackerEnemyPos.position);
            Debug.Log("Current Attacker Enemy Is Not Null !!!");
            if (distanceToAttackerEnemy > range)
            {
                currentAttackerEnemy = null;
            }
        }

    }
    
    public void SetCurrentAttackerEnemy(Enemy enemy)
    {
        currentAttackerEnemy = enemy;
    }

    private void DefenderSwordAttack(Transform target)
    {
        Enemy enemy = target.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.AmountOfDamagetoEnemy(damage);
        }
    }

    public void AmountOfDamageToDefender(float amount)
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
        IsDefenderDead = true;
        animator.SetBool(isDeadHash, true);
        
    }

    public void ReviveDefender()
    {
        health = startHealth;
        healthBar.fillAmount = 1f;
        IsDefenderDead = false;
        animator.SetBool(isDeadHash, false);
        animator.SetBool(isStopedMoveHash, true);
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
