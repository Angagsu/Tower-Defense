using UnityEngine;

public abstract class BaseSFXHandler : MonoBehaviour
{
    [SerializeField] protected AudioSource audioSource;

    public virtual void PlayAttackSFX() { }

    public virtual void StopSFX() { }

    public virtual void PauseSFX() { }

    public virtual void UnPauseSFX() { }

    public virtual void PlaySuperAttackSFX() { }

    public virtual void PlayReviveSFX() { }

    public virtual void PlayDyingSFX() { }
}
