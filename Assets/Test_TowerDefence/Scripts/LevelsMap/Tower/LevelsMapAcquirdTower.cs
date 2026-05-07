using UnityEngine;
using UnityEngine.UI;

public class LevelsMapAcquirdTower : MonoBehaviour
{
    public TowerSO TowerSO { get => towerSO; }
    [field: SerializeField] public TowerData TowerData { get; private set; }

    [SerializeField] private Button button;
    [SerializeField] private Image TowerImage;
    [SerializeField] private Image selectImage;

    private PreparingToPlayUI preparingToPlayUI;

    private TowerSO towerSO;
    private int iD;

    private void Awake()
    {
        button.onClick.AddListener(OnClick);
    }


    public void SetPreparingToPlay(PreparingToPlayUI preparingToPlayUI)
    {
        this.preparingToPlayUI = preparingToPlayUI;
    }

    public void OnHeroDataChange(TowerData heroData)
    {
        TowerData.UpgradeLevel = heroData.UpgradeLevel;
    }

    public void SetParams(TowerSO heroSO, int iD)
    {
        this.towerSO = heroSO;
        this.iD = iD;

        TowerData = heroSO.TowerData;
        TowerImage.sprite = heroSO.TowerSprite;

        UnSelect();
    }

    private void OnClick()
    {
        //Select();
        preparingToPlayUI.SetAcquiredSelectedTower(this, iD);
    }

    public void Select() => selectImage.gameObject.SetActive(true);

    public void UnSelect() => selectImage.gameObject.SetActive(false);
}
