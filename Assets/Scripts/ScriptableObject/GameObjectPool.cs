using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="My Assets/GameObject Pool")]
public class GameObjectPool : ScriptableObject
{
    public GameObject prefab;
    private List<PooledObject> objectsInPool;
    private List<PooledObject> objectsInUse;

    public GameObject SpawnObject(Vector3 position, Quaternion rotation)
    {
        PooledObject currentObject;

        if (objectsInPool.Count <= 0)
        {
            GameObject newGO = Instantiate(prefab);
            currentObject = newGO.AddComponent<PooledObject>();
            currentObject.pool = this;
        }
        else
        {
            currentObject = objectsInPool[0];
            objectsInPool.Remove(currentObject);
        }

        objectsInUse.Add(currentObject);
        currentObject.gameObject.SetActive(true);
        currentObject.gameObject.transform.position = position;
        currentObject.gameObject.transform.rotation = rotation;

        return currentObject.gameObject;
    }

    public void ReturnToPool(PooledObject objectToReturn)
    {
        if (objectsInPool.Contains(objectToReturn)) { return; }

        objectToReturn.gameObject.SetActive(false);

        objectsInUse.Remove(objectToReturn);
        objectsInPool.Add(objectToReturn);
    }

    public void RemoveObject(PooledObject objectToRemove)
    {
        objectsInPool.Remove(objectToRemove);
        objectsInUse.Remove(objectToRemove);
    }
}

