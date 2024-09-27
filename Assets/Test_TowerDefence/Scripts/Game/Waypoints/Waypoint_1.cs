using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint_1 : MonoBehaviour
{
    public static Transform[] waypoints_1;

    private void Awake()
    {
        waypoints_1 = new Transform[transform.childCount];

        for (int i = 0; i < waypoints_1.Length; i++)
        {
            waypoints_1[i] = transform.GetChild(i);
        }
    }
}
