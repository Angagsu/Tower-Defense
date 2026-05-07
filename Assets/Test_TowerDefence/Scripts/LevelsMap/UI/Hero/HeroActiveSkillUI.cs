using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HeroActiveSkillUI : MonoBehaviour , IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Image sliderImage;
    [SerializeField] private Image lockImage;

    private HeroSkillsInformPopupUI informPopupUI;
    private HeroSO heroSO;

    private int id;
    private HeroUpgradeLevelSO nextUpgradeLevel;


    public void SetupSlider(HeroSkillsInformPopupUI informPopupUI, HeroSO heroSO, int id, HeroUpgradeLevelSO nextUpgradeLevel = null)
    {
        this.informPopupUI = informPopupUI;
        this.heroSO = heroSO;
        this.nextUpgradeLevel = nextUpgradeLevel;

        sliderImage.sprite = heroSO.ActiveSkillSprite;

        if (!heroSO.CurrentUpgradeLevel.ActiveSkillAccessibility)
        {
            lockImage.gameObject.SetActive(true);
        }
        else
        {
            lockImage.gameObject.SetActive(false);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        informPopupUI.EnablePopup(heroSO, isPassiveSkill: false, 3, nextUpgradeLevel);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        informPopupUI.DisablePopup();
    }
}
