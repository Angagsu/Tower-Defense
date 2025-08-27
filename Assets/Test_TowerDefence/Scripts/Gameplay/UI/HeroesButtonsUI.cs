using UnityEngine;
using UnityEngine.UI;


public class HeroesButtonsUI : MonoBehaviour
{
    [SerializeField] private HeroButtonFade[] heroButtonFades;
    [SerializeField] private HeroesSpawner heroesSpawner;

    private BaseHero[] heroes;

    private void Start()
    {
        heroes = heroesSpawner.GetHeroesOnScene();

        for (int i = 0; i < heroes.Length; i++)
        {
            heroButtonFades[i].SetHero(heroes[i]);
        }
    }  
}
