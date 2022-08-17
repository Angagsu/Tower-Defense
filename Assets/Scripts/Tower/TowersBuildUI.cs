using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowersBuildUI : MonoBehaviour
{
    private GroundBehavior selectedGround;
    [SerializeField] private GameObject towerBuildUI;
    public bool IsTowerBuild = false;
   

    public void SetTargetGroundForBuilding(GroundBehavior selectedGround)
    {
        this.selectedGround = selectedGround;
        transform.position = selectedGround.GetBuildPosition() + new Vector3(0, 15, 0);
        towerBuildUI.SetActive(true);
    }

    public void HideCanvas()
    {
        towerBuildUI.SetActive(false);
    }
}
