using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelsMapTowerUpgrade : MonoBehaviour
{
    public TowerSO TowerSO { get => towerSO; }
    [field: SerializeField] public TowerData TowerData { get; private set; }

    [SerializeField] private Button button;
    [SerializeField] private Image towerImage;
    [SerializeField] private Image selectImage;
    [SerializeField] private Image lockImage;

    //[SerializeField] private GameObject priceObj;
    //[SerializeField] TextMeshProUGUI priceText;

    private TowersUpgradeUI towersUpgradeUI;

    private TowerSO towerSO;
    private bool isAcquired;
    public int ID;

    private void Awake()
    {
        button.onClick.AddListener(OnClick);
    }


    public void SetTowersUpgradeUI(TowersUpgradeUI towersUpgradeUI)
    {
        this.towersUpgradeUI = towersUpgradeUI;
    }

    public void OnTowerDataChange(TowerData towerData)
    {
        TowerData.ReceivedCards = towerData.ReceivedCards;
        TowerData.UpgradeLevel = towerData.UpgradeLevel;
        isAcquired = TowerData.IsAcquired;
        //priceText.text = TowerData.Price.ToString();
    
        if (isAcquired)
        {
            //priceObj.SetActive(false);
            lockImage.gameObject.SetActive(false);
        }
        else
        {
            lockImage.gameObject.SetActive(true);
        }
    }

    public void SetParams(TowerSO TowerSO, int ID)
    {
        this.towerSO = TowerSO;
        this.ID = ID;

        TowerData = TowerSO.TowerData;
        towerImage.sprite = TowerSO.TowerSprite;
        isAcquired = TowerSO.TowerData.IsAcquired;
        //priceText.text = TowerSO.TowerData.Price.ToString();
        UnSelect();

        if (isAcquired)
        {
            //priceObj.SetActive(false);
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
        towersUpgradeUI.SetSelectedTowerUpgradeStats(this);
    }

    public void Select() => selectImage.gameObject.SetActive(true);

    public void UnSelect() => selectImage.gameObject.SetActive(false);
}
