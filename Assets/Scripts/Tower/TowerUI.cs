using UnityEngine.UI;
using UnityEngine;

public class TowerUI : MonoBehaviour
{
    private GroundBehavior selectedGround;
    
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
        else
        {
            sellCostText.text = selectedGround.towerBlueprint.GetUpgradedTowerSellCost().ToString();
            upgradeCostText.text = "";
            upgradeButton.interactable = false;
        }
    }

    public void HideCanvas()
    {
        uI.SetActive(false);
    }

    public void UpgradeButton()
    {
        selectedGround.UpgradeTower();
        TowerBuildManager.Instance.DeselectGround();
    }

    public void SellButton()
    {
        selectedGround.SellTower();
        TowerBuildManager.Instance.DeselectGround();
    }
}
