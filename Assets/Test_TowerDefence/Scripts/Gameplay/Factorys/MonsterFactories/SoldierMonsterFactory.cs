using System.Collections.Generic;
using UnityEngine;

public class SoldierMonsterFactory : BaseMonsterFactory
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

    public override T GetMonsterByType<T>(T monster, Transform transform, Quaternion rotation)
    {
        return (T)pool.GetFreeElement(monster, transform, rotation);     
    }
}

   

