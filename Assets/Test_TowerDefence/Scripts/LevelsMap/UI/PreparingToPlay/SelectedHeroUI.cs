using UnityEngine;
using UnityEngine.UI;

public class SelectedHeroUI : MonoBehaviour
{
    public HeroSO HeroSO { get => heroSO; }
    public int ID => iD;

    [SerializeField] private Button button;
    [SerializeField] private Image heroImage;
    [SerializeField] private Image selectImage;
    [SerializeField] private int iD;


    private PreparingToPlayUI preparingToPlayUI;
    private HeroSO heroSO;
    

    private void Awake()
    {
        button.onClick.AddListener(OnClick);
        selectImage.gameObject.SetActive(false);
    }


    public void Construct(PreparingToPlayUI preparingToPlayUI)
    {
        this.preparingToPlayUI = preparingToPlayUI;
    }


    public void SetParams(HeroSO heroSO)
    {
        this.heroSO = heroSO;
        heroImage.sprite = heroSO.Sprite;
        //UnSelect();
    }

    private void OnClick()
    {
        Select();  
        preparingToPlayUI.SetCurrentSelectedHeroUI(this);
    }

    public void Select()
    {
        selectImage.gameObject.SetActive(true);
        heroImage.sprite = null;
    }

    public void UnSelect()
    {
        selectImage.gameObject.SetActive(false);
        heroImage.sprite = heroSO.Sprite;
    }
}
