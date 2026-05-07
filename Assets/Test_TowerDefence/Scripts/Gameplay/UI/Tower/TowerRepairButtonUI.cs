using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerRepairButtonUI : MonoBehaviour
{
    [SerializeField] private TowerUpgradeUI towerUpgradeUI;
    [SerializeField] private Button button;
    [SerializeField] private Image selectImage;
    [SerializeField] private TextMeshProUGUI repairCostText;

    public bool IsSelected { get; private set; }



    private void Awake()
    {
        button.onClick.AddListener(OnClick);
    }

    public void SetParams(BuildingArea selectedBuildingArea)
    {
        if (selectedBuildingArea.CurrentTower.IsDamaged)
        {
            repairCostText.text = selectedBuildingArea.CurrentTower.GetRepairCost().ToString();
        }
        else
        {
            repairCostText.text = "";
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

        towerUpgradeUI.OnRepairButtonClick();
    }
}
