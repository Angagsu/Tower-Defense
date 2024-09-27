using Assets.Scripts.Hero;
using UnityEngine;
using UnityEngine.UI;

public class SelectHeroUIHandler : MonoBehaviour
{
    [SerializeField] private Button archerHeroButton;
    [SerializeField] private Button knightHeroButton;
    [SerializeField] private HeroesReviveHandler heroesReviveHandler;

    private BaseHero[] heroes;    
    private TowerBuildManager towerBuildManager;
    private Camera mainCamera;

    private void Awake()
    {
        heroes = heroesReviveHandler.GetHeroesOnScene();
        towerBuildManager = TowerBuildManager.Instance;
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

    private void Update()
    {
        SelectHeroOnClick();
    }

    public void SelectHeroOnClick()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit) && raycastHit.collider && raycastHit.collider.gameObject.TryGetComponent(out BaseHero hero))
            {
                if (hero == heroes[0])
                {
                    heroes[0].Select();
                    heroes[1].Deselect();
                    towerBuildManager.DeselectGround();
                }
                else
                {
                    heroes[1].Select();
                    heroes[0].Deselect();
                    towerBuildManager.DeselectGround();
                }  
            }
        }
    }
}
