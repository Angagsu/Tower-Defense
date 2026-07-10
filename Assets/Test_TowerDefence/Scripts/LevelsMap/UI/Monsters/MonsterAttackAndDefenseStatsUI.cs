using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MonsterAttackAndDefenseStatsUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Image sliderImage;

    private MonsterStatsInformPopupUI informPopupUI;
    private MonsterSO monsterSO;


    private int popupPositionId;


    public void SetupSlider(MonsterStatsInformPopupUI informPopupUI, IconsSO iconsSO, MonsterSO monsterSO, int id)
    {
        this.informPopupUI = informPopupUI;
        this.monsterSO = monsterSO;

        popupPositionId = id == 0 ? 2 : 4;

        if (id == 0)
        {
            popupPositionId = 0;
            sliderImage.sprite = iconsSO.AttackStatsSprites[(int)monsterSO.Stats.DamageData.DamageType];
        }
        else
        {
            popupPositionId = 1;
            sliderImage.sprite = iconsSO.DefenseSprites[(int)monsterSO.Stats.ArmorType];
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        informPopupUI.EnablePopup(monsterSO, popupPositionId);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        informPopupUI.DisablePopup();
    }
}
