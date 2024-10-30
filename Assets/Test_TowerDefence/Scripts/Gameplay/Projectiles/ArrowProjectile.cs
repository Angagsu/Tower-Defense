using System.Collections;
using UnityEngine;

public class ArrowProjectile : BaseProjectile
{
    [SerializeField] private MeshRenderer meshRenderer;

    protected override IEnumerator ImpactEffectDuration()
    {
        meshRenderer.enabled = false;

        yield return new WaitForSeconds(0.2f);

        impactEffect.gameObject.SetActive(false);
        isHitted = false;
        meshRenderer.enabled = true;
        gameObject.SetActive(false);
    }
}
