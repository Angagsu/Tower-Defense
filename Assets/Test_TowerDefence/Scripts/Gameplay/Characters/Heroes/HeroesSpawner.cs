using System.Collections.Generic;
using System;
using UnityEngine;

public class HeroesSpawner : MonoBehaviour
{
    public event Action<BaseHero> HeroAdded;

    [SerializeField] private HeroDependenciesInjector dependenciesInjector;
    [SerializeField] private GameplayHeroesButtonsController selectHeroOnUIHandler;
    [SerializeField] private HeroesButtonsSpawner heroesButtonsUI;
    [SerializeField] private Transform[] heroesPosition;
    [SerializeField] private BaseHero[] heroesPrefab;

    private List<BaseHero> heroes = new();

    private HeroesDataStoreService heroesDataStoreService;
    private List<HeroSO> selectedHeroes = new();

    [Inject]
    public void Construct(HeroesDataStoreService heroesDataStoreService)
    {
        this.heroesDataStoreService = heroesDataStoreService;

        for(int i = 0; i < heroesDataStoreService.SelectedAcquiredHeroesByIndex.Count; i++)
        {
            selectedHeroes.Add(heroesDataStoreService.AllHeroes.AcquiredHeroes[heroesDataStoreService.SelectedAcquiredHeroesByIndex[i]]);
            heroesPrefab[i] = selectedHeroes[i].Prefab;
        }
        InstantiateHeroes();
        heroesButtonsUI.SetDependencies(this, selectHeroOnUIHandler);
    }

    private void InstantiateHeroes()
    {
        for (int i = 0; i < heroesPrefab.Length; i++)
        {
            heroes.Add(Instantiate(heroesPrefab[i], heroesPosition[i].transform.position, Quaternion.identity));
            dependenciesInjector.SetHeroDependencies(heroes[i], selectedHeroes[i]);
            HeroAdded?.Invoke(heroes[i]);
        }

        selectHeroOnUIHandler.SetHeroes(heroes);
    } 

    public List<BaseHero> GetHeroesOnScene()
    {
        return heroes;
    }

    public List<HeroSO> GetHeroSOs()
    {
        return selectedHeroes;
    }
}
