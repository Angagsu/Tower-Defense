using UnityEngine;

public class MonsterDependenciesInjecter : MonoBehaviour
{
    [SerializeField] private GameplayPlayerDataHandler gameplayPlayerDataHandler;
    [SerializeField] private GameplayStates gameplayStateHandler;
    [SerializeField] private MonstersFactoriesService monstersFactoriesService;
    [SerializeField] private ProjectilesFactoriesService projectilesFactoriesService;

    public void SetMonsterDependencies(BaseMonster monster)
    {
        monster.Construct(gameplayPlayerDataHandler, gameplayStateHandler, monstersFactoriesService, projectilesFactoriesService);
    }
}
