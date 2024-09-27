using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonsterFactory : MonsterFactory
{
    public override BaseMonster GetFactoryType => factorieType;

    public List<BaseMonster> BossMonsters;
    public int Capacity = 3;

    private BossMonster factorieType;
    private Pool<BaseMonster> pool;

    private void Awake()
    {
        factorieType = BossMonsters[0].GetComponent<BossMonster>();
        pool = new Pool<BaseMonster>(BossMonsters, Capacity, transform);
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

    public override T GetMonsterTypeByID<T>(T monster, Transform transform, Quaternion rotation)
    {
        for (int i = 0; i < BossMonsters.Count; i++)
        {
            if (BossMonsters[i] == monster)
            {
                return (T)pool.GetFreeElement(monster, transform, rotation);
            }
        }

        return null;
    }
}
