using UnityEngine.EventSystems;
using UnityEngine;

public class GroundBehavior : MonoBehaviour
{
    
    private TowerUpgradeUI towerUpgradeUI;
    private TowersBuildUI towersBuildUI;
    private TowerOnBuy towerOnBuy;
    public TowerBuildManager towerBuildManager;
    private Renderer rend;
    private Color startColor;
    private Vector3 positionOffset;

    [SerializeField] private Color hoverColor;
    [SerializeField] private Color cantBuildColor;
    [SerializeField] private GameObject buildEffectPrefab;

    public DefenderMovement defendersMovement;
    [HideInInspector] public GameObject tower;

    [HideInInspector] public TowerBlueprint towerBlueprint;
    [HideInInspector] public bool IsUpgraded = false;
    [HideInInspector] public bool IsUpgradedSecondTime = false;
    [HideInInspector] public bool IsUpgradedThirdTime = false;

    public Transform defendersStartPoint;


    void Start()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
        towerBuildManager = TowerBuildManager.Instance;
        towerOnBuy = FindObjectOfType<TowerOnBuy>();
        towerUpgradeUI = FindObjectOfType<TowerUpgradeUI>();
        towersBuildUI = FindObjectOfType<TowersBuildUI>();
    }

    private bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
    }

    public void BuildTower(TowerBlueprint towerBlueprint)
    {
        if (PlayerStats.Money < towerBlueprint.Cost)
        {
            Debug.Log("Not Enough Money to Build !!!");
            towerBuildManager.SelectStandardTower = false;
            towerBuildManager.SelectMissileLauncherTower = false;
            towerBuildManager.SelectLaserTower = false;
            return;
        }

        PlayerStats.Money -= towerBlueprint.Cost;
        this.towerBlueprint = towerBlueprint;
        GameObject tower = Instantiate(towerBlueprint.TowerPrefab, GetBuildPosition(), Quaternion.identity);
        GameObject effect = Instantiate(buildEffectPrefab, GetBuildPosition(), Quaternion.identity);
        Destroy(effect.gameObject, 4f);

        this.tower = tower;

        if (towerBlueprint == towerOnBuy.DefenderTower)
        {
            defendersMovement = tower.GetComponentInChildren<DefenderMovement>();
        }
        
        towerBuildManager.SelectStandardTower = false;
        towerBuildManager.SelectMissileLauncherTower = false;
        towerBuildManager.SelectLaserTower = false;
    }

    public void UpgradeTower()
    {
        if (PlayerStats.Money < towerBlueprint.UpgradeCost)
        {
            Debug.Log("Not Enough Money to Upgrade !!!");
            towerBuildManager.SelectStandardTower = false;
            towerBuildManager.SelectMissileLauncherTower = false;
            towerBuildManager.SelectLaserTower = false;
            return;
        }
        
        PlayerStats.Money -= towerBlueprint.UpgradeCost;

        //Destroy Old Tower
        Destroy(this.tower);

        //Upgrade Tower
        GameObject tower = Instantiate(towerBlueprint.UpgradedTower, GetBuildPosition(), Quaternion.identity);
        GameObject effect = Instantiate(buildEffectPrefab, GetBuildPosition(), Quaternion.identity);
        Destroy(effect.gameObject, 4f);
        this.tower = tower;

        IsUpgraded = true;
        IsUpgradedSecondTime = false;
        IsUpgradedThirdTime = false;

        towerBuildManager.SelectStandardTower = false;
        towerBuildManager.SelectMissileLauncherTower = false;
        towerBuildManager.SelectLaserTower = false;
    }

    public void SecondTimeUpgradeTower()
    {
        if (PlayerStats.Money < towerBlueprint.SecondTimeUpgradeCost)
        {
            Debug.Log("Not Enough Money to Upgrade !!!");
            towerBuildManager.SelectStandardTower = false;
            towerBuildManager.SelectMissileLauncherTower = false;
            towerBuildManager.SelectLaserTower = false;
            return;
        }
        PlayerStats.Money -= towerBlueprint.SecondTimeUpgradeCost;

        Destroy(this.tower);

        //Upgrade Tower
        GameObject tower = Instantiate(towerBlueprint.SecondTimeUpgradedTower, GetBuildPosition(), Quaternion.identity);
        GameObject effect = Instantiate(buildEffectPrefab, GetBuildPosition(), Quaternion.identity);
        Destroy(effect.gameObject, 4f);
        this.tower = tower;

        IsUpgradedSecondTime = true;
        IsUpgradedThirdTime = false;
        IsUpgraded = false;

        towerBuildManager.SelectStandardTower = false;
        towerBuildManager.SelectMissileLauncherTower = false;
        towerBuildManager.SelectLaserTower = false;
    }

    public void ThirdTimeUpgradeTower()
    {
        if (PlayerStats.Money < towerBlueprint.ThirdTimeUpgradeCost)
        {
            Debug.Log("Not Enough Money to Upgrade !!!");
            towerBuildManager.SelectStandardTower = false;
            towerBuildManager.SelectMissileLauncherTower = false;
            towerBuildManager.SelectLaserTower = false;
            return;
        }
        PlayerStats.Money -= towerBlueprint.ThirdTimeUpgradeCost;

        Destroy(this.tower);

        //Upgrade Tower
        GameObject tower = Instantiate(towerBlueprint.ThirdTimeUpgradedTower, GetBuildPosition(), Quaternion.identity);
        GameObject effect = Instantiate(buildEffectPrefab, GetBuildPosition(), Quaternion.identity);
        Destroy(effect.gameObject, 4f);
        this.tower = tower;

        IsUpgradedThirdTime = true;
        IsUpgradedSecondTime = false;
        IsUpgraded = false;

        towerBuildManager.SelectStandardTower = false;
        towerBuildManager.SelectMissileLauncherTower = false;
        towerBuildManager.SelectLaserTower = false;
    }

    public void SellTower()
    {
        Destroy(tower);
        if (IsUpgradedSecondTime)
        {
            PlayerStats.Money += towerBlueprint.GetSecondTimeUpgradedTowerSellCost();
            IsUpgradedSecondTime = false;
        }
        else if (IsUpgraded)
        {
            PlayerStats.Money += towerBlueprint.GetUpgradedTowerSellCost();
            IsUpgraded = false;
        }
        else if (IsUpgradedThirdTime)
        {
            PlayerStats.Money += towerBlueprint.GetThirdTimeUpgradedTowerSellCost();
            IsUpgradedThirdTime = false;
        }
        else
        {
            PlayerStats.Money += towerBlueprint.GetTowerSellCost();
        }
    }

    public Vector3 GetBuildPosition()
    {
        if (towerBlueprint == towerOnBuy.ArrowTower)
        {
            positionOffset = new Vector3(0, -0.7f, 0);
        }
        
        if (towerBlueprint == towerOnBuy.GolemTower || towerBlueprint == towerOnBuy.FireTower)
        {
            positionOffset = new Vector3(0, 2f, 0);
        }
        
        if (towerBlueprint == towerOnBuy.ThunderTower || towerBlueprint == towerOnBuy.IceTower)
        {
            positionOffset = new Vector3(0, 0.5f, 0);
        }

        if (towerBlueprint == towerOnBuy.DefenderTower)
        {
            positionOffset = new Vector3(0, 2, 0);
        }
        return transform.position + positionOffset;
    }
    
    public void SelectDefenderTowerOnUI()
    {
        defendersMovement.SelectOrDeSelectDefenders(true);
        towerBuildManager.DeselectGround();
    }
    
}
