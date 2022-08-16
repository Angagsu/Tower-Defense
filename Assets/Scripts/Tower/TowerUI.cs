using UnityEngine.UI;
using UnityEngine;

public class TowerUI : MonoBehaviour
{
    private GroundBehavior selectedGround;
    
    [SerializeField] private GameObject towerUpgradeUI, towerBuildUI;
    [SerializeField] private Text upgradeCostText, sellCostText;
    [SerializeField] private Button upgradeButton;

    
    public void SetTargetGround(GroundBehavior selectedGround)
    {
        this.selectedGround = selectedGround;
        transform.position = selectedGround.GetBuildPosition();
        towerUpgradeUI.SetActive(true);

        if (!selectedGround.IsUpgraded || !selectedGround.IsUpgradedSecondTime || !selectedGround.IsUpgradedThirdTime)
        {
            sellCostText.text = selectedGround.towerBlueprint.GetTowerSellCost().ToString();
            upgradeCostText.text = "$ " + selectedGround.towerBlueprint.UpgradeCost;
            upgradeButton.interactable = true;
        }
        if (selectedGround.IsUpgraded)
        {
            sellCostText.text = selectedGround.towerBlueprint.GetUpgradedTowerSellCost().ToString();
            upgradeCostText.text = "$ " + selectedGround.towerBlueprint.SecondTimeUpgradeCost;
            upgradeButton.interactable = true;
        }
        if (selectedGround.IsUpgradedSecondTime)
        {
            sellCostText.text = selectedGround.towerBlueprint.GetSecondTimeUpgradedTowerSellCost().ToString();
            upgradeCostText.text = "$ " + selectedGround.towerBlueprint.ThirdTimeUpgradeCost;
            upgradeButton.interactable = true;
        }
        if (selectedGround.IsUpgradedThirdTime)
        {
            sellCostText.text = selectedGround.towerBlueprint.GetThirdTimeUpgradedTowerSellCost().ToString();
            upgradeCostText.text = " ";
            upgradeButton.interactable = false;
        }
    }

    public void HideCanvas()
    {
        towerUpgradeUI.SetActive(false);
    }

    public void UpgradeButton()
    {
        if (selectedGround.IsUpgradedSecondTime && !selectedGround.IsUpgraded && !selectedGround.IsUpgradedThirdTime)
        {
            selectedGround.ThirdTimeUpgradeTower();
        }
        else if (selectedGround.IsUpgraded && !selectedGround.IsUpgradedThirdTime)
        {
            selectedGround.SecondTimeUpgradeTower();
        }
        else
        {
            selectedGround.UpgradeTower();
        }
        
        TowerBuildManager.Instance.DeselectGround();
    }

    public void SellButton()
    {
        selectedGround.SellTower();
        TowerBuildManager.Instance.DeselectGround();
    }
}
