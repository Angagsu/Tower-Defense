
using UnityEngine;

public class Enemy : MonoBehaviour
{
    
    [SerializeField] private float enemySpeed = 10f;
    [SerializeField] private int enemyHealth = 100;
    [SerializeField] private int moneyGain = 50;
    
    private Transform target;
    private int wayCountIndex = 0;
    public bool IsEnemyDead = false;
    
    private void Start()
    {
        target = WayPoints.wayPoints[0];
        
    }

    private void Update()
    {
        Vector3 direction = target.position - transform.position;
        transform.Translate(direction.normalized * enemySpeed * Time.deltaTime , Space.World);

        if (Vector3.Distance(target.transform.position, transform.position) <= 0.5f)
        {
            GetNextWayPoint();
        }
    }

    public void AmountOfDamagetoEnemy(int amount)
    {
        enemyHealth -= amount;
        if (enemyHealth <= 0)
        {
            
            Die();
        }
    }

    private void Die()
    {
        CalculateMoneyForKillingEnemy();
        Destroy(gameObject);
    }

    private void GetNextWayPoint()
    {
        if (WayPoints.wayPoints.Length - 1 <= wayCountIndex)
        {
            PathEnd();
            return;
        }

        wayCountIndex++;
        target = WayPoints.wayPoints[wayCountIndex];
    }

    private void PathEnd()
    {
        PlayerStats.Lives--;
        Destroy(gameObject);
    }

    private void CalculateMoneyForKillingEnemy()
    {
        PlayerStats.Money += moneyGain;
    }

  
}
