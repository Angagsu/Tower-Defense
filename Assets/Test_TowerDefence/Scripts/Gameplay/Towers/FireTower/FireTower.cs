using Assets.Scripts.Tower;
using UnityEngine;

public class FireTower : BaseTower
{
    [SerializeField] private float impactRadius;

    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Update()
    {
        base.Update();
    }

    public override void ReBuild()
    {
        base.ReBuild();
    }

    public override void TakeDamage(float damageAmount)
    {
        base.TakeDamage(damageAmount);
    }

    protected override void AttackTarget(Transform target)
    {
        base.AttackTarget(target);
    }

    protected override void Crash()
    {
        base.Crash();
    }

    protected override void DetectTarget()
    {
        base.DetectTarget();
    }
}
