
using UnityEngine;

public class WayPoints_3 : MonoBehaviour
{
    public static Transform[] wayPoints_3;

    private void Awake()
    {
        wayPoints_3 = new Transform[transform.childCount];

        for (int i = 0; i < wayPoints_3.Length; i++)
        {
            wayPoints_3[i] = transform.GetChild(i);
        }
    }
}
