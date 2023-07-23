using System.Collections;
using UnityEngine;

public class TowerDetection : MonoBehaviour
{
    [SerializeField] private Transform towerRotatPart;
    [SerializeField] private Transform bulletInstPoint;
    [SerializeField] private GameObject bulletPrefab;
    
    [Header("Settings")] 
    [SerializeField] private float turnSpeed = 10f;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float range = 15f;

    [Header("Use Laser")]
    [SerializeField] private int damageOverTime = 30;
    [SerializeField] private float slowAmount = 0.5f;
    [SerializeField] private bool UseLaser = false;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private ParticleSystem laserImpactEffect;
    
    [HideInInspector] public Enemy slowedEnemy;

    private Transform target;
    private Enemy targetEnemy;
    private readonly string enemyTag = "Enemy";
    private float fireCountdown = 0f;

    private Material thanderMaterial;
    private readonly float randomWithOffsetMax = 4f;
    private readonly float randomWithOffsetMin = 3f;

    private void Awake()
    {
        if (lineRenderer != null && laserImpactEffect != null)
        {
            thanderMaterial = GetComponent<LineRenderer>().material;
        }
        UpdateTarget();
    }
    

    private void Update()
    {
        if (GameController.IsGameOver)
        {
            enabled = false;
            if (UseLaser && lineRenderer.enabled)
            {
                lineRenderer.enabled = false;
                laserImpactEffect.Stop();
            }
            return;
        }

        UpdateTarget();
    }

    private void Shoot()
    {
        GameObject bulletObj = Instantiate(bulletPrefab, bulletInstPoint.position, bulletInstPoint.rotation);
        Bullet bullet = bulletObj.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.BulletSeek(target);
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

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
            targetEnemy = nearestEnemy.GetComponent<Enemy>();

            LockOnTarget();

            if (UseLaser)
            {
                LaserAttack();
            }
            else
            {
                if (fireCountdown <= 0)
                {
                    Shoot();
                    fireCountdown = 1f / fireRate;
                }

                fireCountdown -= Time.deltaTime;
            }
            
        }
        else
        {
            target = null;
            if (UseLaser && lineRenderer.enabled)
            {
                lineRenderer.enabled = false;
                laserImpactEffect.Stop();
            }
        }
        slowedEnemy = targetEnemy;
    }

    private void LaserAttack()
    {
        if (!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
            laserImpactEffect.Play();
        }

        targetEnemy.SetAttackedTower(this);
        targetEnemy.AmountOfDamagetoEnemy(damageOverTime * Time.deltaTime);
        targetEnemy.Slow(slowAmount);

        lineRenderer.SetPosition(0, bulletInstPoint.position);
        lineRenderer.SetPosition(1, target.localPosition);

        thanderMaterial.mainTextureOffset = new Vector2(Random.Range(0f, 1f), 0);
        lineRenderer.startWidth = RandomWidthOffset();
        lineRenderer.endWidth = RandomWidthOffset();

        Vector3 direction = bulletInstPoint.position - target.position;
        laserImpactEffect.transform.position = target.position + direction.normalized;
        laserImpactEffect.transform.rotation = Quaternion.LookRotation(direction);

    }

    private float RandomWidthOffset()
    {
        return Random.Range(randomWithOffsetMin, randomWithOffsetMax);
    }

    private void LockOnTarget()
    {
        Vector3 direction = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Vector3 rotation = Quaternion.Lerp(towerRotatPart.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        towerRotatPart.rotation = Quaternion.Euler(0, rotation.y, 0);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
