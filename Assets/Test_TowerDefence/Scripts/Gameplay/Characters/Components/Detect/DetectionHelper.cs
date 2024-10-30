using System.Collections.Generic;
using UnityEngine;

public class DetectionHelper : MonoBehaviour
{
    public static DetectionHelper Instance { get; private set; }   

    public List<BaseMonster> Monsters { get; private set; } = new();
    public List<BaseHero> Heroes { get; private set; } = new();

    [SerializeField] private FactoriesService factoriesService;
    [SerializeField] private HeroesReviveHandler heroesReviveHandler;
    [SerializeField] private BuildsController buildsController;

    private void Awake()
    {
        Instance = this;

        factoriesService.MonsterCountChanged += OnMonstersCountChanged;
        heroesReviveHandler.HeroAdded += OnHeroesCountIncreased;
        buildsController.DefendersRemoved += OnDefendersRemoved;
    }

    private void OnDefendersRemoved(DefenderUnit defender)
    {
        Heroes.Remove(defender);
    }

    private void OnMonstersCountChanged(BaseMonster monster)
    {
        Monsters.Add(monster);  
    }

    public void OnHeroesCountIncreased(BaseHero hero)
    {
        Heroes.Add(hero);
    }

    private void OnDestroy()
    {
        factoriesService.MonsterCountChanged -= OnMonstersCountChanged;
        heroesReviveHandler.HeroAdded -= OnHeroesCountIncreased;
        buildsController.DefendersRemoved -= OnDefendersRemoved;
    }
}
