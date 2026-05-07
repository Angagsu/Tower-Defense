using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TowerAttackAndDefenseStatsUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Image sliderImage;


    private TowerPassiveSkillInformPopupUI informPopupUI;
    private TowerSO towerSO;

    private int popupPositionId;



    public void SetupSlider(TowerPassiveSkillInformPopupUI informPopupUI, IconsSO iconsSO, TowerSO towerSO, int id)
    {
        this.informPopupUI = informPopupUI;
        this.towerSO = towerSO;

        popupPositionId = id == 0 ? 2 : 3;

        if (id == 0)
        {
            popupPositionId = 2;
            sliderImage.sprite = iconsSO.AttackStatsSprites[(int)towerSO.CurrentUpgradeLevel.AttackStats.DamageData.DamageType];
        }
        else
        {
            popupPositionId = 3;
            sliderImage.sprite = iconsSO.DefenseSprites[(int)towerSO.CurrentUpgradeLevel.AttackStats.ArmorType];
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        informPopupUI.EnablePopup(towerSO, popupPositionId);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        informPopupUI.DisablePopup();
    }
}
