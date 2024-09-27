using UnityEngine;

public class RangeAttack : BaseAttack
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletInstPoint;

    public override void AttackTarget(Transform target, float damage)
    {
        GameObject bulletObj = Instantiate(bulletPrefab, bulletInstPoint.position, bulletInstPoint.rotation);

        if (bulletObj.TryGetComponent<Bullet>(out var bullet))
        {
            bullet.SetTarget(target, damage);
        }
    }
}
