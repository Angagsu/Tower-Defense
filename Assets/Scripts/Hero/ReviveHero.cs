using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviveHero : MonoBehaviour
{
    [SerializeField] private Transform[] heroesPosition; 
    [Space(5f)]
    [SerializeField] private GameObject[] heroesObj;
    private GameObject[] heroes = new GameObject[2];
    private Hero[] hero = new Hero[2];

    private void Awake()
    {
        InstantiateTheHero();
    }

    private void Start()
    {
        heroes[0] = GameObject.FindGameObjectWithTag("ArcherHero");
        heroes[1] = GameObject.FindGameObjectWithTag("KnightHero");
        for (int i = 0; i < heroes.Length; i++)
        {
            hero[i] = heroes[i].GetComponent<Hero>();
        }
    }

    private void Update()
    {
        OnHeroDied();
    }

    private void InstantiateTheHero()
    {
        for (int i = 0; i < heroesObj.Length; i++)
        {
            Instantiate(heroesObj[i], heroesPosition[i].position, Quaternion.identity);
            
        }
    }
    private void OnHeroDied()
    {
        for (int i = 0; i < heroesObj.Length; i++)
        {
            
            if (hero[i].isHeroDead)
            {
                //heroes[i].SetActive(false);
                StartCoroutine(TimerForReviveHero(hero[i]));
            }
        }

    }

    private IEnumerator TimerForReviveHero(Hero hero)
    {
        while (hero.isHeroDead)
        {
            yield return new WaitForSeconds(4);
            hero.ReviveHero();
            hero.gameObject.SetActive(true);
        }
    }

    public void SelectArcherHeroOnUI()
    {
        hero[0].SelectArcherHeroOnUI();
    }

    public void SelectKnightHeroOnUI()
    {
        hero[1].SelectKnightHeroOnUI();
    }
}
