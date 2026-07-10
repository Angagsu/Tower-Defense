using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class HeroesUpgradeUI : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button buyButton;
    [SerializeField] private Button upgradeButton;
    [Space(10)]
    [SerializeField] private Slider reachedUpgradeLevelBar;
    [SerializeField] private TextMeshProUGUI heroBuyCostText;
    [SerializeField] private TextMeshProUGUI reachedUpgradeLevelText;
    [SerializeField] private TextMeshProUGUI reachedUpgradeLevelProgressText;
    [SerializeField] private TextMeshProUGUI heroNameText;
    [Header("Cards")]
    [SerializeField] private TextMeshProUGUI receivedCardsText;
    [SerializeField] private HeroCardUI cardUI;
    [SerializeField] private HeroCardUI requiredCardToUpgrade;
    [SerializeField] private Image heroImage;
    [Space(10)]
    [SerializeField] private UpgradeButtonUI heroesUpgradeButtonUI;
    [SerializeField] private HeroesDataHandler heroesDataHandler;
    [SerializeField] private HeroStatsHandlerUI heroStatsHandlerUI;


    private List<LevelsMapHero> levelsMapHeros;
    private LevelsMapHero currentSelectedHero;
    private HeroUpgradeLevelSO nextUpgradeLevel;

    private int currentUpgradeGoldCost;
    private int currentUpgradeCardCost;



    private void Awake()
    {
        levelsMapHeros = heroesDataHandler.GetLevelsMapHeroes();

        currentSelectedHero = levelsMapHeros[0];
        SetSelectedHeroStats(currentSelectedHero);

        heroesDataHandler.DataChanged += OnDataChange;

        buyButton.onClick.AddListener(OnBuy);
        upgradeButton.onClick.AddListener(OnUpgrade);
    }

    private void OnDestroy()
    {
        heroesDataHandler.DataChanged -= OnDataChange;
    }

    public void SetSelectedHeroStats(LevelsMapHero currentSelectedHero)
    {
        if (this.currentSelectedHero && this.currentSelectedHero != currentSelectedHero)
        {
            this.currentSelectedHero.UnSelect(); 
        }

        nextUpgradeLevel = null;

        this.currentSelectedHero = currentSelectedHero;
        this.currentSelectedHero.Select();

        SetupCardParams();
        SetupButtons();
        SetupTexts();
        SetupSliders();
    }

    public void OnDataChange()
    {
        SetupCardParams();
        SetupButtons();
        SetupTexts();
        SetupSliders();
    }

    private void SetupCardParams()
    {
        cardUI.SetParams(currentSelectedHero.HeroSO);
        requiredCardToUpgrade.SetCardSprite(currentSelectedHero.HeroSO);

        receivedCardsText.text = currentSelectedHero.HeroSO.HeroData.ReceivedCards.Count.ToString();
    }

    private void SetupTexts()
    {
        heroImage.sprite = currentSelectedHero.HeroSO.BigSprite;

        heroNameText.text = currentSelectedHero.HeroSO.HeroName;
        reachedUpgradeLevelProgressText.text = $"Level  {(int)currentSelectedHero.HeroSO.HeroData.UpgradeLevel} / {currentSelectedHero.HeroSO.UpgradeLevelsSO.HeroUpgradeLevelSOs.Count}";
        reachedUpgradeLevelText.text = $"{(int)currentSelectedHero.HeroSO.HeroData.UpgradeLevel}";
    }

    private void SetupSliders()
    {
        float x = (float)currentSelectedHero.HeroSO.HeroData.UpgradeLevel;
        float y = currentSelectedHero.HeroSO.UpgradeLevelsSO.HeroUpgradeLevelSOs.Count;
        float value = x / y;

        heroStatsHandlerUI.UpdateStats(currentSelectedHero.HeroSO, nextUpgradeLevel);
    }

    private void SetupButtons()
    {
        if (currentSelectedHero.HeroData.IsAcquired)
        {
            buyButton.gameObject.SetActive(false);
            upgradeButton.gameObject.SetActive(true);

            DetermineUpgradeCost(currentSelectedHero.HeroSO);
            heroesUpgradeButtonUI.SetParams(currentUpgradeGoldCost, currentUpgradeCardCost);
        }
        else
        {
            buyButton.gameObject.SetActive(true);
            upgradeButton.gameObject.SetActive(false);
            heroBuyCostText.text = currentSelectedHero.HeroData.Price.ToString();
            nextUpgradeLevel = null;
        }
    }

    private void DetermineUpgradeCost(HeroSO heroSO)
    {
        var currentUpgradeLevle = heroSO.HeroData.UpgradeLevel;

        if ((int)currentUpgradeLevle < heroSO.UpgradeLevelsSO.HeroUpgradeLevelSOs.Count)
        {
            nextUpgradeLevel = heroSO.UpgradeLevelsSO.HeroUpgradeLevelSOs[(int)currentUpgradeLevle];
            currentUpgradeGoldCost = nextUpgradeLevel.UpgradeCostByGold;
            currentUpgradeCardCost = nextUpgradeLevel.UpgradeCostByCard;
        }
        else
        {
            nextUpgradeLevel = null;
            upgradeButton.gameObject.SetActive(false);
        }
    }

    private void OnUpgrade()
    {
        heroesDataHandler.UpgradeHero(currentUpgradeGoldCost, currentUpgradeCardCost);
    }
    
    private void OnBuy()
    {
        if (heroesDataHandler.TryBuyHero())
        {
            upgradeButton.gameObject.SetActive(true);
            buyButton.gameObject.SetActive(false);
        }
    }

}
