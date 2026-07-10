using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class LevelsMapTower : MonoBehaviour
{
    public TowerFullBlueprintSO TowerFullBlueprintSO { get => towerFullBlueprintSO; }
    public TowerSO TowerSO { get => towerSO; }
    [field: SerializeField] public TowerData TowerData { get; private set; }

    [SerializeField] private Button button;
    [SerializeField] private Image towerImage;
    [SerializeField] private Image selectImage;
    [SerializeField] private Image lockImage;

   // [SerializeField] private GameObject priceObj;
    [SerializeField] TextMeshProUGUI priceText;

    private TowersDataHandler towersDataHandler;

    private TowerFullBlueprintSO towerFullBlueprintSO;
    private TowerSO towerSO;
    private bool isAcquired;
    public int ID;

    private void Awake()
    {
        button.onClick.AddListener(OnClick);
    }


    public void SetTowersDataHandler(TowersDataHandler heroesDataHandler)
    {
        this.towersDataHandler = heroesDataHandler;
    }

    public void OnTowerDataChange(TowerData towerData)
    {
        TowerData.ReceivedCards = towerData.ReceivedCards;
        TowerData.UpgradeLevel = towerData.UpgradeLevel;
        isAcquired = TowerData.IsAcquired;
        priceText.text = TowerData.Price.ToString();

        if (isAcquired)
        {
            //priceObj.SetActive(false);
            lockImage.gameObject.SetActive(false);
        }
        else
        {
           // priceObj.SetActive(true);
            lockImage.gameObject.SetActive(true);
        }
    }

    public void SetParams(TowerFullBlueprintSO towerFullBlueprintSO, int ID)
    {
        this.towerFullBlueprintSO = towerFullBlueprintSO;
        this.towerSO = towerFullBlueprintSO.TowerUpgrades[0];
        this.ID = ID;

        TowerData = towerSO.TowerData;
        towerImage.sprite = towerSO.TowerSprite;
        isAcquired = towerSO.TowerData.IsAcquired;
        priceText.text = towerSO.TowerData.Price.ToString();
        UnSelect();

        if (isAcquired)
        {
           // priceObj.SetActive(false);
            lockImage.gameObject.SetActive(false);
        }
        else
        {
            //priceObj.SetActive(true);
            lockImage.gameObject.SetActive(true);
        }
    }

    private void OnClick()
    {
        Select();
        towersDataHandler.SetCurrentSelectedTower(this, ID);
    }

    public void Select() => selectImage.gameObject.SetActive(true);

    public void UnSelect() => selectImage.gameObject.SetActive(false);
}
