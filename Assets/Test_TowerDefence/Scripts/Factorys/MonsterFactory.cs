using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterFactory : MonoBehaviour
{
    public virtual BaseMonster GetFactoryType { get; private set; }

    public virtual T CreatSimpleMonster<T>() where T : BaseMonster { return default; } 
    public virtual T CreatSpeedsterMonster<T>() where T : BaseMonster { return default; }
    public virtual T CreatArmoredMonster<T>() where T : BaseMonster { return default; }
    public virtual T CreatStrongAttackerMonster<T>() where T : BaseMonster { return default; }
    public virtual T CreatGenerativeMonster<T>() where T : BaseMonster { return default; }

    public virtual T GetMonsterTypeByID<T>(T type, Transform transform, Quaternion rotation) where T : BaseMonster { return default; }
}
