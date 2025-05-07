using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Test_TowerDefence.Scripts.Monster.Captain
{
    public class GenerativeCaptainMonster : CaptainMonster
    {
        [SerializeField] private List<BaseMonster>  minions;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Update()
        {
            base.Update();
        }

        protected override void Attack(Transform target)
        {
            base.Attack(target);
        }

        protected override void DetectTarget()
        {
            base.DetectTarget();
        }

        protected override void OnDie()
        {
            base.OnDie();
        }

        protected override void Move(Transform target)
        {
            base.Move(target);
        }

        public override void TakeDamage(float amount)
        {
            base.TakeDamage(amount);
        }

        protected override IEnumerator DyingAnimationDuration()
        {
            anim.SetDeadAnimation(isDead);

            yield return new WaitForSeconds(0.7f);
            Movement.SetMinionsPositionAndTarget(minions, transform, transform.rotation);
            yield return new WaitForSeconds(dyingAnimationDuration);
            
            gameObject.SetActive(false);
        }
    }
}