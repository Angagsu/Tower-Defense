using System.Collections.Generic;
using UnityEngine;

public class WaypointsService : MonoBehaviour
{
    public static  List<Way>  Ways { get; private set; }

    private void Awake()
    {
        Ways = new List<Way>();
        
        for (int i = 0; i < transform.childCount; i++)
        {
            Ways.Add(transform.GetChild(i).GetComponent<Way>());
        }
    }
}
