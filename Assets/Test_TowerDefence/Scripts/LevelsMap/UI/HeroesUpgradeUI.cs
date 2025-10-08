using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class HeroesUpgradeUI : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private Slider healthBar;
    [SerializeField] private TextMeshProUGUI currentHealthValueText;
    [SerializeField] private TextMeshProUGUI nextUpgradeHealthValueText;
    [Header("Damage")]
    [SerializeField] private Slider damageBar;
    [SerializeField] private TextMeshProUGUI currentDamageValueText;
    [SerializeField] private TextMeshProUGUI nextUpgradeDamageValueText;
    [Header("Speed")]
    [SerializeField] private Slider speedBar;
    [SerializeField] private TextMeshProUGUI currentSpeedValueText;
    [SerializeField] private TextMeshProUGUI nextUpgradeSpeedValueText;
    [Header("Buttons")]
    [SerializeField] private Button buyButton;
    [SerializeField] private Button upgradeButton;
    [Space(10)]
    [SerializeField] private Slider reachedUpgradeLevelBar;
    [SerializeField] private TextMeshProUGUI heroBuyCostText;
    [SerializeField] private TextMeshProUGUI reachedUpgradeLevelText;
    [SerializeField] private TextMeshProUGUI reachedUpgradeLevelProgressText;
    [Header("Cards")]
    [SerializeField] private TextMeshProUGUI receivedCardsText;
    [SerializeField] private CardUI cardUI;
    [SerializeField] private Image heroImage;
    [Space(10)]
    [SerializeField] private HeroesUpgradeButtonUI heroesUpgradeButtonUI;
    [SerializeField] private HeroesDataHandler heroesDataHandler;


    private List<LevelsMapHero> levelsMapHeros;
    private LevelsMapHero currentSelectedHero;
    private HeroUpgradeLevelSO nextUpgradeLevel;

    private int currentUpgradeGoldCost;

    private void Awake()
    {
        levelsMapHeros = heroesDataHandler.GetLevelsMapHeroes();

        currentSelectedHero = levelsMapHeros[0];
        SetSelectedHeroStats(currentSelectedHero);

        buyButton.onClick.AddListener(OnBuy);
        upgradeButton.onClick.AddListener(OnUpgrade);
    }

    public void SetSelectedHeroStats(LevelsMapHero currentSelectedHero)
    {
        if (this.currentSelectedHero && this.currentSelectedHero != currentSelectedHero)
        {
            this.currentSelectedHero.UnSelect(); 
        }

        this.currentSelectedHero = currentSelectedHero;
        this.currentSelectedHero.Select();

        SetupCardParams();
        SetupButtons();
        SetupTexts();
        SetupSliders();
    }

    private void SetupCardParams()
    {
        cardUI.SetParams(currentSelectedHero.HeroSO);
        receivedCardsText.text = currentSelectedHero.HeroSO.HeroData.ReseivedCards.Count.ToString();
    }

    private void SetupTexts()
    {
        heroImage.sprite = currentSelectedHero.HeroSO.Sprite;

        reachedUpgradeLevelProgressText.text = $"{(int)currentSelectedHero.HeroSO.HeroData.UpgradeLevel} / {currentSelectedHero.HeroSO.UpgradeLevelsSO.HeroUpgradeLevelSOs.Count}";
        reachedUpgradeLevelText.text = $"{(int)currentSelectedHero.HeroSO.HeroData.UpgradeLevel}";

        currentHealthValueText.text = currentSelectedHero.HeroSO.CurrentUpgradeLevel.Health.ToString();
        currentDamageValueText.text = currentSelectedHero.HeroSO.CurrentUpgradeLevel.Damage.ToString();
        currentSpeedValueText.text = currentSelectedHero.HeroSO.CurrentUpgradeLevel.MoveSpeed.ToString();

        if (nextUpgradeLevel)
        {
            nextUpgradeHealthValueText.text = $"+{nextUpgradeLevel.Health - currentSelectedHero.HeroSO.CurrentUpgradeLevel.Health}";
            nextUpgradeDamageValueText.text = $"+{nextUpgradeLevel.Damage - currentSelectedHero.HeroSO.CurrentUpgradeLevel.Damage}";
            nextUpgradeSpeedValueText.text = $"+{nextUpgradeLevel.MoveSpeed - currentSelectedHero.HeroSO.CurrentUpgradeLevel.MoveSpeed}";
        }
        else
        {
            nextUpgradeHealthValueText.text = "";
            nextUpgradeDamageValueText.text = "";
            nextUpgradeSpeedValueText.text = "";
        }
    }

    private void SetupSliders()
    {
        float x = (float)currentSelectedHero.HeroSO.HeroData.UpgradeLevel;
        float y = currentSelectedHero.HeroSO.UpgradeLevelsSO.HeroUpgradeLevelSOs.Count;
        float value = x / y;

        reachedUpgradeLevelBar.value = value;
        healthBar.value = currentSelectedHero.HeroSO.CurrentUpgradeLevel.Health / 1000;
        damageBar.value = currentSelectedHero.HeroSO.CurrentUpgradeLevel.Damage / 100;
        speedBar.value = currentSelectedHero.HeroSO.CurrentUpgradeLevel.MoveSpeed / 100;
    }

    private void SetupButtons()
    {
        if (currentSelectedHero.HeroData.IsAcquired)
        {
            buyButton.gameObject.SetActive(false);
            upgradeButton.gameObject.SetActive(true);

            DetermineUpgradeCost(currentSelectedHero.HeroSO);
            heroesUpgradeButtonUI.SetParams(currentUpgradeGoldCost);
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
        }
        else
        {
            nextUpgradeLevel = null;
            upgradeButton.gameObject.SetActive(false);
        }
    }

    private void OnUpgrade()
    {
        heroesDataHandler.UpgradeHero(currentUpgradeGoldCost);
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
