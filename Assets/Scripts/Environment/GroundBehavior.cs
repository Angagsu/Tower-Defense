using UnityEngine.EventSystems;
using UnityEngine;

public class GroundBehavior : MonoBehaviour
{
    
    private TowerBuildManager towerBuildManager;
    private Renderer rend;
    private Color startColor;
    private Vector3 positionOffset;

    [SerializeField] private Color hoverColor;
    [SerializeField] private Color cantBuildColor;
    [SerializeField] private GameObject buildEffectPrefab;

    [HideInInspector] public GameObject tower;
    [HideInInspector] public TowerBlueprint towerBlueprint;
    

    void Start()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
        towerBuildManager = TowerBuildManager.Instance;
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

        if (towerBuildManager.SelectMissileLauncherTower == true && towerBuildManager.HasManey ||
            towerBuildManager.SelectStandardTower == true && towerBuildManager.HasManey || towerBuildManager.SelectLaserTower == true && towerBuildManager.HasManey)
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

        if (!towerBuildManager.CanBuild)
        {
            return;
        }

        //Build a tower
        if (towerBuildManager.SelectMissileLauncherTower == true || towerBuildManager.SelectStandardTower == true || towerBuildManager.SelectLaserTower == true)
        {
            BuildTower(towerBuildManager.GetTowerToBuild());
        }  
    }

    private void BuildTower(TowerBlueprint towerBlueprint)
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

        GameObject tower = Instantiate(towerBlueprint.TowerPrefab, GetBuildPosition(), Quaternion.identity);
        GameObject effect = Instantiate(buildEffectPrefab, GetBuildPosition(), Quaternion.identity);
        Destroy(effect.gameObject, 4f);

        this.tower = tower;
        this.towerBlueprint = towerBlueprint;

        towerBuildManager.SelectStandardTower = false;
        towerBuildManager.SelectMissileLauncherTower = false;
        towerBuildManager.SelectLaserTower = false;

        Debug.Log("Tower build! Money left: " + PlayerStats.Money);
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
        GameObject tower = Instantiate(towerBlueprint.UpgrateTower, GetBuildPosition(), Quaternion.identity);
        GameObject effect = Instantiate(buildEffectPrefab, GetBuildPosition(), Quaternion.identity);
        Destroy(effect.gameObject, 4f);
        this.tower = tower;

        towerBuildManager.IsUpgraded = true;
        towerBuildManager.SelectStandardTower = false;
        towerBuildManager.SelectMissileLauncherTower = false;
        towerBuildManager.SelectLaserTower = false;

        Debug.Log("Tower Upgraded! Money left: " + PlayerStats.Money);
    }

    public Vector3 GetBuildPosition()
    {
        if (towerBuildManager.SelectStandardTower == true)
        {
            positionOffset = new Vector3(0, -0.5f, 0);
        }

        if (towerBuildManager.SelectMissileLauncherTower == true)
        {
            positionOffset = new Vector3(0, 2f, 0);
        }

        if (towerBuildManager.SelectLaserTower == true)
        {
            positionOffset = new Vector3(0, 0.5f, 0);
        }
        return transform.position + positionOffset;
    }
}
