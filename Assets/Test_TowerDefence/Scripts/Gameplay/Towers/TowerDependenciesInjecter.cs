using Assets.Scripts.Tower;
using UnityEngine;

public class TowerDependenciesInjecter : MonoBehaviour
{
    [SerializeField] private TouchBuildingArea touchBuildingArea;
    [SerializeField] private TowerUpgradeUI towerUpgradeUI;
    [SerializeField] private GameplayStates gameplayStates;
    [SerializeField] private ProjectilesFactoriesService projectilesFactoriesService;
    private PlayerInputHandler playerInputHandler;



    private void Awake()
    {
        playerInputHandler = ServiceLocator.GetService<PlayerInputHandler>();
    }

    public void SetTowerDependencies(BaseTower tower)
    {
        tower.Construct(gameplayStates, projectilesFactoriesService);
    }

    public void SetDefenderTowerDependencies(DefenderTower tower)
    {
        tower.Construct(playerInputHandler, gameplayStates, towerUpgradeUI, touchBuildingArea, projectilesFactoriesService);
    }
}
