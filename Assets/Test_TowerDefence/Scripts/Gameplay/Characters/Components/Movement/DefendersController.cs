using Assets.Scripts.Tower;
using System;
using UnityEngine;



public class DefendersController : BaseMovement
{
    public Action<Vector3> DefendersPositionChanged;

    [SerializeField] private BaseTower baseTower;
    [SerializeField] private DefenderUnit[] defenderUnits;
    [SerializeField] private Transform towerTransform;
    [SerializeField] private float defendersSpeed;
    [SerializeField] private float timeOfRevive;
    [SerializeField] private float turnSpeed;

    private PlayerInputHandler playerInputHandler;
    private TouchBuildingArea touchBuildingArea;
    private TowerUpgradeUI towerUpgradeUI;
    private DetectionHelper detectionHelper;

    private TowerUpgradeLevelSO upgradeLevel;
    private Camera mainCamera;
    private Vector3 defendersNewPoint;
    private Vector3 defendersStartPoint;
    private float movementRandiusRange;
    private int groundLayer;
    private bool isSelected;

    LayerMask unsafeAreaLayerMask;

    public void Construct(PlayerInputHandler playerInputHandler, GameplayStates gameplayStates, TowerUpgradeUI towerUpgradeUI, TouchBuildingArea touchBuildingArea, ProjectilesFactoriesService projectilesFactoriesService, TowerSO towerSO)
    {
        this.playerInputHandler = playerInputHandler;
        this.towerUpgradeUI = towerUpgradeUI;
        this.touchBuildingArea = touchBuildingArea;
        movementRandiusRange = towerSO.CurrentUpgradeLevel.AttackRange;
        for (int i = 0; i < defenderUnits.Length; i++)
        {
            defenderUnits[i].Construct(playerInputHandler, gameplayStates, projectilesFactoriesService);
            defenderUnits[i].SetDefenderParams(towerSO);
        }

        this.playerInputHandler.TouchPressed += GetDefendersNewPosition;
    }

    private void Awake()
    {
        mainCamera = Camera.main;

        groundLayer = LayerMask.NameToLayer("DefendersMoveZone");
        unsafeAreaLayerMask = LayerMask.GetMask("UnsafeArea");
        detectionHelper = DetectionHelper.Instance;

        SetTheSworderDefenderArray();

        baseTower.TowerCrashed += OnTowerCrashed;
        baseTower.TowerRepaired += OnTowerRepaired;
    }

    private void OnTowerRepaired()
    {
        for (int i = 0; i < defenderUnits.Length; i++)
        {
            defenderUnits[i].OnTowerRepair();
            //DefendersPositionChanged?.Invoke(defendersStartPoint);
        }
    }

    private void OnTowerCrashed()
    {
        for (int i = 0; i < defenderUnits.Length; i++)
        {
            defenderUnits[i].OnTowerCrush(defendersStartPoint);
        }
    }

    void Start()
    {
        isMoves = true;
        isSelected = false;

        defendersStartPoint = touchBuildingArea.BuildingArea.GetDefendersStartPoint().position;
        
        DefendersPositionChanged?.Invoke(defendersStartPoint);
    }

    private void OnDisable()
    {
        playerInputHandler.TouchPressed -= GetDefendersNewPosition;
    }

    private void GetDefendersNewPosition(Vector2 touchPosition)
    {
        if (isSelected)
        {
            Ray ray = mainCamera.ScreenPointToRay(touchPosition);

            if (Physics.Raycast(ray, out RaycastHit unsafeAreaHit, 1000f, unsafeAreaLayerMask))
            {
                isSelected = false;
                isMoves = false;
                return;
            }

            if (Physics.Raycast(ray, out RaycastHit raycastHit) && Vector3.Distance(raycastHit.point, transform.position) <= movementRandiusRange)
            {
                isSelected = false;

                DefendersPositionChanged?.Invoke(raycastHit.point);

                defendersNewPoint = raycastHit.point;
            }
            else
            {
                isSelected = false;
                isMoves = false;
            }
        }
    }

    private void SetTheSworderDefenderArray()
    {
        for (int i = 0; i < defenderUnits.Length; i++)
        {
            detectionHelper.OnHeroesCountIncreased(defenderUnits[i]);
        }
    }

    public void SelectOrDeSelectDefenders(bool tof)
    {
        isSelected = tof;
    }

    public DefenderUnit[] GetDefendersArray()
    {
        return defenderUnits;
    }
}


