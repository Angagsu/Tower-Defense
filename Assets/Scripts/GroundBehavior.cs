using UnityEngine.EventSystems;
using UnityEngine;

public class GroundBehavior : MonoBehaviour
{
    private Renderer rend;
    private Color startColor;
    public GameObject tower;
    private TowerBuildManager towerBuildManager;
    [SerializeField] private Color hoverColor;
    [SerializeField] private Color cantBuildColor;

    public Vector3 positionOffset;

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
            towerBuildManager.SelectStandardTower == true && towerBuildManager.HasManey)
        {
            rend.material.color = hoverColor;
        }
        else if(towerBuildManager.SelectMissileLauncherTower == true && !towerBuildManager.HasManey ||
            towerBuildManager.SelectStandardTower == true && !towerBuildManager.HasManey)
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

        if (!towerBuildManager.CanBuild)
        {
            return;
        }

        if (tower != null)
        {
            
            return;
        }

        //Build a tower
        if (towerBuildManager.SelectMissileLauncherTower == true || towerBuildManager.SelectStandardTower == true)
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
        return transform.position + positionOffset;
    }
}
