using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelsMapHero : MonoBehaviour
{
    public HeroSO HeroSO { get => heroSO; }
    [field: SerializeField] public HeroData HeroData {  get; private set; }
    
    [SerializeField] private Button button;
    [SerializeField] private Image heroImage;
    [SerializeField] private Image selectImage;
    [SerializeField] private Image lockImage;
    [SerializeField] private Image deSelectImage;

   // [SerializeField] private GameObject priceObj;
    [SerializeField] TextMeshProUGUI priceText;

    private HeroesDataHandler heroesDataHandler;
    
    private HeroSO heroSO;
    private bool isAcquired;
    private int ID;

    private void Awake()
    {
        button.onClick.AddListener(OnClick);
    }

    
    public void SetHeroesDataHandler(HeroesDataHandler heroesDataHandler)
    {
        this.heroesDataHandler = heroesDataHandler;
    }

    public void OnHeroDataChange(HeroData heroData)
    {
        HeroData.ReseivedCards = heroData.ReseivedCards;
        HeroData.UpgradeLevel = heroData.UpgradeLevel;
        isAcquired = HeroData.IsAcquired;
        priceText.text = HeroData.Price.ToString();

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

    public void SetParams(HeroSO heroSO, int ID)
    { 
        this.heroSO = heroSO;
        this.ID = ID;

        HeroData = heroSO.HeroData;
        heroImage.sprite = heroSO.Sprite;
        isAcquired = heroSO.HeroData.IsAcquired;
        priceText.text = heroSO.HeroData.Price.ToString();
        UnSelect();

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

    private void OnClick()
    {
        Select();
        heroesDataHandler.SetCurrentSelectedHero(this, ID);
    }

    public void Select()
    {
        selectImage.gameObject.SetActive(true);
        deSelectImage.gameObject.SetActive(false);
    }

    public void UnSelect() 
    {
        selectImage.gameObject.SetActive(false);
        deSelectImage.gameObject.SetActive(true);
    }
}
