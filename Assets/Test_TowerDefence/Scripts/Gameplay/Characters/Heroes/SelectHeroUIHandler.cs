using UnityEngine;
using UnityEngine.UI;

public class SelectHeroUIHandler : MonoBehaviour
{
    [SerializeField] private Button archerHeroButton;
    [SerializeField] private Button knightHeroButton;
    [SerializeField] private HeroesReviveHandler heroesReviveHandler;
    [SerializeField] private BuildsController buildsController;

    private BaseHero[] heroes;    
    private Camera mainCamera;


    private PlayerInputHandler playerInputHandler;


    private void Awake()
    {
        playerInputHandler = PlayerInputHandler.Instance;
        heroes = heroesReviveHandler.GetHeroesOnScene();
        mainCamera = Camera.main;

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
    }

    private void OnEnable()
    {
        playerInputHandler.TouchPressed += SelectHeroOnClick;
    }

    private void OnDisable()
    {
        playerInputHandler.TouchPressed -= SelectHeroOnClick;
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
}
