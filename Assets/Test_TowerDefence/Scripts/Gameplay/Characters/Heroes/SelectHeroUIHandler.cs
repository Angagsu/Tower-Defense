using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectHeroUIHandler : MonoBehaviour
{
    private Button firstHeroButton;
    private Button secondHeroButton;
    private List<Button> heroesButtons = new();
    [Space(10)]
    [SerializeField] private Image[] heroesHealthBars;

    [SerializeField] private HeroesButtonsUI heroesButtonsUI;
    [SerializeField] private BuildsController buildsController;

    private List<BaseHero> heroes;    
    private Camera mainCamera;
    private PlayerInputHandler playerInputHandler;
    private List<HeroButtonFade> herosButtonsFades;

    [Inject]
    public void Construct(PlayerInputHandler playerInputHandler)
    {
        this.playerInputHandler = playerInputHandler;
        playerInputHandler.TouchPressed += SelectHeroOnClick;
    }

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    public void SetHeroesButtons()
    {
        herosButtonsFades = heroesButtonsUI.GetHeroButtons();
        for (int i = 0; i < herosButtonsFades.Count; i++)
        {
            heroesButtons.Add(herosButtonsFades[i].gameObject.GetComponent<Button>());
            heroesHealthBars[i] = herosButtonsFades[i].GetHealthBarImage();
        }

        firstHeroButton = heroesButtons[0];
        secondHeroButton = heroesButtons[1];

        firstHeroButton.onClick.AddListener(() =>
        {
            if (heroes[0].IsSelected)
            {
                heroes[0].Deselect();
            }
            else if (!heroes[0].IsSelected)
            {
                heroes[0].Select();
                heroes[1].Deselect();
            }
        });
        
        secondHeroButton.onClick.AddListener(() =>
        {
            if (heroes[1].IsSelected)
            {
                heroes[1].Deselect();
            }
            else if (!heroes[1].IsSelected)
            {
                heroes[1].Select();
                heroes[0].Deselect();
            }
        });
        
        for (int i = 0; i < heroes.Count; i++)
        {
            heroes[i].DamageTaked += OnHeroDamageTaked;
        }
    }

    public void SetHeroes(List<BaseHero> heroes)
    {
        this.heroes = heroes;
    }

    private void OnHeroDamageTaked(BaseHero hero, float health, float startHealth)
    {
        for (int i = 0; i < heroes.Count; i++)
        {
            if (heroes[i] == hero)
            {
                heroesHealthBars[i].fillAmount = health / startHealth;
            }
        }
    }

    public void SelectHeroOnClick(Vector2 touchPosition)
    {
        Ray ray = mainCamera.ScreenPointToRay(touchPosition);

        if (Physics.Raycast(ray, out RaycastHit raycastHit) && raycastHit.collider && raycastHit.collider.gameObject.TryGetComponent(out BaseHero hero))
        {
            if (hero == heroes[0])
            {
                heroes[0].Select();
                heroes[1].Deselect();
                buildsController.DeselectGround();
            }
            else
            {
                heroes[1].Select();
                heroes[0].Deselect();
                buildsController.DeselectGround();
            }  
        }
    }

    private void OnDisable()
    {
        playerInputHandler.TouchPressed -= SelectHeroOnClick;
    }
}
