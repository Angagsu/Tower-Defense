using TMPro;
using UnityEngine;


public class TowerBuildInformPopupUI : MonoBehaviour
{
    [Header("Popup Setup")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private RectTransform leftSideSpawnPoint;
    [SerializeField] private RectTransform rightSideSpawnPoint;

    [Header("Tower Stats")]
    [SerializeField] private TextMeshProUGUI towerNameText;
    [SerializeField] private TextMeshProUGUI towerDescriptionText;
    [SerializeField] private TextMeshProUGUI damageText;

    [Header("Health")]
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private GameObject healthObj;




    public void EnablePopup(TowerSO towerSO)
    {
        DeterminePopupSpawnPoint();
        gameObject.SetActive(true);
        SetParams(towerSO);
    }

    public void DisablePopup()
    {
        gameObject.SetActive(false); 
    }

    private void SetParams(TowerSO towerSO)
    {
        towerNameText.text = towerSO.TowerName;
        towerDescriptionText.text = towerSO.TowerDescription;
        damageText.text = towerSO.CurrentUpgradeLevel.AttackStats.DamageData.DamageAmount.ToString();
        if (towerSO.TowerName == "Defender")
        {
            healthObj.SetActive(true);
        }
        else
        {
            healthObj.SetActive(false);
        }
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
