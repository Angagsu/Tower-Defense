using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public Transform[] Waypoint;

    private void Awake()
    {
        Waypoint = new Transform[transform.childCount];
    
        for (int i = 0; i < Waypoint.Length; i++)
        {
            Waypoint[i] = transform.GetChild(i);
        }
    }
}
