using UnityEngine;

namespace Assets.Scripts.Tower
{
    public class DefenderTower : MonoBehaviour, IAttackableTower
    {
        [SerializeField] DefendersController defendersController;

        public void Construct(PlayerInputHandler playerInputHandler, GameplayStates gameplayStates, TowerUpgradeUI towerUpgradeUI, TouchBuildingArea touchBuildingArea, ProjectilesFactoriesService projectilesFactoriesService)
        {
            defendersController.Construct(playerInputHandler, gameplayStates, towerUpgradeUI, touchBuildingArea, projectilesFactoriesService);
        }

        public void TakeDamage(float damageAmount)
        {
            throw new System.NotImplementedException();
        }
    }
}