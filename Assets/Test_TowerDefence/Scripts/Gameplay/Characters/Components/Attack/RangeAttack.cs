using Assets.Scripts;
using UnityEngine;

public class RangeAttack : BaseAttack
{

    [SerializeField] private BaseProjectile bulletPrefab;
    [SerializeField] private Transform bulletInstPoint;
    

    private ProjectilesFactoriesService projectilesFactoryService;
    private BaseProjectile projectile;

    public override void Costruct(ProjectilesFactoriesService projectilesFactoriesService)
    {
        this.projectilesFactoryService = projectilesFactoriesService;
    }

    public override void AttackTarget(Transform target, float damage, Character targetCharacter = null)
    {
        projectile = projectilesFactoryService.GetProjectileByType(bulletPrefab, bulletInstPoint, bulletInstPoint.rotation);
        projectile.SetTarget(targetCharacter, target, damage);
    }
}
