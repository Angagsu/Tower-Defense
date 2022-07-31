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

    [HideInInspector]
    public GameObject tower;
    

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
            towerBuildManager.BuildTowerOn(this);
        }
        
        
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
