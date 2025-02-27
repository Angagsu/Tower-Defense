﻿using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Tower
{
    public class ArrowTower : BaseTower
    {

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
}