using UnityEngine;

public class TowerAttackRangeView : MonoBehaviour
{
    [SerializeField] private Material attackRangeViewShader;

    private const string RANGE_REF = "_Range";

    public void SetAttackRange(float attackRange)
    {
        attackRangeViewShader.SetFloat(RANGE_REF, attackRange);
    }
}
