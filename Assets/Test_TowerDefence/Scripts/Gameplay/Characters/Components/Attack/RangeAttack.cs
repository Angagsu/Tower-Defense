using Assets.Scripts;
using UnityEngine;

public class RangeAttack : BaseAttack
{

    [SerializeField] private BaseProjectile bulletPrefab;
    [SerializeField] private Transform bulletInstPoint;
    [SerializeField] private Transform bulletInstPoint1;
    [SerializeField] private Transform bulletInstPoint2;
    [SerializeField] private bool hasTwoInstantiatePoint;


    private ProjectilesFactoriesService projectilesFactoryService;
    private BaseProjectile projectile;

    public override void Costruct(ProjectilesFactoriesService projectilesFactoriesService)
    {
        this.projectilesFactoryService = projectilesFactoriesService;
    }

    public override void AttackTarget(AttackStats stats, Transform target, float damage, Character targetCharacter = null, IAttackable attackableTarget = null)
    {
        projectile = projectilesFactoryService.GetProjectileByType(bulletPrefab, bulletInstPoint, bulletInstPoint.rotation);
        projectile.SetTarget(stats, targetCharacter, target, damage, attackableTarget);
    }
    public override void AttackWithPassiveSkill(AttackStats attackStats, Transform target, float damage, bool hasPassiveSkill = default, float passiveValue_1 = default, float passiveValue_2 = default, Character targetCharacter = null, IAttackable attackableTarget = null)
    {
        projectile = projectilesFactoryService.GetProjectileByType(bulletPrefab, bulletInstPoint, bulletInstPoint.rotation);
        projectile.SetTarget(attackStats,targetCharacter, target, damage, hasPassiveSkill, passiveValue_1, passiveValue_2, attackableTarget);
    }

    public override void AttackWithPassiveSkillAndDifferentHands(AttackStats stats, Transform target, float damage, bool hasPassiveSkill, float passiveValue_1 = default, float passiveValue_2 = default, Character targetCharacter = null, bool isRightHandAttack = default, IAttackable attackableTarget = null)
    {
        if (hasTwoInstantiatePoint)
        {
            if (isRightHandAttack)
            {
                projectile = projectilesFactoryService.GetProjectileByType(bulletPrefab, bulletInstPoint1, bulletInstPoint.rotation);
            }
            else
            {
                projectile = projectilesFactoryService.GetProjectileByType(bulletPrefab, bulletInstPoint2, bulletInstPoint.rotation);
            }
        }
        else
        {
            projectile = projectilesFactoryService.GetProjectileByType(bulletPrefab, bulletInstPoint, bulletInstPoint.rotation);
        }

        projectile.SetTarget(stats, targetCharacter, target, damage, hasPassiveSkill, passiveValue_1, passiveValue_2, attackableTarget);
    }
}
