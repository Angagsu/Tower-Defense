using UnityEngine;
using UnityEngine.UI;

public class SelectHeroUIHandler : MonoBehaviour
{
    [SerializeField] private Button archerHeroButton;
    [SerializeField] private Button knightHeroButton;
    [Space(10)]
    [SerializeField] private Image[] heroesHealthBars;


    [SerializeField] private BuildsController buildsController;

    private BaseHero[] heroes;    
    private Camera mainCamera;
    private PlayerInputHandler playerInputHandler;


    [Inject]
    public void Costruct(PlayerInputHandler playerInputHandler)
    {
        this.playerInputHandler = playerInputHandler;
        playerInputHandler.TouchPressed += SelectHeroOnClick;
    }

    private void Awake()
    {
        mainCamera = Camera.main;
    }


    public void SetHeroes(BaseHero[] heroes)
    {
        this.heroes = heroes;

        archerHeroButton.onClick.AddListener(() =>
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

        knightHeroButton.onClick.AddListener(() =>
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

        for (int i = 0; i < heroes.Length; i++)
        {
            heroes[i].DamageTaked += OnHeroDamageTaked;
        }
    }

    private void OnHeroDamageTaked(BaseHero hero, float health, float startHealth)
    {
        for (int i = 0; i < heroes.Length; i++)
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
