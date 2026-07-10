using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonstersByType", menuName = "SO/MonsterSO/MonsterByType")]
public class MonstersByTypeSO : ScriptableObject
{
    [field: SerializeField] public List<MonsterSO> Monsters { get; private set; }
}
