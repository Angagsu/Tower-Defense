using UnityEngine;


public class TowerUpgradeUI : MonoBehaviour
{
    [SerializeField] private BuildsController buildsController;
    [SerializeField] private TowerUpgradeButtonUI upgradeButtonUI;
    [SerializeField] private TowerSellButtonUI sellButtonUI;
    [SerializeField] private TowerRepairButtonUI repairButtonUI;
    [SerializeField] private TowerUpgradeInformPopupUI upgradeInformPopupUI;

    [Space(10)]
    [SerializeField] private TowerAttackRangeView attackRangeView;
    [SerializeField] private GameObject defendersMoveZone;
    [SerializeField] private GameObject selectDefendersButton;
    [SerializeField] private GameObject gameObjectRoot;
    [SerializeField] private GameObject repairUI;

    [Header("Upgrade Panel Position Offset")]
    [SerializeField] private Vector3 positionOffset;


    private BuildingArea selectedBuildingArea;
    private TowerSO nextUpgradeTowerSO;



    private void Awake()
    {
        DeActivateDefendersMoveZone();
    }

    public void SetTargetGround(TowerFullBlueprintSO towerFullBlueprintSO, BuildingArea selectedBuildingArea)
    {
        this.selectedBuildingArea = selectedBuildingArea;

        Hide();

        transform.position = selectedBuildingArea.gameObject.transform.position;
        gameObjectRoot.transform.position = selectedBuildingArea.gameObject.transform.position + positionOffset;
        
        gameObjectRoot.SetActive(true);

        nextUpgradeTowerSO = upgradeButtonUI.SetParams(selectedBuildingArea);

        sellButtonUI.SetParams(selectedBuildingArea, upgradeButtonUI.CurrentTowerSOIndex);

        defendersMoveZone.SetActive(true);
        attackRangeView.SetAttackRange(selectedBuildingArea.CurrentTowerSO.CurrentUpgradeLevel.AttackRange);

        if (selectedBuildingArea.CurrentTower.IsDamaged)
        {
            repairUI.SetActive(true);
            repairButtonUI.SetParams(selectedBuildingArea);
        }
        else
        {
            repairUI.SetActive(false);
        }
    }

    public void OnUpgradeButtonClick()
    {
        if (!upgradeButtonUI.IsSelected)
        {
            upgradeInformPopupUI.DisablePopup();
            UpgradeTower();   
        }
        else
        {
            upgradeInformPopupUI.EnablePopup(selectedBuildingArea.CurrentTowerSO, nextUpgradeTowerSO);
            defendersMoveZone.SetActive(true);
            attackRangeView.SetAttackRange(nextUpgradeTowerSO.CurrentUpgradeLevel.AttackRange);
        }
    }

    public void UpgradeTower()
    {
        if (selectedBuildingArea.IsUpgradedSecondTime)
        {
            buildsController.UpdateThirdTime();
        }
        else if (selectedBuildingArea.IsUpgraded)
        {
            buildsController.UpdateSecondTime();
        }
        else
        {
            buildsController.UpgradeTower();
        }

        buildsController.DeselectGround();
    }

    public void OnSellButtonClick()
    {
        if(!sellButtonUI.IsSelected)
        {
            SellTower();
        }
    }

    private void SellTower()
    {
        buildsController.SellTower();
        buildsController.DeselectGround();
    }

    public void OnRepairButtonClick()
    {
        if (!repairButtonUI.IsSelected)
        {
            RepairTower();
        }
    }

    private void RepairTower()
    {
        buildsController.Repair();
        Hide();
    }

    public void DeActivateDefendersMoveZone()
    {
        defendersMoveZone.SetActive(false);
    }

    public void SelectDefendersOnUI()
    {
        selectedBuildingArea.SelectDefenderTowerOnUI();
        defendersMoveZone.SetActive(true);
    }

    

    public void ActivateOrDeactivateSelectDefendersButton(bool tof)
    {
        selectDefendersButton.SetActive(tof);
    }

    public void Hide()
    {
        gameObjectRoot.SetActive(false);
        upgradeButtonUI.Unselect();
        sellButtonUI.Unselect();
        repairButtonUI.Unselect();
        upgradeInformPopupUI.DisablePopup();
        defendersMoveZone.SetActive(false);
    }

    public bool IsGameObjectActive()
    {
        return gameObjectRoot.activeSelf;
    }
}
