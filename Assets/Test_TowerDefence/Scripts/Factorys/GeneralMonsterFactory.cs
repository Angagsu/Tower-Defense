using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralMonsterFactory : MonsterFactory
{
    public override BaseMonster GetFactoryType { get => factorieType; }

    public List<BaseMonster> GeneralMonsters;
    public int Capacity = 3;

    private GeneralMonster factorieType;
    private Pool<BaseMonster> pool;

    private void Awake()
    {
        factorieType = GeneralMonsters[0].GetComponent<GeneralMonster>();
        pool = new Pool<BaseMonster>(GeneralMonsters, Capacity, transform);
        pool.AutoExpand = true;
    }


    public override T CreatSimpleMonster<T>()
    {
        return base.CreatSimpleMonster<T>();
    }

    public override T CreatSpeedsterMonster<T>()
    {
        return base.CreatSpeedsterMonster<T>();
    }

    public override T CreatArmoredMonster<T>()
    {
        return base.CreatArmoredMonster<T>();
    }

    public override T CreatStrongAttackerMonster<T>()
    {
        return base.CreatStrongAttackerMonster<T>();
    }

    public override T CreatGenerativeMonster<T>()
    {
        return base.CreatGenerativeMonster<T>();
    }

    public override T GetMonsterTypeByID<T>(T type, Transform transform, Quaternion rotation)
    {
        for (int i = 0; i < GeneralMonsters.Count; i++)
        {
            if (GeneralMonsters[i] == type)
            {
                return (T)pool.GetFreeElement(type, transform, rotation);
            }

        }
        return null;
    }
}
