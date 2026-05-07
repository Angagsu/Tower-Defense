using UnityEngine;
using UnityEngine.UI;

public class SelectedTowerUI : MonoBehaviour
{
    public TowerSO TowerSO { get => towerSO; }
    public int ID => iD;


    [SerializeField] private Button button;
    [SerializeField] private Image towerImage;
    [SerializeField] private Image selectImage;
    [SerializeField] private int iD;


    private PreparingToPlayUI preparingToPlayUI;
    private TowerSO towerSO;


    private void Awake()
    {
        button.onClick.AddListener(OnClick);
        selectImage.gameObject.SetActive(false);
    }


    public void Construct(PreparingToPlayUI preparingToPlayUI)
    {
        this.preparingToPlayUI = preparingToPlayUI;
    }


    public void SetParams(TowerSO towerSO)
    {
        this.towerSO = towerSO;
        towerImage.sprite = towerSO.TowerSprite;
        //UnSelect();
    }

    private void OnClick()
    {
        Select();
        preparingToPlayUI.SetCurrentSelectedTowerUI(this);
    }

    public void Select()
    {
        selectImage.gameObject.SetActive(true);
        towerImage.sprite = null;
    }

    public void UnSelect()
    {
        selectImage.gameObject.SetActive(false);
        towerImage.sprite = towerSO.TowerSprite;
    }
}
