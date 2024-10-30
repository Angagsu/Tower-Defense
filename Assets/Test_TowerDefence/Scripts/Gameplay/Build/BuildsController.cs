using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildsController : MonoBehaviour
{
    public event Action<DefenderUnit> DefendersRemoved;

    [SerializeField] private TowerBuilder towerBuilder;
    [SerializeField] private HeroesReviveHandler heroesReviveHandler;
    [SerializeField] private TowerBuildUI towerBuildUI;
    [SerializeField] private TowerUpgradeUI upgradeTowerUI;

    private BaseHero[] heroes;
    private DefenderUnit[] defenders;
    private TowerFullBlueprintSO towerFullBlueprintSO;

    public bool CanBuild { get { return towerFullBlueprintSO != null; } }
    public bool HasManey { get { return PlayerStats.Money >= towerFullBlueprintSO.Tower.Cost; } }

    private BuildingArea selectedBuildingArea;
    private Camera mainCamera;
    private int groundLayer;


    private PlayerInputHandler playerInputHandler;
    private void Awake()
    {
        playerInputHandler = PlayerInputHandler.Instance;
        mainCamera = Camera.main;
        groundLayer = LayerMask.NameToLayer("Ground");
    }

    private void Start()
    {
        heroes = heroesReviveHandler.GetHeroesOnScene();
    }

    private void Update()
    {
        DeselectAllUIAndHeroesThenClickGround();
    }

    public void SelectedGroundForUpgradeTowerUI(BuildingArea buildingArea)
    {
        if (selectedBuildingArea == buildingArea)
        {
            DeselectGround();
            return;
        }

        DeSelectHeroes();
        
        if (towerBuildUI.IsGameObjectActive())
        {
            towerBuildUI.Hide();
        }

        selectedBuildingArea = buildingArea;
        towerFullBlueprintSO = selectedBuildingArea.GetTowerBlueprint();

        if (selectedBuildingArea.DefenderMovement == null)
        {
            upgradeTowerUI.ActivateOrDeactivateSelectDefendersButton(false);
        }
        else
        {
            upgradeTowerUI.ActivateOrDeactivateSelectDefendersButton(true);
        }

        upgradeTowerUI.SetTargetGround(buildingArea);
    }

    public void SelectedGroundForBuildTowerUI(BuildingArea buildingArea)
    {
        if (selectedBuildingArea == buildingArea)
        {
            DeselectGround();
            return;
        }

        DeSelectHeroes();

        if (upgradeTowerUI.IsGameObjectActive())
        {
            upgradeTowerUI.Hide();
        }

        selectedBuildingArea = buildingArea;
        towerFullBlueprintSO = selectedBuildingArea.GetTowerBlueprint();

        towerBuildUI.SetTargetGroundForBuilding(buildingArea);
    }

    public void BuildTowerByType(TowerFullBlueprintSO tower)
    {
        towerFullBlueprintSO = tower;
        selectedBuildingArea.SetTower(tower);
        towerBuilder.SetBuildingAreaAndTowerBlueprint(tower, selectedBuildingArea);
        towerBuilder.Build();
        DeselectGround();
    }

    public void UpgradeTower()
    {
        SetDefendersBeforeDestroy();

        towerBuilder.SetBuildingAreaAndTowerBlueprint(towerFullBlueprintSO, selectedBuildingArea);
        towerBuilder.UpgradeTower();
    }

    public void UpdateSecondTime()
    {
        SetDefendersBeforeDestroy();

        towerBuilder.SetBuildingAreaAndTowerBlueprint(towerFullBlueprintSO, selectedBuildingArea);
        towerBuilder.SecondTimeUpgradeTower();
    }

    public void UpdateThirdTime()
    {
        SetDefendersBeforeDestroy();

        towerBuilder.SetBuildingAreaAndTowerBlueprint(towerFullBlueprintSO, selectedBuildingArea);
        towerBuilder.ThirdTimeUpgradeTower();
    }

    public void SellTower()
    {
        SetDefendersBeforeDestroy();

        selectedBuildingArea.DestroyTower();
    }

    private void DeselectAllUIAndHeroesThenClickGround()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (playerInputHandler.TapInput > 0)
        {
            if (!IsMouseOverUI())
            {
                if (Physics.Raycast(ray, out RaycastHit raycastHit) && raycastHit.collider.gameObject.layer.CompareTo(groundLayer) == 0)
                {
                    DeselectGround();
                    DeSelectHeroes();
                    upgradeTowerUI.DeActivateDefendersMoveZone();
                }
            }
        }
    }

    private void SetDefendersBeforeDestroy()
    {
        if (selectedBuildingArea.DefenderMovement)
        {
            defenders = selectedBuildingArea.DefenderMovement.GetDefendersArray();

            for (int i = 0; i < defenders.Length; i++)
            {
                DefendersRemoved?.Invoke(defenders[i]);
            }
        }   
    }

    public void DeselectGround()
    {
        selectedBuildingArea = null;
        upgradeTowerUI.Hide();
        towerBuildUI.Hide();
    }

    private void DeSelectHeroes()
    {
        for (int i = 0; i < heroes.Length; i++)
        {
            heroes[i].Deselect();
        }
    }

    private bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
    }
}
