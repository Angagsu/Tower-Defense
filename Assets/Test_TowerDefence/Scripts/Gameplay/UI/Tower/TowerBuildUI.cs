using UnityEngine;
using System.Collections.Generic;


public class TowerBuildUI : MonoBehaviour
{
    [SerializeField] private GameObject gameObjectRoot;
    [SerializeField] private BuildsController buildsController;
    [SerializeField] private TowerBuildInformPopupUI popupUI;
    [SerializeField] private TowerAttackRangeView attackRangeView;
    [SerializeField] private GameObject attackRangeViewObj;

    [Header("Acquired Selected Towers To Build")]
    [Space(5)]
    [SerializeField] private List<AcquiredSelectedTowerUI> towersToBuild;

    [Header("Tower Build Panel Position Offset")]
    [SerializeField] private Vector3 positionOffset;


    private List<TowerFullBlueprintSO> acquiredSelectedTowers = new();
    private TowersDataStoreService towersDataStoreService;
    private AcquiredSelectedTowerUI currentSelectedTower;
    private BuildingArea selectedBuildingArea;

    private Vector3 startPosition;


    [Inject]
    public void Construct(TowersDataStoreService towersDataStoreService)
    {
        this.towersDataStoreService = towersDataStoreService;

        for (int i = 0; i < towersDataStoreService.SelectedAcquiredTowerByIndex.Count; i++)
        {
            acquiredSelectedTowers.Add(towersDataStoreService.AllTowers.AcquiredTowers[towersDataStoreService.SelectedAcquiredTowerByIndex[i]]);
            towersToBuild[i].SetTowerBuildUI(this);
            towersToBuild[i].SetParams(acquiredSelectedTowers[i], i);
        }
    }


    private void Start()
    {
        startPosition = transform.position;
    }

    public void SetCurrentSelectedTower(AcquiredSelectedTowerUI currentSelectedTower)
    {
        if (this.currentSelectedTower != null && this.currentSelectedTower == currentSelectedTower)
        {
            buildsController.BuildTowerByType(currentSelectedTower.TowerFullBlueprintSO, currentSelectedTower.TowerSO);
            popupUI.DisablePopup();
            attackRangeViewObj.gameObject.SetActive(false);
        }
        else if (this.currentSelectedTower != null && this.currentSelectedTower != currentSelectedTower)
        {
            this.currentSelectedTower.UnSelect();
            this.currentSelectedTower = currentSelectedTower;
            popupUI.EnablePopup(currentSelectedTower.TowerSO);
            attackRangeViewObj.SetActive(true);
            attackRangeView.SetAttackRange(currentSelectedTower.TowerSO.CurrentUpgradeLevel.AttackRange);
        }
        else if (this.currentSelectedTower == null)
        {
            this.currentSelectedTower = currentSelectedTower;
            popupUI.EnablePopup(currentSelectedTower.TowerSO);
            attackRangeViewObj.SetActive(true);
            attackRangeView.SetAttackRange(currentSelectedTower.TowerSO.CurrentUpgradeLevel.AttackRange);
        }  
    }

    public void SetTargetGroundForBuilding(BuildingArea selectedBuildingArea)
    {
        Hide();
        this.selectedBuildingArea = selectedBuildingArea;
        transform.position = selectedBuildingArea.gameObject.transform.position + positionOffset;
        gameObjectRoot.SetActive(true);
    }

    public void Hide()
    {
        if (currentSelectedTower)
        {
            currentSelectedTower.UnSelect();
            currentSelectedTower = null;
            popupUI.DisablePopup();
        }
        
        gameObjectRoot.SetActive(false);
        transform.position = startPosition;
        attackRangeViewObj.SetActive(false);
    }

    public bool IsGameObjectActive()
    {
        return gameObjectRoot.activeSelf;
    }
}
