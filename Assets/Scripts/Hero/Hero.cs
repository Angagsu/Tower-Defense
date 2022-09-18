using UnityEngine.UI;
using UnityEngine;
using System;

public class Hero : MonoBehaviour
{
    
    private HeroesMovement heroesMovement;
    private HeroesMovement archerHero, knightHero;
    private Camera mainCamera;
    private string enemyTag = "Enemy";
    private Transform target;
    private Enemy targetEnemy;
    private float attackCountdown = 0f;
    private float health;
    public bool isHeroDead;

    [SerializeField] private TowerBuildManager towerBuildManager;
    [SerializeField] private Image healthBar;
    [SerializeField] private float startHealth = 1000f;
    [SerializeField] private float attackRate = 1f;
    [SerializeField] private float range = 15f;
    [SerializeField] private float turnSpeed = 10f;
    [SerializeField] private float damage = 100f;
    [SerializeField] private Transform bulletInstPoint;
    [SerializeField] private GameObject bulletPrefab;
    

    public bool IsSwordAttack;

    private void Awake()
    {
        
    }

    private void Start()
    {
        archerHero = GameObject.FindGameObjectWithTag("ArcherHero").GetComponent<HeroesMovement>();
        knightHero = GameObject.FindGameObjectWithTag("KnightHero").GetComponent<HeroesMovement>();
        heroesMovement = GetComponent<HeroesMovement>();
        mainCamera = Camera.main;
        isHeroDead = false;
        health = startHealth;
        //InvokeRepeating("UpdateTarget", 0f, 0.5f); 
    }

    private void Update()
    {
        UpdateTarget();
        //SelectHeroOnClick();

        if (target == null)
        {
            if (heroesMovement.isHeroStoppedMove && IsSwordAttack)
            {
                if (true)
                {
                    
                }
            }
            return;
        }
        
        if (heroesMovement.isHeroStoppedMove && !isHeroDead)
        {
            LockOnTarget();
        }

        

    }

    private void OnMouseDown()
    {
        if (gameObject.CompareTag("ArcherHero"))
        {
            archerHero.isHeroSelected = true;
            knightHero.isHeroSelected = false;
            towerBuildManager.DeselectGround();
        }
        else if (gameObject.CompareTag("KnightHero"))
        {
            knightHero.isHeroSelected = true;
            archerHero.isHeroSelected = false;
            towerBuildManager.DeselectGround();
        }   
    }

    private void SelectHeroOnClick()
    {
       
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit) && raycastHit.collider && raycastHit.collider.gameObject.CompareTag("ArcherHero"))
            {
                archerHero.isHeroSelected = true;
                knightHero.isHeroSelected = false;
                towerBuildManager.DeselectGround();
            }
            if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider && hit.collider.gameObject.CompareTag("KnightHero"))
            {
                knightHero.isHeroSelected = true;
                archerHero.isHeroSelected = false;
                towerBuildManager.DeselectGround();
            }
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

        if (nearestEnemy != null && shortestDistance <= range && !isHeroDead)
        {
            target = nearestEnemy.transform;
            targetEnemy = nearestEnemy.GetComponent<Enemy>();

            if (attackCountdown <= 0)
            {
                attackCountdown = 1 / attackRate;
                if (IsSwordAttack)
                {
                    SwordAttack(target);
                    Debug.Log("Archer Attack");
                }
                else
                {
                    ArcherAttack();
                }
            }

            attackCountdown -= Time.deltaTime;
        }
        else
        {
            target = null;
        }
    }

    private void ArcherAttack()
    {
        GameObject bulletObj = Instantiate(bulletPrefab, bulletInstPoint.position, bulletInstPoint.rotation);
        Bullet bullet = bulletObj.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.BulletSeek(target);
        }
    }

    private void SwordAttack(Transform enemy)
    {
        Enemy e = enemy.GetComponent<Enemy>();
        if (e != null)
        {
            e.AmountOfDamagetoEnemy(damage);
        }
    }
    private void LockOnTarget()
    {
        Vector3 direction = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Vector3 rotation = Quaternion.Lerp(heroesMovement.heroRotatPart.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        heroesMovement.heroRotatPart.rotation = Quaternion.Euler(0, rotation.y, 0);
    }

    public void AmountOfDamagetoHero(float amount)
    {
        health -= amount;
        healthBar.fillAmount = health / startHealth;

        if (health <= 0 && !isHeroDead)
        {
            HeroDie();
        }
    }

    private void HeroDie()
    {
        isHeroDead = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    public void SelectArcherHeroOnUI()
    {
        archerHero.isHeroSelected = true;
        knightHero.isHeroSelected = false;
        towerBuildManager.DeselectGround();
    }
    public void SelectKnightHeroOnUI()
    {
        knightHero.isHeroSelected = true;
        archerHero.isHeroSelected = false;
        towerBuildManager.DeselectGround();
    }
    
    public void DeselectHeroes()
    {
        archerHero.isHeroSelected = false;
        knightHero.isHeroSelected = false;
    }
}
