using UnityEngine;
using System.Collections.Generic;



public class HeroesButtonsUI : MonoBehaviour
{
    [SerializeField] private HeroButtonFade prefab;
    [SerializeField] private RectTransform[] spawnPoints;
    [SerializeField] private RectTransform parentContainer;
    [SerializeField] private HeroesSpawner heroesSpawner;

    private List<HeroButtonFade> heroButtonFades = new();
    private List<BaseHero> heroes;

    public void SetDependencies(HeroesSpawner heroesSpawner, SelectHeroUIHandler selectHeroUIHandler)
    {
        this.heroesSpawner = heroesSpawner;

        heroes = heroesSpawner.GetHeroesOnScene();
        var heroSOs = heroesSpawner.GetHeroSOs();

        for (int i = 0; i < heroes.Count; i++)
        {
            heroButtonFades.Add(Instantiate(prefab, spawnPoints[i].position, Quaternion.identity, parentContainer));
            heroButtonFades[i].SetHero(heroes[i]);
            heroButtonFades[i].SetHeroImageSprite(heroSOs[i].Sprite);
        }

        selectHeroUIHandler.SetHeroesButtons();
    }
    
    public List<HeroButtonFade> GetHeroButtons()
    {
        return heroButtonFades;
    } 
}
