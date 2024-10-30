using UnityEngine.UI;
using UnityEngine;

public class TowerUpgradeUI : MonoBehaviour
{
    [SerializeField] private Text upgradeCostText, sellCostText;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private GameObject defendersMoveZone;
    [SerializeField] private GameObject selectDefendersButton;
    [SerializeField] private GameObject gameObjectRoot;
    [SerializeField] private BuildsController buildsController;

    private BuildingArea selectedBuildingArea;


    private void Awake()
    {
        DeActivateDefendersMoveZone();
    }

    public void SetTargetGround(BuildingArea selectedBuildingArea)
    {
        this.selectedBuildingArea = selectedBuildingArea;
        transform.position = selectedBuildingArea.GetBuildPosition();
        gameObjectRoot.SetActive(true);

        if (!selectedBuildingArea.IsUpgraded || !selectedBuildingArea.IsUpgradedSecondTime || !selectedBuildingArea.IsUpgradedThirdTime)
        {
            sellCostText.text = selectedBuildingArea.TowerFullBlueprintSO.GetTowerSellCost().ToString();
            upgradeCostText.text = "$ " + selectedBuildingArea.TowerFullBlueprintSO.UpgradedTower.Cost;
            upgradeButton.interactable = true;
        }
        if (selectedBuildingArea.IsUpgraded)
        {
            sellCostText.text = selectedBuildingArea.TowerFullBlueprintSO.GetUpgradedTowerSellCost().ToString();
            upgradeCostText.text = "$ " + selectedBuildingArea.TowerFullBlueprintSO.SecondTimeUpgradedTower.Cost;
            upgradeButton.interactable = true;
        }
        if (selectedBuildingArea.IsUpgradedSecondTime)
        {
            sellCostText.text = selectedBuildingArea.TowerFullBlueprintSO.GetSecondTimeUpgradedTowerSellCost().ToString();
            upgradeCostText.text = "$ " + selectedBuildingArea.TowerFullBlueprintSO.ThirdTimeUpgradedTower.Cost;
            upgradeButton.interactable = true;
        }
        if (selectedBuildingArea.IsUpgradedThirdTime)
        {
            sellCostText.text = selectedBuildingArea.TowerFullBlueprintSO.GetThirdTimeUpgradedTowerSellCost().ToString();
            upgradeCostText.text = " ";
            upgradeButton.interactable = false;
        }
    }

    public void UpgradeButton()
    {
        if (selectedBuildingArea.IsUpgradedSecondTime && !selectedBuildingArea.IsUpgraded && !selectedBuildingArea.IsUpgradedThirdTime)
        {
            buildsController.UpdateThirdTime();
        }
        else if (selectedBuildingArea.IsUpgraded && !selectedBuildingArea.IsUpgradedThirdTime)
        {
            buildsController.UpdateSecondTime();
        }
        else
        {
            buildsController.UpgradeTower();
        }

        buildsController.DeselectGround();
    }

    public void SellButton()
    {
        buildsController.SellTower();
        buildsController.DeselectGround();
    }

    public void DeActivateDefendersMoveZone()
    {
        defendersMoveZone.gameObject.SetActive(false);
    }

    public void SelectDefendersOnUI()
    {
        selectedBuildingArea.SelectDefenderTowerOnUI();
        defendersMoveZone.gameObject.SetActive(true);
    }

    public void ActivateOrDeactivateSelectDefendersButton(bool tof)
    {
        selectDefendersButton.SetActive(tof);
    }

    public void Hide()
    {
        gameObjectRoot.SetActive(false);
    }

    public bool IsGameObjectActive()
    {
        return gameObjectRoot.activeSelf;
    }
}
