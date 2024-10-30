using Assets.Scripts.Tower;
using UnityEngine;

[CreateAssetMenu(fileName = "Tower", menuName = "SO/TowerSO/Tower")]
public class TowerSO : ScriptableObject
{
    [field: SerializeField] public GameObject TowerPrefab { get; private set; }
    [field: SerializeField] public int Cost { get; private set; }
}
