using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Test_TowerDefence.Scripts.Monster.Captain
{
    public class GenerativeCaptainMonster : CaptainMonster
    {
        [SerializeField] private List<BaseMonster>  minions;


        protected override IEnumerator DyingAnimationDuration()
        {
            anim.SetDeadAnimation(isDead);

            yield return new WaitForSeconds(0.7f);

            while (isPaused) yield return null;

            Movement.SetMinionsPositionAndTarget(minions, transform, transform.rotation);

            yield return new WaitForSeconds(dyingAnimationDuration);
            
            gameObject.SetActive(false);
        }
    }
}