
using UnityEngine;

public class WayPoints_2 : MonoBehaviour
{
    public static Transform[] wayPoints_2;

    private void Awake()
    {
        wayPoints_2 = new Transform[transform.childCount];

        for (int i = 0; i < wayPoints_2.Length; i++)
        {
            wayPoints_2[i] = transform.GetChild(i);
        }
    }
}
