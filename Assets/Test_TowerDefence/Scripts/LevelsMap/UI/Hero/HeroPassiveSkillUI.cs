using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HeroPassiveSkillUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
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
        this.id = id;
        this.nextUpgradeLevel = nextUpgradeLevel;

        sliderImage.sprite = heroSO.PassivSkillsSprites[id];

        if (heroSO.CurrentUpgradeLevel.PassiveSkillsAccessibility.Capacity == 0 || heroSO.CurrentUpgradeLevel.PassiveSkillsAccessibility[id] == false)
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
        informPopupUI.EnablePopup(heroSO, true, id, nextUpgradeLevel);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        informPopupUI.DisablePopup();
    }
}
