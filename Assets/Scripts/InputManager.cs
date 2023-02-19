using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private TowerOnBuy towerOnBuy;
    private TowerBuildManager towerBuildManager;
    private int buildingAreaLayer;

    int touchID;
    private void Start()
    {
        buildingAreaLayer = LayerMask.NameToLayer("BuildingArea");
        towerBuildManager = TowerBuildManager.Instance;
#if UNITY_EDITOR
        touchID = Input.GetMouseButtonDown(0).CompareTo(true);

#elif UNITY_ANDROID
        touchID = Input.GetTouch(0).fingerId;
#endif

    }

    private void Update()
    {
        SelectBuildingArea();
    }
    private void SelectBuildingArea()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began || Input.GetMouseButtonDown(0))
        {
            if (!IsMouseOverUI())
            {
                if (Physics.Raycast(ray, out RaycastHit raycastHit) && raycastHit.collider.gameObject.layer.CompareTo(buildingAreaLayer) == 0)
                {
                    GroundBehavior groundBehavior = raycastHit.collider.gameObject.GetComponent<GroundBehavior>();

                    if (groundBehavior.tower != null)
                    {
                        towerBuildManager.SelectedGroundForUpgradeTowerUI(groundBehavior);
                        return;
                    }

                    if (groundBehavior.tower == null)
                    {
                        towerBuildManager.SelectedGroundForBuildTowerUI(groundBehavior);
                        towerOnBuy.SetGroundForBuilding(groundBehavior);

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
