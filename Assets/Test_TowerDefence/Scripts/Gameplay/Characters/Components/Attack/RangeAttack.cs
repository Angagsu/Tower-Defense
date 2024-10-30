using UnityEngine;

public class RangeAttack : BaseAttack
{
    [SerializeField] private BaseProjectile bulletPrefab;
    [SerializeField] private Transform bulletInstPoint;

    private ProjectilesFactory projectilesFactory;
    private BaseProjectile projectile;

    private void Awake()
    {
        projectilesFactory = FindAnyObjectByType<ProjectilesFactory>();
    }

    public override void AttackTarget(Transform target, float damage)
    {
        projectile = projectilesFactory.GetProjectileByType(bulletPrefab, bulletInstPoint, bulletInstPoint.rotation);
        projectile.SetTarget(target, damage);
    }
}
