using UnityEngine.UI;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    
    [HideInInspector]
    public float enemySpeed = 10f;
    public float startSpeed = 10f;

    
    [SerializeField] private Image healthBar;
    [SerializeField] private int moneyGain = 50;
    [SerializeField] private float startHealth = 200f;
    private float health;
    private bool isDead = false;

    private void Start()
    {
        enemySpeed = startSpeed;
        health = startHealth;
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
}
