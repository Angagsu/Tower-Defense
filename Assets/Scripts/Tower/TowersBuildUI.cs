using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowersBuildUI : MonoBehaviour
{
    [SerializeField] private GameObject uIRotatPart;
    [SerializeField] private Camera cameraPosition;
    private GroundBehavior selectedGround;
    
    public GameObject towerBuildUI;
    public bool IsTowerBuild = false;
    public float turnSpeed = 60f;

    private void Update()
    {
        
    }
    public void SetTargetGroundForBuilding(GroundBehavior selectedGround)
    {
        this.selectedGround = selectedGround;
        transform.position = selectedGround.GetBuildPosition() + new Vector3(0, 2, 2);
        towerBuildUI.SetActive(true);
    }

    public void HideCanvas()
    {
        towerBuildUI.SetActive(false);
    }

    
}
