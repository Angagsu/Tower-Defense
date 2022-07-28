
using UnityEngine;

public class Enemy : MonoBehaviour
{
    
    [HideInInspector]
    public float enemySpeed = 10f;
    public float startSpeed = 10f;
    [SerializeField] private float enemyHealth = 100;
    [SerializeField] private int moneyGain = 50;

    private void Start()
    {
        enemySpeed = startSpeed;
    }

    public void AmountOfDamagetoEnemy(float amount)
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

    private void CalculateMoneyForKillingEnemy()
    {
        PlayerStats.Money += moneyGain;
    }

    public void Slow(float amount)
    {
        enemySpeed = startSpeed * (1 - amount);
    }
}
