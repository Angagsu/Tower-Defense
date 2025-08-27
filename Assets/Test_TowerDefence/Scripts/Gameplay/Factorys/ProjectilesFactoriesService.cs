using System.Collections.Generic;
using UnityEngine;

public class ProjectilesFactoriesService : MonoBehaviour
{
    [SerializeField] private List<BaseProjectileFactory> projectilesFactories;
    [SerializeField] private GameplayStates gameplayStates;


    public BaseProjectile GetProjectileByType(BaseProjectile projectile, Transform projectilePosition, Quaternion rotation)
    {      
        for (int i = 0; i < projectilesFactories.Count; i++)
        {
            if (projectilesFactories[i].ProjectileUpgradeLevel == projectile.UpgradeLevel)
            {
                var createdProjectile = projectilesFactories[i].GetProjectileByType(projectile, projectilePosition, rotation);
                createdProjectile.Construct(gameplayStates);
                return createdProjectile;
            }
        }
        return null;
    }
}
