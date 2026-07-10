using UnityEngine;
using System.Collections.Generic;



public class HeroesButtonsSpawner : MonoBehaviour
{
    [Header("Heroes Buttons")]
    [SerializeField] private GameplayHeroButton HeroButtonPrefab;
    [SerializeField] private RectTransform[] heroesButtonsSpawnPoints;
    [SerializeField] private RectTransform heroesButtonsParentContainer;

    [Header("Heroes Active Skill Buttons")]
    [SerializeField] private GameplayHeroActiveSkillUI activeSkillButtonPrefab;
    [SerializeField] private RectTransform[] activeSkillsSpawnPoints;
    [SerializeField] private RectTransform activeSkillsParentContainer;


    private HeroesSpawner heroesSpawner;
    private List<GameplayHeroButton> heroButtons = new();
    private List<GameplayHeroActiveSkillUI> heroActiveSkillsButtons = new();
    private List<BaseHero> heroes;

    public void SetDependencies(HeroesSpawner heroesSpawner, GameplayHeroesButtonsController heroButtonsController)
    {
        this.heroesSpawner = heroesSpawner;

        heroes = heroesSpawner.GetHeroesOnScene();
        var heroSOs = heroesSpawner.GetHeroSOs();

        for (int i = 0; i < heroes.Count; i++)
        {
            heroButtons.Add(Instantiate(HeroButtonPrefab, heroesButtonsSpawnPoints[i].position, Quaternion.identity, heroesButtonsParentContainer));
            heroActiveSkillsButtons.Add(Instantiate(activeSkillButtonPrefab, activeSkillsSpawnPoints[i].position, Quaternion.identity, activeSkillsParentContainer));

            heroButtons[i].Setup(heroButtonsController, heroSOs[i], heroes[i], i);
            heroActiveSkillsButtons[i].Setup(heroButtonsController, heroSOs[i], heroes[i], i);
        }

        heroButtonsController.SetHeroesButtons(this);
    }
    
    public List<GameplayHeroButton> GetHeroButtons()
    {
        return heroButtons;
    } 

    public List<GameplayHeroActiveSkillUI> GetHeroActiveSkillsButtons()
    {
        return heroActiveSkillsButtons;
    }
}
