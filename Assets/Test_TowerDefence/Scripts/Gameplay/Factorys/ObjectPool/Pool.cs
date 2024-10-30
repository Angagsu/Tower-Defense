using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class Pool<T> where T : MonoBehaviour
{
    public event Action<T> PoolChanged; 
    public List<T> Prefabs { get; }
    public bool AutoExpand { get; set; }
    public Transform Container { get; }

    private List<T> pool;


    public Pool(List<T> prefabs, int capacity)
    {
        Prefabs = prefabs;
        Container = null;

        CreatPool(capacity);
    }

    public Pool(List<T> prefabs, int capacity, Transform container)
    {
        Prefabs = prefabs;
        Container = container;

        CreatPool(capacity);
    }

    private void CreatPool(int count)
    {
        pool = new List<T>();

       // for (int i = 0; i < count; i++)
       // {
       //     CreatObject(false, Container, Container.transform.rotation);
       // }
    }

    private T CreatObject(T type, bool isActiveByDefault = false, Transform transform = default, Quaternion rotation = default) 
    {
        foreach (var item in Prefabs)
        {
            if (item.GetType() == type.GetType())
            {
                var createdObject = Object.Instantiate(item, transform.position, rotation, Container);
                createdObject.gameObject.SetActive(isActiveByDefault);
                pool.Add(createdObject);
                PoolChanged?.Invoke(createdObject);
                return createdObject;
            }
        }

        Debug.Log("The prefab is not founded !!!");
        return null;
    }

    public bool HasFreeElemnt(out T element, T type, Vector3 position, Quaternion rotation)
    {
        foreach (var mono in pool)
        {
            if (!mono.gameObject.activeInHierarchy && mono.GetType() == type.GetType())
            {
                element = mono;
                element.transform.SetPositionAndRotation(position, rotation);
                mono.gameObject.SetActive(true);
                return true;
            }
        }

        element = null;
        return false;
    }

    public T GetFreeElement(T type, Transform transform, Quaternion rotation)
    {
        if (HasFreeElemnt(out var element, type, transform.position, rotation))
        {
            return element;
        }

        if (AutoExpand)
        {
            return CreatObject(type, true, transform, rotation);
        }

        throw new Exception($"There is no free elements in pool of type {typeof(T)}");
    }
}
