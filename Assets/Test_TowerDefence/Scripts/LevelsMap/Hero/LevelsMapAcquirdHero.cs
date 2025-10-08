using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelsMapAcquirdHero : MonoBehaviour
{
    public HeroSO HeroSO { get => heroSO; }
    [field: SerializeField] public HeroData HeroData { get; private set; }

    [SerializeField] private Button button;
    [SerializeField] private Image heroImage;
    [SerializeField] private Image selectImage;

    private PreparingToPlayUI preparingToPlayUI;

    private HeroSO heroSO;
    private int iD;

    private void Awake()
    {
        button.onClick.AddListener(OnClick);
    }


    public void SetHeroesDataHandler(PreparingToPlayUI preparingToPlayUI)
    {
        this.preparingToPlayUI = preparingToPlayUI;
    }

    public void OnHeroDataChange(HeroData heroData)
    {
        HeroData.UpgradeLevel = heroData.UpgradeLevel;
    }

    public void SetParams(HeroSO heroSO, int iD)
    {
        this.heroSO = heroSO;
        this.iD = iD;

        HeroData = heroSO.HeroData;
        heroImage.sprite = heroSO.Sprite;

        UnSelect();
    }

    private void OnClick()
    {
        //Select();
        preparingToPlayUI.SetAcquiredSelectedHero(this, iD);
    }

    public void Select() => selectImage.gameObject.SetActive(true);
    
    public void UnSelect() => selectImage.gameObject.SetActive(false);
}
