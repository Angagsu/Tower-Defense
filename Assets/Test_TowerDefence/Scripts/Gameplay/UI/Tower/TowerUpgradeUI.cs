using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class TowerUpgradeUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI upgradeCostText, sellCostText;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private GameObject defendersMoveZone;
    [SerializeField] private GameObject selectDefendersButton;
    [SerializeField] private GameObject gameObjectRoot;
    [SerializeField] private BuildsController buildsController;
    [SerializeField] private Vector3 positionOffset;


    private BuildingArea selectedBuildingArea;


    private void Awake()
    {
        DeActivateDefendersMoveZone();
    }

    public void SetTargetGround(BuildingArea selectedBuildingArea)
    {
        this.selectedBuildingArea = selectedBuildingArea;
        transform.position = selectedBuildingArea.gameObject.transform.position;
        gameObjectRoot.transform.position = selectedBuildingArea.gameObject.transform.position + positionOffset;
        
        gameObjectRoot.SetActive(true);


        if (!selectedBuildingArea.IsUpgraded || !selectedBuildingArea.IsUpgradedSecondTime || !selectedBuildingArea.IsUpgradedThirdTime)
        {
            sellCostText.text = selectedBuildingArea.TowerFullBlueprintSO.GetTowerSellCost().ToString();
            upgradeCostText.text = "" + selectedBuildingArea.TowerFullBlueprintSO.Towers[1].Cost;
            upgradeButton.interactable = true;
        }
        if (selectedBuildingArea.IsUpgraded)
        {
            sellCostText.text = selectedBuildingArea.TowerFullBlueprintSO.GetUpgradedTowerSellCost().ToString();
            upgradeCostText.text = "" + selectedBuildingArea.TowerFullBlueprintSO.Towers[2].Cost;
            upgradeButton.interactable = true;
        }
        if (selectedBuildingArea.IsUpgradedSecondTime)
        {
            sellCostText.text = selectedBuildingArea.TowerFullBlueprintSO.GetSecondTimeUpgradedTowerSellCost().ToString();
            upgradeCostText.text = "" + selectedBuildingArea.TowerFullBlueprintSO.Towers[3].Cost;
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
