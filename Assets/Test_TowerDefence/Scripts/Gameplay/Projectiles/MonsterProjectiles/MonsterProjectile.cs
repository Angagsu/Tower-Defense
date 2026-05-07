using System.Collections;
using UnityEngine;

public class MonsterProjectile : BaseProjectile
{
    [SerializeField] private MeshRenderer meshRenderer;

    protected override IEnumerator ImpactEffectDuration()
    {
        meshRenderer.enabled = false;

        yield return waitForSeconds;

        impactEffect.gameObject.SetActive(false);
        isHitted = false;
        meshRenderer.enabled = true;
        gameObject.SetActive(false);
    }
}
