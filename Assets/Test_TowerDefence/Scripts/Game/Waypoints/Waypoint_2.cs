using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint_2 : MonoBehaviour
{
    public static Transform[] waypoints_2;

    private void Awake()
    {
        waypoints_2 = new Transform[transform.childCount];

        for (int i = 0; i < waypoints_2.Length; i++)
        {
            waypoints_2[i] = transform.GetChild(i);
        }
    }
}
