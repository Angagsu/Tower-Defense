using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterStatsInformPopupUI : MonoBehaviour
{
    [Header("Popup Setup")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private RectTransform[] popupPositions;
    [SerializeField] private TextMeshProUGUI statNameText;
    [SerializeField] private TextMeshProUGUI statDescriptionText;




    public void EnablePopup(MonsterSO monsterSO, int id)
    {
        SetAttackAndDefenseStatsParams(monsterSO.Stats, id);
        gameObject.SetActive(true);
    }


    private void SetAttackAndDefenseStatsParams(AttackStats attackStats, int id)
    {
        statNameText.text = id == 0 ? "Attack Stats" : "Defense Stats";
        rectTransform.position = popupPositions[id].position;


        if (id == 0)
        {
            switch (attackStats.DamageData.DamageType)
            {
                case DamageType.Physical:
                    statDescriptionText.text = $"Damage Type: {attackStats.DamageData.DamageType}\nPhysical Penetration: {attackStats.DamageData.PhysicalPenetration}\n\nDamage Percent To Armor: {70}%\nDamage Percent To Health: {30}%";
                    break;
                case DamageType.Magical:
                    statDescriptionText.text = $"Damage Type: {attackStats.DamageData.DamageType}\nMagical Penetration: {attackStats.DamageData.MagicalPenetration}\n\nDamage Percent To Armor: {30}%\nDamage Percent To Health: {70}%";
                    break;
                case DamageType.Pure:
                    statDescriptionText.text = $"Damage Type: {attackStats.DamageData.DamageType}\n\nDamage Percent To Armor: {0}%\nDamage Percent To Health: {100}%";
                    break;
                default:
                    break;
            }
        }
        else
        {
            if (attackStats.ArmorType == ArmorType.Unarmored)
            {
                statDescriptionText.text = $"Armor Type: {attackStats.ArmorType}\n\nPhysical Resistance: {attackStats.PhysicalResistance}\nMagical Resistance: {attackStats.MagicalResistance}";
            }
            else
            {
                statDescriptionText.text = $"Armor Type: {attackStats.ArmorType}\nArmor: {attackStats.Armor}\n\nPhysical Resistance: {attackStats.PhysicalResistance}\nMagical Resistance: {attackStats.MagicalResistance}";
            }
        }
    }

    public void DisablePopup()
    {
        rectTransform.position = popupPositions[2].position;
        gameObject.SetActive(false);
    }
}
