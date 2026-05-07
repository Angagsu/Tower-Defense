using Assets.Scripts.Tower;
using System.Collections.Generic;
using UnityEngine;

public class DetectionHelper : MonoBehaviour
{
    public static DetectionHelper Instance { get; private set; }   

    public List<BaseMonster> Monsters { get; private set; } = new();
    public List<BaseHero> Heroes { get; private set; } = new();
    public List<BaseTower> Towers { get; private set; }


    [SerializeField] private MonstersFactoriesService monstersFactoriesService;
    [SerializeField] private HeroesSpawner heroesSpawner;
    [SerializeField] private BuildsController buildsController;
    [SerializeField] private TowerBuilder towerBuilder;



    private void Awake()
    {
        Instance = this;

        monstersFactoriesService.MonsterCountIncreased += OnMonstersCountIncreased;
        monstersFactoriesService.MonsterCountDecreased += OnMonstersCountDecreased;

        heroesSpawner.HeroAdded += OnHeroesCountIncreased;
        buildsController.DefendersRemoved += OnDefendersRemoved;

        Towers = towerBuilder.Towers;
    }

    private void OnDefendersRemoved(DefenderUnit defender)
    {
        Heroes.Remove(defender);
    }

    private void OnMonstersCountIncreased(BaseMonster monster)
    {
        Monsters.Add(monster);
    }

    private void OnMonstersCountDecreased(BaseMonster monster)
    {
        Monsters.Remove(monster);
    }

    public void OnHeroesCountIncreased(BaseHero hero)
    {
        Heroes.Add(hero);
    }

    private void OnDestroy()
    {
        monstersFactoriesService.MonsterCountIncreased -= OnMonstersCountIncreased;
        monstersFactoriesService.MonsterCountDecreased -= OnMonstersCountDecreased;

        heroesSpawner.HeroAdded -= OnHeroesCountIncreased;
        buildsController.DefendersRemoved -= OnDefendersRemoved;
    }
}
