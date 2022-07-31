using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerUpgrade : MonoBehaviour
{
    [SerializeField]
    private GroundBehavior groundBehavior;
    private TowerBuildManager towerBuildManager;
    private TowerBlueprint towerToBuild;

    [SerializeField] private GameObject buildEffectPrefab;

    void Start()
    {
        towerBuildManager = GetComponent<TowerBuildManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpgradeTower()
    {
        if (PlayerStats.Money < towerToBuild.UpgradeCost)
        {
            Debug.Log("Not Enough Money to Upgrade !!!");
            towerBuildManager.SelectStandardTower = false;
            towerBuildManager.SelectMissileLauncherTower = false;
            towerBuildManager.SelectLaserTower = false;
            return;
        }

        PlayerStats.Money -= towerToBuild.UpgradeCost;

        GameObject tower = Instantiate(towerToBuild.UpgrateTower, groundBehavior.GetBuildPosition(), Quaternion.identity);
        GameObject effect = Instantiate(buildEffectPrefab, groundBehavior.GetBuildPosition(), Quaternion.identity);
        Destroy(effect.gameObject, 4f);
        groundBehavior.tower = tower;
        towerBuildManager.SelectStandardTower = false;
        towerBuildManager.SelectMissileLauncherTower = false;
        towerBuildManager.SelectLaserTower = false;

        Debug.Log("Tower Upgrade! Money left: " + PlayerStats.Money);
    }

    public void SellTower()
    {

    }
}
