using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectHeroOnUI : MonoBehaviour
{
    [SerializeField] private BuildsController buildsController;

    private Camera mainCamera;
    private PlayerInputHandler playerInputHandler;

    private Button firstHeroButton;
    private Button secondHeroButton;

    private List<BaseHero> heroes;
    private List<Button> heroesButtons = new();
    private List<GameplayHeroButton> heroButtons;



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

    public void SetHeroesButtons(HeroesButtonsUI heroesButtonsUI)
    {
        heroButtons = heroesButtonsUI.GetHeroButtons();

        for (int i = 0; i < heroButtons.Count; i++)
        {
            heroesButtons.Add(heroButtons[i].gameObject.GetComponent<Button>());
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
    }

    public void SetHeroes(List<BaseHero> heroes)
    {
        this.heroes = heroes;
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
