using TMPro;
using UnityEngine;

public class TowerUpgradeInformPopupUI : MonoBehaviour
{
    [Header("Popup Setup")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private RectTransform leftSideSpawnPoint;
    [SerializeField] private RectTransform rightSideSpawnPoint;
    [SerializeField] private TextMeshProUGUI towerNameText;

    [Header("Tower Health")]
    [SerializeField] private TextMeshProUGUI towerHealthText;
    [SerializeField] private TextMeshProUGUI towerHealthUpgradeText;
    [SerializeField] private GameObject towerHealthObj;

    [Header("Damage")]   
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private TextMeshProUGUI damageUpgradeText;

    [Header("Attack Rate")]
    [SerializeField] private TextMeshProUGUI attackRateText;
    [SerializeField] private TextMeshProUGUI attackRateUpgradeText;

    [Header("Defenders Health")]
    [SerializeField] private TextMeshProUGUI defendersHealthText;
    [SerializeField] private TextMeshProUGUI defendersHealthUpgradeText;
    [SerializeField] private GameObject defefndersHealthObj;

    private DefenderTowerUpgradeLevelSO defenderTowerUpgradeLevel;


    public void EnablePopup(TowerSO currentUpgradeTowerSO, TowerSO nextUpgradeTowerSO)
    {
        DeterminePopupSpawnPoint();
        gameObject.SetActive(true);
        SetParams(currentUpgradeTowerSO, nextUpgradeTowerSO);     
    }

    private void SetParams(TowerSO currentUpgradeTowerSO, TowerSO nextUpgradeTowerSO)
    {
        towerNameText.text = currentUpgradeTowerSO.TowerName;

        //towerHealthText.text = currentUpgradeTowerSO.CurrentUpgradeLevel.Health.ToString();
        //towerHealthUpgradeText.text = nextUpgradeTowerSO.CurrentUpgradeLevel.Health.ToString();

        damageText.text = currentUpgradeTowerSO.CurrentUpgradeLevel.AttackStats.DamageData.DamageAmount.ToString();
        damageUpgradeText.text = nextUpgradeTowerSO.CurrentUpgradeLevel.AttackStats.DamageData.DamageAmount.ToString();

        attackRateText.text = currentUpgradeTowerSO.CurrentUpgradeLevel.AttackRate.ToString();
        attackRateUpgradeText.text = nextUpgradeTowerSO.CurrentUpgradeLevel.AttackRate.ToString();

        if (currentUpgradeTowerSO.TowerData.TowerType == TowerType.Defender)
        {
            defefndersHealthObj.SetActive(true);
            defenderTowerUpgradeLevel = currentUpgradeTowerSO.CurrentUpgradeLevel as DefenderTowerUpgradeLevelSO;
            defendersHealthText.text = defenderTowerUpgradeLevel.TroopsHealth.ToString();
            defendersHealthUpgradeText.text = defenderTowerUpgradeLevel.TroopsHealth.ToString();
        }
        else
        {
            defefndersHealthObj.SetActive(false);
        }
    }

    public void DisablePopup()
    {
        gameObject.SetActive(false);
    }

    private void DeterminePopupSpawnPoint()
    {
        Vector3[] worldCorners = new Vector3[4];
        rectTransform.GetWorldCorners(worldCorners);

        foreach (Vector3 corner in worldCorners)
        {
            Vector3 viewportPoint = mainCamera.WorldToViewportPoint(corner);

            if (viewportPoint.x < 0)
            {
                rectTransform.position = rightSideSpawnPoint.position;
            }
            else if (viewportPoint.x > 1)
            {
                rectTransform.position = leftSideSpawnPoint.position;
            }
        }
    }
}
