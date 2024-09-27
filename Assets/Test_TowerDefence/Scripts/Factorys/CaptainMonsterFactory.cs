using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptainMonsterFactory : MonsterFactory
{
    public override BaseMonster GetFactoryType => factorieType;

    public List<BaseMonster> CaptainMonsters;
    public int Capacity = 3;

    private CaptainMonster factorieType;
    private Pool<BaseMonster> pool;

    private void Awake()
    {
        factorieType = CaptainMonsters[0].GetComponent<CaptainMonster>();
        pool = new Pool<BaseMonster>(CaptainMonsters, Capacity, transform);
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
        for (int i = 0; i < CaptainMonsters.Count; i++)
        {
            if (CaptainMonsters[i].MonsterID == type.MonsterID)
            {
                return (T)pool.GetFreeElement(type, transform, rotation);
            }

        }
        return null;
    }
}
