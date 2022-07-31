using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyMovement : MonoBehaviour
{
    private Enemy enemy;
    private Transform target;
    private int wayCountIndex = 0;

    private void Start()
    {
        enemy = GetComponent<Enemy>();
        target = WayPoints.wayPoints[0];
    }

    private void Update()
    {
        Vector3 direction = target.position - transform.position;
        transform.Translate(direction.normalized * enemy.enemySpeed * Time.deltaTime, Space.World);

        if (Vector3.Distance(target.transform.position, transform.position) <= 0.5f)
        {
            GetNextWayPoint();
        }

        enemy.enemySpeed = enemy.startSpeed;
    }

    private void GetNextWayPoint()
    {
        if (WayPoints.wayPoints.Length - 1 <= wayCountIndex)
        {
            PathEnd();
            return;
        }

        wayCountIndex++;
        target = WayPoints.wayPoints[wayCountIndex];
    }

    private void PathEnd()
    {
        PlayerStats.Lives--;
        Destroy(gameObject);
    }
}
