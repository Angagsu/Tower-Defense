using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerSellButtonUI : MonoBehaviour
{
    [SerializeField] private TowerUpgradeUI towerUpgradeUI;
    [SerializeField] private Button button;
    [SerializeField] private Image selectImage;
    [SerializeField] private TextMeshProUGUI sellCostText;

    public bool IsSelected { get; private set; }



    private void Awake()
    {
        button.onClick.AddListener(OnClick);
    }

    public void SetParams(BuildingArea selectedBuildingArea, int currentTowerSOIndex)
    {
        if (selectedBuildingArea.CurrentTower.IsCrashed)
        {
            sellCostText.text = "0";
        }
        else
        {
            sellCostText.text = selectedBuildingArea.TowerFullBlueprintSO.GetTowerSellCostByIndex(currentTowerSOIndex).ToString();
        }        
    }

    public void Unselect()
    {
        IsSelected = false;
        selectImage.gameObject.SetActive(false);
    }

    private void Select()
    {
        IsSelected = true;
        selectImage.gameObject.SetActive(IsSelected);
    }

    private void OnClick()
    {
        if (IsSelected)
        {
            Unselect();
        }
        else
        {
            Select();
        }

        towerUpgradeUI.OnSellButtonClick();
    }
}
