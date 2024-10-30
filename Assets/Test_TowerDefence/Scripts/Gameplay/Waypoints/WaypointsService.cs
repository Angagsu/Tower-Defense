using System.Collections.Generic;
using UnityEngine;

public class WaypointsService : MonoBehaviour
{
    public static  List<Waypoints>  Waypoints { get; private set; }

    private void Awake()
    {
        Waypoints = new List<Waypoints>();

        for (int i = 0; i < transform.childCount; i++)
        {
            Waypoints.Add(transform.GetChild(i).GetComponent<Waypoints>());
        }
    }
}
