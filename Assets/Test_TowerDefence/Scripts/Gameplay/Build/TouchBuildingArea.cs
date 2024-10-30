using UnityEngine;
using UnityEngine.EventSystems;

public class TouchBuildingArea : MonoBehaviour
{
    public BuildingArea BuildingArea { get; private set; }

    [SerializeField] private BuildsController buildsController;

    private int buildingAreaLayer;
    private int touchID;

    private PlayerInputHandler playerInputHandler;

    private void Start()
    {
        playerInputHandler = PlayerInputHandler.Instance;

        buildingAreaLayer = LayerMask.NameToLayer("BuildingArea");
        touchID = playerInputHandler.TapInput;



    }

    private void Update()
    {
        SelectBuildingArea();
    }
    private void SelectBuildingArea()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (playerInputHandler.TapInput > 0)
        {
            if (!IsMouseOverUI())
            {
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
        }
    }

    private bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject(touchID);
    }
}
