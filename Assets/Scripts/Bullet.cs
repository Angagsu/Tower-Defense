
using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    private Transform target;

    [SerializeField] private float speed = 50f;
    [SerializeField] private GameObject impactEffect;

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direction = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (direction.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }
        transform.Translate(direction.normalized * distanceThisFrame, Space.World);
    }

    private void HitTarget()
    {
        GameObject impactObj = Instantiate(impactEffect, transform.position, transform.rotation);
        
        Destroy(impactObj, 2f);
        Destroy(target.gameObject);
        Destroy(gameObject);
        
    }

    public void BulletSeek(Transform target)
    {
        this.target = target;
    }
}
