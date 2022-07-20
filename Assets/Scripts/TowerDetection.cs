using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerDetection : MonoBehaviour
{
    [SerializeField] private Transform towerRotatPart;
    [SerializeField] private Transform bulletInstPoint;
    [SerializeField] private GameObject bulletPrefab;
    
    [Header("settings")] 
    [SerializeField] private float turnSpeed = 10f;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float range = 15f;

    private Transform target;
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
            return;
        }

        Vector3 direction = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Vector3 rotation = Quaternion.Lerp(towerRotatPart.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        towerRotatPart.rotation = Quaternion.Euler(0, rotation.y, 0);

        if (fireCountdown <= 0)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime; 
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
        }
        else
        {
            target = null;
        }
    }
}
