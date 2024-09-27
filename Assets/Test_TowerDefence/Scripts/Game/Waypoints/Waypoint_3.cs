using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint_3 : MonoBehaviour
{
    public static Transform[] waypoints_3;

    private void Awake()
    {
        waypoints_3 = new Transform[transform.childCount];

        for (int i = 0; i < waypoints_3.Length; i++)
        {
            waypoints_3[i] = transform.GetChild(i);
        }
    }
}
