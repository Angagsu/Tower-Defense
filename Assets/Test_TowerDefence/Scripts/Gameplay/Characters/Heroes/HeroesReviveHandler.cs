using System;
using System.Collections;
using UnityEngine;

public class HeroesReviveHandler : MonoBehaviour
{
    public event Action<BaseHero> HeroAdded;
    

    [SerializeField] private Transform[] heroesPosition;
    [SerializeField] private BaseHero[] heroesPrefab;

    private GameObject[] heroesOnScene = new GameObject[2];
    private BaseHero[] heroes = new BaseHero[2];

    private Coroutine coroutine;


    private void Awake()
    {
        
    }

    private void InstantiateHeroes()
    {
        for (int i = 0; i < heroesPrefab.Length; i++)
        {
            heroesOnScene[i] = Instantiate(heroesPrefab[i], heroesPosition[i].transform.position, Quaternion.identity).gameObject;
            heroes[i] = heroesOnScene[i].GetComponent<BaseHero>();
            heroes[i].Died += OnHeroDied;
        }
    }

    private void Start()
    {
        InstantiateHeroes();
        for (int i = 0; i < heroes.Length; i++)
        {
            HeroAdded?.Invoke(heroes[i]);
        }
    }

    private void OnHeroDied()
    {
        for (int i = 0; i < heroes.Length; i++)
        {

            if (heroes[i].IsDead)
            {
                heroes[i].enabled = false;

                if (coroutine != null)
                {
                    StopCoroutine(coroutine);
                }

                coroutine = StartCoroutine(TimerForReviveHero(heroes[i]));
            }
        }
    }

    private IEnumerator TimerForReviveHero(BaseHero h)
    {
        yield return new WaitForSeconds(h.ReviveTimer);
        h.enabled = true;
        h.OnRevive();
    }

    private void OnDisable()
    {
        for (int i = 0; i < heroesPrefab.Length; i++)
        {
            heroesPrefab[i].Died -= OnHeroDied;
        }
    }

    public BaseHero[] GetHeroesOnScene()
    {
        return heroes;
    }
}
