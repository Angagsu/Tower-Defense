using System.Collections;
using UnityEngine;

public class FireBallProjectile : BaseProjectile
{
    protected override IEnumerator ImpactEffectDuration()
    {
        yield return new WaitForSeconds(0.2f);

        impactEffect.gameObject.SetActive(false);
        isHitted = false;
        gameObject.SetActive(false);
    }
}
