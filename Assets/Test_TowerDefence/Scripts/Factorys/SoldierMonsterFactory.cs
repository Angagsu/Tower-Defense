using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierMonsterFactory : MonsterFactory
{
    public override BaseMonster GetFactoryType { get => factorieType; }

    public List<BaseMonster> SoldierMonsters;
    public int Capacity = 3;

    private SoldierMonster factorieType;
    private Pool<BaseMonster> pool;

    
    private void Awake()
    {
        factorieType = SoldierMonsters[0].GetComponent<SoldierMonster>();
        pool = new Pool<BaseMonster>(SoldierMonsters, Capacity, transform);
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
        for (int i = 0; i < SoldierMonsters.Count; i++)
        {
            if (SoldierMonsters[i].MonsterID == monster.MonsterID)
            {
                return (T)pool.GetFreeElement(monster, transform, rotation);
            }

        }
        return null;
    }
}

   

