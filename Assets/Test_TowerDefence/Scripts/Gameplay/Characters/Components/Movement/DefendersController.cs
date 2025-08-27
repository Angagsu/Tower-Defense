using System;
using UnityEngine;



public class DefendersController : BaseMovement
{
    public Action<Vector3> DefendersPositionChanged;

    [SerializeField] private DefenderUnit[] defenderUnits;
    [SerializeField] private Transform towerTransform;
    [SerializeField] private float defendersSpeed;
    [SerializeField] private float timeOfRevive;
    [SerializeField] private float turnSpeed;

    private PlayerInputHandler playerInputHandler;
    private TouchBuildingArea touchBuildingArea;
    private TowerUpgradeUI towerUpgradeUI;
    private DetectionHelper detectionHelper;
    
    private Camera mainCamera;
    private Vector3 defendersNewPoint;
    private int groundLayer;
    private bool isSelected;

    

    public void Construct(PlayerInputHandler playerInputHandler, GameplayStates gameplayStates, TowerUpgradeUI towerUpgradeUI, TouchBuildingArea touchBuildingArea, ProjectilesFactoriesService projectilesFactoriesService)
    {
        this.playerInputHandler = playerInputHandler;
        this.towerUpgradeUI = towerUpgradeUI;
        this.touchBuildingArea = touchBuildingArea;

        for (int i = 0; i < defenderUnits.Length; i++)
        {
            defenderUnits[i].Construct(playerInputHandler, gameplayStates, projectilesFactoriesService);
        }

        this.playerInputHandler.TouchPressed += GetDefendersNewPosition;
    }

    private void Awake()
    {
        mainCamera = Camera.main;

        groundLayer = LayerMask.NameToLayer("DefendersMoveZone");
        detectionHelper = DetectionHelper.Instance;

        SetTheSworderDefenderArray();
    }

    void Start()
    {
        isMoves = true;
        isSelected = false;

        defendersNewPoint = touchBuildingArea.BuildingArea.GetDefendersStartPoint().position;
        
        DefendersPositionChanged?.Invoke(defendersNewPoint);
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

            if (Physics.Raycast(ray, out RaycastHit raycastHit) &&
                raycastHit.collider.gameObject.layer.CompareTo(groundLayer) == 0)
            {
                isSelected = false;

                DefendersPositionChanged?.Invoke(raycastHit.point);
                towerUpgradeUI.DeActivateDefendersMoveZone();

                defendersNewPoint = raycastHit.point;
            }
            else
            {
                isSelected = false;
                isMoves = false;
                towerUpgradeUI.DeActivateDefendersMoveZone();
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


