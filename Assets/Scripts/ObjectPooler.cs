using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public GameObjectPool bullet;

    private void FixedUpdate()
    {
        bullet.SpawnObject(transform.position, transform.rotation);
    }

}
