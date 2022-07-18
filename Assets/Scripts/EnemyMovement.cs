
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private float enemySpeed = 10f;
    private Transform target;
    private int wayCountIndex = 0;

    private void Start()
    {
        target = WayPoints.wayPoints[0];
        
    }

    private void Update()
    {
        Vector3 direction = target.position - transform.position;
        transform.Translate(direction.normalized * enemySpeed * Time.deltaTime , Space.World);

        if (Vector3.Distance(target.transform.position, transform.position) <= 0.5f)
        {
            GetNextWayPoint();
        }
    }

    private void GetNextWayPoint()
    {
        if (WayPoints.wayPoints.Length - 1 <= wayCountIndex)
        {
            Destroy(gameObject);
            return;
        }

        wayCountIndex++;
        target = WayPoints.wayPoints[wayCountIndex];
    }
}
