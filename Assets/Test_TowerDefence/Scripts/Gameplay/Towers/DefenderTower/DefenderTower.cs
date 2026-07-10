using UnityEngine;

namespace Assets.Scripts.Tower
{
    public class DefenderTower : BaseTower, IAttackableTower
    {
        [SerializeField] DefendersController defendersController;

        public void Construct(PlayerInputHandler playerInputHandler, GameplayStates gameplayStates, TowerUpgradeUI towerUpgradeUI, TouchBuildingArea touchBuildingArea, ProjectilesFactoriesService projectilesFactoriesService, TowerSO towerSO)
        {
            defendersController.Construct(playerInputHandler, gameplayStates, towerUpgradeUI, touchBuildingArea, projectilesFactoriesService, towerSO);
        }

        protected override void Awake() { }

        protected override void Update() { }
        protected override void OnDisable() { }

        public override void Construct(GameplayStates gameplayStateHandler, ProjectilesFactoriesService projectilesFactoriesService) { }

        public override void SetParams(TowerSO towerSO)
        {
            base.SetParams(towerSO);
        }
        
        public override void TakeDamage(AttackStats enemy)
        {
            base.TakeDamage(enemy);
        }

        public override void Repair()
        {
            base.Repair();
        }

        protected override void DetectTarget()
        {
            base.DetectTarget();
        }

        protected override void Crash()
        {
            base.Crash();
        }
    }
}