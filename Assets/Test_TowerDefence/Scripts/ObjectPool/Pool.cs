using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class Pool<T> where T : MonoBehaviour
{
    public List<T> Prefab { get; }
    public bool AutoExpand { get; set; }
    public Transform Container { get; }

    private List<T> pool;


    public Pool(List<T> prefabs, int capacity)
    {
        Prefab = prefabs;
        Container = null;

        CreatPool(capacity);
    }

    public Pool(List<T> prefabs, int capacity, Transform container)
    {
        Prefab = prefabs;
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

    private T CreatObject(T monster, bool isActiveByDefault = false, Transform transform = default, Quaternion rotation = default) 
    {
        for (int i = 0; i < Prefab.Count; i++)
        {
            Prefab[i].TryGetComponent(out BaseMonster currentMonster);
            monster.TryGetComponent(out BaseMonster BaseMonster);

            if (currentMonster.MonsterID == BaseMonster.MonsterID)
            {
                var createdObject = Object.Instantiate(Prefab[i], transform.position, rotation, Container);
                createdObject.gameObject.SetActive(isActiveByDefault);
                pool.Add(createdObject);
                return createdObject;
            }
        }
        Debug.Log("The prefabs not match");
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

    public T GetFreeElement(T monster, Transform transform, Quaternion rotation)
    {
        if (HasFreeElemnt(out var element, monster, transform.position, rotation))
        {
            return element;
        }

        if (AutoExpand)
        {
            return CreatObject(monster, true, transform, rotation);
        }

        throw new Exception($"There is no free elements in pool of type {typeof(T)}");
    }
}
