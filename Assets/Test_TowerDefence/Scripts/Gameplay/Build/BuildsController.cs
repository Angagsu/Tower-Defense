using Assets.Scripts.Tower;
using System;
using System.Collections.Generic;
using UnityEngine;



public class BuildsController : MonoBehaviour
{
    public event Action<DefenderUnit> DefendersRemoved;

    [field: SerializeField] public GameplayPlayerDataHandler PlayerDataHandler {  get; private set; }

    [SerializeField] private TowerBuilder towerBuilder;
    [SerializeField] private HeroesSpawner heroesSpawner;
    [SerializeField] private TowerBuildUI towerBuildUI;
    [SerializeField] private TowerUpgradeUI upgradeTowerUI;
    [SerializeField] private BuildsControllerSFXHandler builderSFXHandler;

    private List<BaseHero> heroes;
    private DefenderUnit[] defenders;

    private PlayerInputHandler playerInputHandler;
    private TowerFullBlueprintSO towerFullBlueprintSO;
    private BuildingArea selectedBuildingArea;
    private Camera mainCamera;

    private int groundLayer;



    [Inject]
    public void Costruct(PlayerInputHandler playerInputHandler)
    {
        this.playerInputHandler = playerInputHandler;
        playerInputHandler.TouchPressed += OnTouchGroundDeselectAll;
    }

    private void Awake()
    {
        mainCamera = Camera.main;
        groundLayer = LayerMask.NameToLayer("Ground");
    }

    private void Start()
    {
        heroes = heroesSpawner.GetHeroesOnScene();
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
        towerBuilder.SetBuildingAreaAndTowerBlueprint(towerFullBlueprintSO, selectedBuildingArea);

        if (selectedBuildingArea.DefendersController == null)
        {
            upgradeTowerUI.ActivateOrDeactivateSelectDefendersButton(false);
        }
        else
        {
            upgradeTowerUI.ActivateOrDeactivateSelectDefendersButton(true);
        }

        upgradeTowerUI.SetTargetGround(towerFullBlueprintSO, buildingArea);
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

    public void BuildTowerByType(TowerFullBlueprintSO towerFullBlueprintSO, TowerSO towerSO)
    {
        builderSFXHandler.PlayBuildSFX();
        this.towerFullBlueprintSO = towerFullBlueprintSO;
        selectedBuildingArea.SetTower(towerSO,towerFullBlueprintSO);

        towerBuilder.SetBuildingAreaAndTowerBlueprint(towerFullBlueprintSO, selectedBuildingArea);
        towerBuilder.Build();

        DeselectGround();
    }

    public void UpgradeTower()
    {
        SetDefendersBeforeDestroy();

        builderSFXHandler.PlayUpgradeSFX();

        towerBuilder.UpgradeTower();
    }

    public void UpdateSecondTime()
    {
        SetDefendersBeforeDestroy();

        builderSFXHandler.PlayUpgradeSFX();

        towerBuilder.SecondTimeUpgradeTower();
    }

    public void UpdateThirdTime()
    {
        SetDefendersBeforeDestroy();

        builderSFXHandler.PlayUpgradeSFX();

        towerBuilder.ThirdTimeUpgradeTower();
    }

    public void SellTower()
    {
        SetDefendersBeforeDestroy();

        builderSFXHandler.PlayDestroySFX();

        towerBuilder.SellTower();
    }

    public void Repair()
    {
        if (PlayerDataHandler.Money >= selectedBuildingArea.CurrentTower.GetRepairCost())
        {
            PlayerDataHandler.ReduceMoney(selectedBuildingArea.CurrentTower.GetRepairCost());
            selectedBuildingArea.CurrentTower.Repair();
        }
        else
        {
            Debug.Log("Not Enough Money to Repair !!!");
        }
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
        if (selectedBuildingArea.DefendersController)
        {
            defenders = selectedBuildingArea.DefendersController.GetDefendersArray();

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

    public void DeselectTowerUpgradeUIObject()
    {
        upgradeTowerUI.Hide();
    }

    public void DeselectTowerBuilUIObject()
    {
        towerBuildUI.Hide();
    }

    private void DeSelectHeroes()
    {
        for (int i = 0; i < heroes.Count; i++)
        {
            heroes[i].Deselect();
        }
    }
}
