using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerUI : MonoBehaviour
{
    private GroundBehavior targetGround;
    [SerializeField] private GameObject uI;

    
    public void SetTargetGround(GroundBehavior targetGround)
    {
        this.targetGround = targetGround;
        transform.position = targetGround.GetBuildPosition();
        uI.SetActive(true);
    }

    public void HideCanvas()
    {
        uI.SetActive(false);
    }
}
