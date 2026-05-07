using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerPassiveSkillInformPopupUI : MonoBehaviour
{
    [Header("Popup Setup")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private RectTransform[] popupPositions;
    [SerializeField] private TextMeshProUGUI passiveSkillNameText;
    [SerializeField] private TextMeshProUGUI passiveSkillDescriptionText;
    [SerializeField] private TextMeshProUGUI unlockAtLevelText;

    [Header("Passive Values View Setup")]
    [SerializeField] private GameObject statsFieldObject;
    [SerializeField] private Image passiveStatIcon;
    [SerializeField] private Image increaseImage;
    [SerializeField] private TextMeshProUGUI currentUpgradeValueText;
    [SerializeField] private TextMeshProUGUI nextUpgradeValueText;
    


    public void EnablePopup(TowerSO currentUpgradeTowerSO, int id, TowerUpgradeLevelSO nextUpgradeLevel = null)
    {
        SetParams(currentUpgradeTowerSO, id, nextUpgradeLevel);

        gameObject.SetActive(true);
    }

    public void EnablePopup(TowerSO towerSO, int id)
    {
        SetAttackAndDefenseStatsParams(towerSO.CurrentUpgradeLevel.AttackStats, id);
        gameObject.SetActive(true);
    }

    private void SetAttackAndDefenseStatsParams(AttackStats attackStats, int id)
    {
        passiveSkillNameText.text = id == 2 ? "Attack Stats" : "Defense Stats";
        rectTransform.position = popupPositions[id].position;
        statsFieldObject.SetActive(false);
        unlockAtLevelText.text = "";

        if (id == 2)
        {
            switch (attackStats.DamageData.DamageType)
            {
                case DamageType.Physical:
                    passiveSkillDescriptionText.text = $"Damage Type: {attackStats.DamageData.DamageType}\nPhysical Penetration: {attackStats.DamageData.PhysicalPenetration}\n\nDamage Percent To Armor: {70}%\nDamage Percent To Health: {30}%";
                    break;
                case DamageType.Magical:
                    passiveSkillDescriptionText.text = $"Damage Type: {attackStats.DamageData.DamageType}\nMagical Penetration: {attackStats.DamageData.MagicalPenetration}\n\nDamage Percent To Armor: {30}%\nDamage Percent To Health: {70}%";
                    break;
                case DamageType.Pure:
                    passiveSkillDescriptionText.text = $"Damage Type: {attackStats.DamageData.DamageType}\n\nDamage Percent To Armor: {0}%\nDamage Percent To Health: {100}%";
                    break;
                default:
                    break;
            }
        }
        else
        {
            if (attackStats.ArmorType == ArmorType.Unarmored)
            {
                passiveSkillDescriptionText.text = $"Armor Type: {attackStats.ArmorType}\n\nPhysical Resistance: {attackStats.PhysicalResistance}\nMagical Resistance: {attackStats.MagicalResistance}";
            }
            else
            {
                passiveSkillDescriptionText.text = $"Armor Type: {attackStats.ArmorType}\nArmor: {attackStats.Armor}\n\nPhysical Resistance: {attackStats.PhysicalResistance}\nMagical Resistance: {attackStats.MagicalResistance}";
            }
        }
    }

    private void SetParams(TowerSO towerSO, int id, TowerUpgradeLevelSO nextUpgradeLevel = null)
    {
        passiveSkillNameText.text = towerSO.PassiveSkillsNames[id];
        passiveSkillDescriptionText.text = towerSO.PassiveSkillsDescripsions[id];
        passiveStatIcon.sprite = towerSO.PassivSkilleSprites[id];

        if (towerSO.CurrentUpgradeLevel.PassiveSkillsAccessibility.Capacity == 0 || towerSO.CurrentUpgradeLevel.PassiveSkillsAccessibility[id] == false)
        {
            unlockAtLevelText.text = $"Unlock at upgrade level {towerSO.PassiveSkillsUnlockLevels[id]}";
            statsFieldObject.SetActive(false);
        }
        else
        {
            unlockAtLevelText.text = "";
            statsFieldObject.SetActive(true);
            currentUpgradeValueText.text = towerSO.CurrentUpgradeLevel.PassiveSkillsValues[id].ToString();

            if (nextUpgradeLevel)
            {
                if (nextUpgradeLevel.PassiveSkillsValues[id] > towerSO.CurrentUpgradeLevel.PassiveSkillsValues[id])
                {
                    nextUpgradeValueText.text = nextUpgradeLevel.PassiveSkillsValues[id].ToString();
                    increaseImage.gameObject.SetActive(true);
                }
                else
                {
                    nextUpgradeValueText.text = "";
                    increaseImage.gameObject.SetActive(false);
                }
            }
            else
            {
                nextUpgradeValueText.text = "";
                increaseImage.gameObject.SetActive(false);
            }
        }

        rectTransform.position = popupPositions[id].position;
    }

    public void DisablePopup()
    {
        rectTransform.position = popupPositions[2].position;
        gameObject.SetActive(false);
    }
}
