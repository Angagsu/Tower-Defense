using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerUpgradeButtonUI : MonoBehaviour
{
    [SerializeField] private TowerUpgradeUI towerUpgradeUI;
    [SerializeField] private Button button;
    [SerializeField] private Button lockButton;
    [SerializeField] private Image selectImage;
    [SerializeField] private Image lockImage;
    [SerializeField] private Image upgradeCostImage;
    [SerializeField] private TextMeshProUGUI upgradeCostText;

    public bool IsSelected { get; private set; } = false;
    public int CurrentTowerSOIndex;
    private TowerSO nextUpgradeTowerSO;



    private void Awake()
    {
        button.onClick.AddListener(OnButtonClick);
        lockButton.onClick.AddListener(towerUpgradeUI.Hide);
    }

    public TowerSO SetParams(BuildingArea selectedBuildingArea)
    {
        if (selectedBuildingArea.CurrentTowerSO == selectedBuildingArea.TowerFullBlueprintSO.TowerUpgrades[0])
        {
            CurrentTowerSOIndex = 0;
            nextUpgradeTowerSO = selectedBuildingArea.TowerFullBlueprintSO.TowerUpgrades[1];

            if (nextUpgradeTowerSO.TowerData.IsAcquired)
            {
                LockUpgradeButton(false);
                upgradeCostText.text = nextUpgradeTowerSO.Cost.ToString();
            }
            else
            {
                LockUpgradeButton(true);
            }
        }
        else if (selectedBuildingArea.CurrentTowerSO == selectedBuildingArea.TowerFullBlueprintSO.TowerUpgrades[1])
        {
            CurrentTowerSOIndex = 1;
            nextUpgradeTowerSO = selectedBuildingArea.TowerFullBlueprintSO.TowerUpgrades[2];

            if (nextUpgradeTowerSO.TowerData.IsAcquired)
            {
                LockUpgradeButton(false);
                upgradeCostText.text = nextUpgradeTowerSO.Cost.ToString();
            }
            else
            {
                LockUpgradeButton(true);
            }
        }
        else if (selectedBuildingArea.CurrentTowerSO == selectedBuildingArea.TowerFullBlueprintSO.TowerUpgrades[2])
        {
            CurrentTowerSOIndex = 2;
            nextUpgradeTowerSO = selectedBuildingArea.TowerFullBlueprintSO.TowerUpgrades[3];

            if (nextUpgradeTowerSO.TowerData.IsAcquired)
            {
                LockUpgradeButton(false);
                upgradeCostText.text = nextUpgradeTowerSO.Cost.ToString();
            }
            else
            {
                LockUpgradeButton(true);
            }
        }
        else if (selectedBuildingArea.CurrentTowerSO == selectedBuildingArea.TowerFullBlueprintSO.TowerUpgrades[3])
        {
            CurrentTowerSOIndex = 3;
            nextUpgradeTowerSO = selectedBuildingArea.TowerFullBlueprintSO.TowerUpgrades[2];

            if (nextUpgradeTowerSO.TowerData.IsAcquired)
            {
                LockUpgradeButton(true);
                upgradeCostText.text = "";
            }
            else
            {
                LockUpgradeButton(true);
            }
        }

        return nextUpgradeTowerSO;
    }

    public void LockUpgradeButton(bool tof)
    {
        lockImage.gameObject.SetActive(tof);
    }

    public void Unselect()
    {
        IsSelected = false;
        selectImage.gameObject.SetActive(IsSelected);
    }

    private void Select()
    {
        IsSelected = true;
        selectImage.gameObject.SetActive(IsSelected);
    }

    private void OnButtonClick()
    {
        if (IsSelected)
        {
            LockUpgradeButton(false);
            Unselect();
        }
        else
        {
            Select();
        }

        towerUpgradeUI.OnUpgradeButtonClick();
    }
}
