
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
    [SerializeField] bool UseLaser = false;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] ParticleSystem laserImpactEffect;
    [SerializeField] Light impactLight;

    private Transform target;
    private Enemy targetEnemy;
    private string enemyTag = "Enemy";
    private float fireCountdown = 0f;

    private void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    private void Update()
    {
        if (target == null)
        {
            if (UseLaser)
            {
                if (lineRenderer.enabled)
                {
                    lineRenderer.enabled = false;
                    impactLight.enabled = false;
                    laserImpactEffect.Stop();  
                }
            }
            return;
        }

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

    private void Shoot()
    {
        GameObject bulletObj = Instantiate(bulletPrefab, bulletInstPoint.position, bulletInstPoint.rotation);
        Bullet bullet = bulletObj.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.BulletSeek(target);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, range);
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

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
            targetEnemy = nearestEnemy.GetComponent<Enemy>();
        }
        else
        {
            target = null;
        }
    }

    private void LaserAttack()
    {
        targetEnemy.AmountOfDamagetoEnemy(damageOverTime * Time.deltaTime);
        targetEnemy.Slow(slowAmount);

        if (!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
            impactLight.enabled = true;
            laserImpactEffect.Play();
            
        }
        lineRenderer.SetPosition(0, bulletInstPoint.position);
        lineRenderer.SetPosition(1, target.position);

        Vector3 direction = bulletInstPoint.position - target.position;
        laserImpactEffect.transform.position = target.position + direction.normalized;
        laserImpactEffect.transform.rotation = Quaternion.LookRotation(direction);
        
    }

    private void LockOnTarget()
    {
        Vector3 direction = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Vector3 rotation = Quaternion.Lerp(towerRotatPart.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        towerRotatPart.rotation = Quaternion.Euler(0, rotation.y, 0);
    }
}
