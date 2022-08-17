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

    [HideInInspector] public GameObject tower;

    [HideInInspector] public TowerBlueprint towerBlueprint;
    [HideInInspector] public bool IsUpgraded = false;
    [HideInInspector] public bool IsUpgradedSecondTime = false;
    [HideInInspector] public bool IsUpgradedThirdTime = false;

    void Start()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
        towerBuildManager = TowerBuildManager.Instance;
        towerOnBuy = FindObjectOfType<TowerOnBuy>();
        towerUpgradeUI = FindObjectOfType<TowerUpgradeUI>();
        towersBuildUI = FindObjectOfType<TowersBuildUI>();
    }

    private void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (!towerBuildManager.CanBuild)
        {
            return;
        }

        if ( towerBuildManager.HasManey)
        {
            rend.material.color = hoverColor;
        }
        else if(towerBuildManager.SelectMissileLauncherTower == true && !towerBuildManager.HasManey ||
            towerBuildManager.SelectStandardTower == true && !towerBuildManager.HasManey || towerBuildManager.SelectLaserTower == true && !towerBuildManager.HasManey)
        {
            rend.material.color = cantBuildColor;
        }
    }

    private void OnMouseExit()
    {
        rend.material.color = startColor;
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (tower != null)
        {
            towerBuildManager.SelectedGround(this);
            return;
        }

        if (tower == null)
        {
            towerBuildManager.SelectedGroundForBuildTowerUI(this);
            towerOnBuy.SetGroundForBuilding(this);
            return;
        }
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
        if (towerBlueprint == towerOnBuy.standardTower)
        {
            positionOffset = new Vector3(0, -0.7f, 0);
        }
        
        if (towerBlueprint == towerOnBuy.missileLauncherTower)
        {
            positionOffset = new Vector3(0, 2f, 0);
        }
        
        if (towerBlueprint == towerOnBuy.laserTower)
        {
            positionOffset = new Vector3(0, 0.5f, 0);
        }
        return transform.position + positionOffset;
    }
    
}
