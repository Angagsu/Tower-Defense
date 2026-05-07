using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AcquiredSelectedTowerUI : MonoBehaviour
{
    public TowerFullBlueprintSO TowerFullBlueprintSO { get => towerFullBlueprintSO; }
    public TowerSO TowerSO { get => towerSO; }
    [field: SerializeField] public TowerData TowerData { get; private set; }

    [SerializeField] private Button button;
    [SerializeField] private Image towerImage;
    [SerializeField] private Image selectImage;
    [SerializeField] private Image lockImage;

    [SerializeField] private GameObject priceObj;
    [SerializeField] TextMeshProUGUI priceText;

    private TowerBuildUI towerBuildUI;

    private TowerFullBlueprintSO towerFullBlueprintSO;
    private TowerSO towerSO;
    private bool isAcquired;
    public int ID;



    private void Awake()
    {
        button.onClick.AddListener(OnClick);
    }

    public void SetTowerBuildUI(TowerBuildUI towerBuildUI)
    {
        this.towerBuildUI = towerBuildUI;
    }

    public void SetParams(TowerFullBlueprintSO towerFullBlueprintSO, int ID)
    {
        this.towerFullBlueprintSO = towerFullBlueprintSO;
        this.towerSO = towerFullBlueprintSO.TowerUpgrades[0];
        this.ID = ID;

        TowerData = towerSO.TowerData;
        towerImage.sprite = towerSO.TowerSprite;
        isAcquired = towerSO.TowerData.IsAcquired;
        priceText.text = towerSO.Cost.ToString();
        UnSelect();

        if (isAcquired)
        {
            lockImage.gameObject.SetActive(false);
        }
    }

    private void OnClick()
    {
        Select();
        towerBuildUI.SetCurrentSelectedTower(this);
    }

    public void Select() => selectImage.gameObject.SetActive(true);

    public void UnSelect() => selectImage.gameObject.SetActive(false);
}
