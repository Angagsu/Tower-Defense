using UnityEngine.UI;
using UnityEngine;

public class TowerUI : MonoBehaviour
{
    private GroundBehavior selectedGround;
    public TowerBuildManager towerBuildManager;
    [SerializeField] private GameObject uI;
    [SerializeField] private Text upgradeCostText, sellCostText;
    [SerializeField] private Button upgradeButton;

    
    public void SetTargetGround(GroundBehavior selectedGround)
    {
        this.selectedGround = selectedGround;
        transform.position = selectedGround.GetBuildPosition();
        uI.SetActive(true);

        if (!selectedGround.IsUpgraded)
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
            upgradeCostText.text = " ";
            upgradeButton.interactable = false;
        }
    }

    public void HideCanvas()
    {
        uI.SetActive(false);
    }

    public void UpgradeButton()
    {
        if (selectedGround.IsUpgraded)
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
