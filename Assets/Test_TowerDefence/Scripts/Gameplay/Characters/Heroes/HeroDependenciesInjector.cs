using UnityEngine;

public class HeroDependenciesInjector : MonoBehaviour
{
    [SerializeField] private ProjectilesFactoriesService projectilesFactoriesService;
    [SerializeField] private GameplayStates gameplayStates;

    private PlayerInputHandler playerInputHandler;



    private void Awake()
    {
        playerInputHandler = ServiceLocator.GetService<PlayerInputHandler>();
    }

    public void SetHeroDependencies(BaseHero hero)
    {
        hero.Construct(playerInputHandler, gameplayStates, projectilesFactoriesService);
    }
}
