using UnityEngine;


public class TouchBuildingArea : MonoBehaviour
{
    public BuildingArea BuildingArea { get; private set; }

    [SerializeField] private BuildsController buildsController;

    private int buildingAreaLayer;

    private PlayerInputHandler playerInputHandler;


    [Inject]
    public void Construct(PlayerInputHandler playerInputHandler)
    {
        this.playerInputHandler = playerInputHandler;
        playerInputHandler.TouchPressed += OnTouchBuildingArea;
    }
    private void Start()
    {
        buildingAreaLayer = LayerMask.NameToLayer("BuildingArea");
    }

    private void OnTouchBuildingArea(Vector2 touchPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);

        if (Physics.Raycast(ray, out RaycastHit raycastHit) && raycastHit.collider.gameObject.layer.CompareTo(buildingAreaLayer) == 0)
        {
            raycastHit.collider.gameObject.TryGetComponent<BuildingArea>(out var buildingArea);

            BuildingArea = buildingArea;

            if (buildingArea.Tower != null)
            {
                buildsController.SelectedGroundForUpgradeTowerUI(buildingArea);
                return;
            }

            if (buildingArea.Tower == null)
            {
                buildsController.SelectedGroundForBuildTowerUI(buildingArea);
                return;
            }
        }
    }

    private void OnDisable()
    {
        playerInputHandler.TouchPressed -= OnTouchBuildingArea;
    }
}
