using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Monster", menuName = "SO/MonsterSO/Monster")]
public class MonsterSO : ScriptableObject
{
    [field: SerializeField] public MonsterType Type {  get; private set; }
    [field: SerializeField] public MonsterUniqueSkillType UniqueSkillType {  get; private set; }
    [field: SerializeField] public BaseMonster Monster {  get; private set; }
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Sprite Sprite { get; private set; }

    [field: SerializeField] public float MoveSpeed { get; private set; }
    [field: SerializeField] public float AttackRange { get; private set; }
    [field: SerializeField] public List<float> StatsValues { get; private set; }

    public AttackStats Stats { get => Monster.AttackStats; }

    [field: SerializeField] public string UniqueSkillDescription {  get; private set; }
}
