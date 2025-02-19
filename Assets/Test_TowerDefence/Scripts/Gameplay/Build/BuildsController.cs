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
    [SerializeField] private BuildsControllerSFXHandler builderSFXHandler;

    private BaseHero[] heroes;
    private DefenderUnit[] defenders;
    private TowerFullBlueprintSO towerFullBlueprintSO;

    public bool CanBuild { get { return towerFullBlueprintSO != null; } }
    public bool HasManey { get { return PlayerStats.Money >= towerFullBlueprintSO.Towers[0].Cost; } }

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

    private void OnEnable()
    {
        playerInputHandler.TouchPressed += OnTouchGroundDeselectAll;
    }

    private void OnDisable()
    {
        playerInputHandler.TouchPressed -= OnTouchGroundDeselectAll;
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
        builderSFXHandler.PlayBuildSFX();
        towerFullBlueprintSO = tower;
        selectedBuildingArea.SetTower(tower);
        towerBuilder.SetBuildingAreaAndTowerBlueprint(tower, selectedBuildingArea);
        towerBuilder.Build();
        DeselectGround();
    }

    public void UpgradeTower()
    {
        SetDefendersBeforeDestroy();

        builderSFXHandler.PlayUpgradeSFX();

        towerBuilder.SetBuildingAreaAndTowerBlueprint(towerFullBlueprintSO, selectedBuildingArea);
        towerBuilder.UpgradeTower();
    }

    public void UpdateSecondTime()
    {
        SetDefendersBeforeDestroy();
        builderSFXHandler.PlayUpgradeSFX();

        towerBuilder.SetBuildingAreaAndTowerBlueprint(towerFullBlueprintSO, selectedBuildingArea);
        towerBuilder.SecondTimeUpgradeTower();
    }

    public void UpdateThirdTime()
    {
        SetDefendersBeforeDestroy();

        builderSFXHandler.PlayUpgradeSFX();

        towerBuilder.SetBuildingAreaAndTowerBlueprint(towerFullBlueprintSO, selectedBuildingArea);
        towerBuilder.ThirdTimeUpgradeTower();
    }

    public void SellTower()
    {
        SetDefendersBeforeDestroy();

        builderSFXHandler.PlayDestroySFX();

        selectedBuildingArea.DestroyTower();
    }

    private void OnTouchGroundDeselectAll(Vector2 touchPosition)
    {
        Ray ray = mainCamera.ScreenPointToRay(touchPosition);

        if (Physics.Raycast(ray, out RaycastHit raycastHit) && raycastHit.collider.gameObject.layer.CompareTo(groundLayer) == 0)
        {
            DeselectGround();
            DeSelectHeroes();
            upgradeTowerUI.DeActivateDefendersMoveZone();
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
        //return EventSystem.current.IsPointerOverGameObject(Touchscreen.current.primaryTouch.touchId.ReadValue());
        //return EventSystem.current.IsPointerOverGameObject(playerInputHandler.TouchPressValue.GetHashCode());
        return EventSystem.current.IsPointerOverGameObject();
    }
}
