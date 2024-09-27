
using UnityEngine;

public class TowersBuildUI : MonoBehaviour
{
    private GroundBehavior selectedGround;
    private Vector3 startPosition;
    public GameObject towerBuildUI;

    private void Start()
    {
        startPosition = transform.position;
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
        transform.position = startPosition;
    } 
}
