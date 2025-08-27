using System;
using UnityEngine;

public class HeroesSpawner : MonoBehaviour
{
    public event Action<BaseHero> HeroAdded;

    [SerializeField] private HeroDependenciesInjector dependenciesInjector;
    [SerializeField] private SelectHeroUIHandler selectHeroOnUIHandler;
    [SerializeField] private Transform[] heroesPosition;
    [SerializeField] private BaseHero[] heroesPrefab;

    private GameObject[] heroesOnScene = new GameObject[2];
    private BaseHero[] heroes = new BaseHero[2];
  



    private void Start()
    {
        InstantiateHeroes();

        for (int i = 0; i < heroes.Length; i++)
        {
            HeroAdded?.Invoke(heroes[i]);
        }
    }

    private void InstantiateHeroes()
    {
        for (int i = 0; i < heroesPrefab.Length; i++)
        {
            heroesOnScene[i] = Instantiate(heroesPrefab[i], heroesPosition[i].transform.position, Quaternion.identity).gameObject;
            heroes[i] = heroesOnScene[i].GetComponent<BaseHero>();
            dependenciesInjector.SetHeroDependencies(heroes[i]);
        }

        selectHeroOnUIHandler.SetHeroes(heroes);
    } 

    public BaseHero[] GetHeroesOnScene()
    {
        return heroes;
    }
}
