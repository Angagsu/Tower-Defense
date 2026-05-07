using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HeroAttackAndDefenseStatsUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Image sliderImage;

    private HeroSkillsInformPopupUI informPopupUI;
    private HeroSO heroSO;


    private int popupPositionId;
    private HeroUpgradeLevelSO nextUpgradeLevel;


    public void SetupSlider(HeroSkillsInformPopupUI informPopupUI, IconsSO iconsSO, HeroSO heroSO, int id, HeroUpgradeLevelSO nextUpgradeLevel = null)
    {
        this.informPopupUI = informPopupUI;
        this.heroSO = heroSO;
        this.nextUpgradeLevel = nextUpgradeLevel;

        popupPositionId = id == 0 ? 2 : 4;

        if (id == 0)
        {
            popupPositionId = 2;
            sliderImage.sprite = iconsSO.AttackStatsSprites[(int)heroSO.CurrentUpgradeLevel.AttackStats.DamageData.DamageType];
        }
        else
        {
            popupPositionId = 4;
            sliderImage.sprite = iconsSO.DefenseSprites[(int)heroSO.CurrentUpgradeLevel.AttackStats.ArmorType];
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        informPopupUI.EnablePopup(heroSO, popupPositionId, nextUpgradeLevel);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        informPopupUI.DisablePopup();
    }
}
