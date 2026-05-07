using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TowerPassiveSkillSliderUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Image sliderImage;
    [SerializeField] private Image lockImage;

    private TowerPassiveSkillInformPopupUI informPopupUI;
    private TowerSO towerSO;
    
    private int id;
    private TowerUpgradeLevelSO nextUpgradeLevel;


    public void SetupSlider(TowerPassiveSkillInformPopupUI informPopupUI, TowerSO towerSO, int id, TowerUpgradeLevelSO nextUpgradeLevel = null)
    {
        this.informPopupUI = informPopupUI;
        this.towerSO = towerSO;
        this.id = id;
        this.nextUpgradeLevel = nextUpgradeLevel;

        sliderImage.sprite = towerSO.PassivSkilleSprites[id];

        if (towerSO.CurrentUpgradeLevel.PassiveSkillsAccessibility.Capacity == 0 || towerSO.CurrentUpgradeLevel.PassiveSkillsAccessibility[id] == false)
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
        informPopupUI.EnablePopup(towerSO, id, nextUpgradeLevel);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        informPopupUI.DisablePopup();
    }
}
