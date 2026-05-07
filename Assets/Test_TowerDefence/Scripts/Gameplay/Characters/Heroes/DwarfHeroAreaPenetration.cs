using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DwarfHeroAreaPenetration : MonoBehaviour
{
    private List<BaseMonster> monsters;

    private DetectionHelper detectionHelper;

    private WaitForSeconds waitForSeconds;
    private Coroutine coroutine;
    private BaseHero hero;

    private float impactRadius;
    private float passivePenetration;



    private void Start()
    {
        detectionHelper = DetectionHelper.Instance;
        monsters = detectionHelper.Monsters;
        waitForSeconds = new WaitForSeconds(1f);
        
    }
    
    public void SetHeroStats(BaseHero hero, float passivePenetration, float impactRadius)
    {
        this.hero = hero;
        this.passivePenetration = passivePenetration;
        this.impactRadius = impactRadius;

        hero.Died += OnHero_Died;

        coroutine = StartCoroutine(Penetrate(hero.AttackStats.DamageData.DamageType, passivePenetration, impactRadius));
    }

    private void OnDestroy()
    {
        if (hero)
        {
            hero.Died -= OnHero_Died;
        }
    }

    private void OnHero_Died(bool isDied)
    {
        if (isDied)
        {
            StopCoroutine(coroutine);
            ResetResistanceToDefault();
        }
        else
        {
           coroutine = StartCoroutine(Penetrate(hero.AttackStats.DamageData.DamageType, passivePenetration, impactRadius));
        }
    }

    private IEnumerator Penetrate(DamageType damageType, float passivePenetration, float impactRadius)
    {
        while (true)
        {
            yield return waitForSeconds;

            ReduceResistance(damageType, passivePenetration);
        }
    }

    private void ReduceResistance(DamageType damageType, float penetration)
    {
        foreach (var monster in monsters)
        {
            if (monster.isActiveAndEnabled)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, monster.transform.position);

                if (distanceToEnemy <= impactRadius)
                {
                    monster.ReduceResistance(damageType, penetration);
                }
                else
                {
                    monster.SetDefaultResistence();
                }               
            }
        }
    }

    private void ResetResistanceToDefault()
    {
        foreach (var monster in monsters)
        {
            if (monster.isActiveAndEnabled)
            {
                monster.SetDefaultResistence();
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, impactRadius);
    }
}
