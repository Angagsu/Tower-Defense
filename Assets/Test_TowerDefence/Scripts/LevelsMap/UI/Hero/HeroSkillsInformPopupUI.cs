using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroSkillsInformPopupUI : MonoBehaviour
{
    [Header("Popup Setup")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private RectTransform[] popupPositions;
    [SerializeField] private TextMeshProUGUI passiveSkillNameText;
    [SerializeField] private TextMeshProUGUI passiveSkillDescriptionText;
    [SerializeField] private TextMeshProUGUI unlockAtLevelText;

    [Header("Skill Values View Setup")]
    [SerializeField] private GameObject statsFieldObject;
    [SerializeField] private Image passiveStatIcon;
    [SerializeField] private Image increaseImage;
    [SerializeField] private TextMeshProUGUI currentUpgradeValueText;
    [SerializeField] private TextMeshProUGUI nextUpgradeValueText;



    public void EnablePopup(HeroSO HeroSO, bool isPassiveSkill, int id, HeroUpgradeLevelSO nextUpgradeLevel = null)
    {
        if (isPassiveSkill)
        {
            SetPassiveSkillParams(HeroSO, id, nextUpgradeLevel);
        }else
        {
            SetActiveSkillParams(HeroSO, id, nextUpgradeLevel);
        }

        gameObject.SetActive(true);
    }

    public void EnablePopup(HeroSO HeroSO, int id, HeroUpgradeLevelSO nextUpgradeLevel = null)
    {
        SetAttackAndDefenseStatsParams(HeroSO.CurrentUpgradeLevel.AttackStats, id, nextUpgradeLevel);
        gameObject.SetActive(true);
    }

    private void SetPassiveSkillParams(HeroSO heroSO, int id, HeroUpgradeLevelSO nextUpgradeLevel = null)
    {
        passiveSkillNameText.text = heroSO.PassiveSkillsNames[id];
        passiveSkillDescriptionText.text = heroSO.PassiveSkillsDescripsions[id];
        passiveStatIcon.sprite = heroSO.PassivSkillsSprites[id];

        if (heroSO.CurrentUpgradeLevel.PassiveSkillsAccessibility.Capacity == 0 || heroSO.CurrentUpgradeLevel.PassiveSkillsAccessibility[id] == false)
        {
            unlockAtLevelText.text = $"Unlock at upgrade level {heroSO.PassiveSkillsUnlockLevels[id]}";
            statsFieldObject.SetActive(false);
        }
        else
        {
            unlockAtLevelText.text = "";
            statsFieldObject.SetActive(true);
            currentUpgradeValueText.text = heroSO.CurrentUpgradeLevel.PassiveSkillsValues[id].ToString();

            if (nextUpgradeLevel)
            {
                if (nextUpgradeLevel.PassiveSkillsValues[id] > heroSO.CurrentUpgradeLevel.PassiveSkillsValues[id])
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

    private void SetActiveSkillParams(HeroSO heroSO, int id, HeroUpgradeLevelSO nextUpgradeLevel = null)
    {
        passiveSkillNameText.text = heroSO.ActiveSkillName;
        passiveSkillDescriptionText.text = heroSO.ActiveSkillDescripsion;
        passiveStatIcon.sprite = heroSO.ActiveSkillSprite;

        if (!heroSO.CurrentUpgradeLevel.ActiveSkillAccessibility)
        {
            unlockAtLevelText.text = $"Unlock at upgrade level {heroSO.ActiveSkillUnlockLevel}";
            statsFieldObject.SetActive(false);
        }
        else
        {
            unlockAtLevelText.text = "";
            statsFieldObject.SetActive(true);
            currentUpgradeValueText.text = heroSO.CurrentUpgradeLevel.ActiveSkillValue.ToString();

            if (nextUpgradeLevel)
            {
                if (nextUpgradeLevel.ActiveSkillValue > heroSO.CurrentUpgradeLevel.ActiveSkillValue)
                {
                    nextUpgradeValueText.text = nextUpgradeLevel.ActiveSkillValue.ToString();
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

    private void SetAttackAndDefenseStatsParams(AttackStats attackStats, int id, HeroUpgradeLevelSO nextUpgradeLevel = null)
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
            if(attackStats.ArmorType == ArmorType.Unarmored)
            {
                passiveSkillDescriptionText.text = $"Armor Type: {attackStats.ArmorType}\n\nPhysical Resistance: {attackStats.PhysicalResistance}\nMagical Resistance: {attackStats.MagicalResistance}";
            }
            else
            {
                passiveSkillDescriptionText.text = $"Armor Type: {attackStats.ArmorType}\nArmor: {attackStats.Armor}\n\nPhysical Resistance: {attackStats.PhysicalResistance}\nMagical Resistance: {attackStats.MagicalResistance}";
            }
        }
    }

    public void DisablePopup()
    {
        rectTransform.position = popupPositions[2].position;
        gameObject.SetActive(false);
    }
}
