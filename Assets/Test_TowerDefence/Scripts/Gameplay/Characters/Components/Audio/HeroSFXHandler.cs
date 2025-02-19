using System.Collections.Generic;
using UnityEngine;

public class HeroSFXHandler : BaseSFXHandler
{
    [Space(10)]
    [SerializeField] private List<AudioClip> attackAudioClips;
    [Space(10)]
    [SerializeField] private AudioClip dyingAudioClip;
    [SerializeField] private AudioClip revivingAudioClip;
    [SerializeField] private AudioClip superAttackAudioClip;

    public override void PlayAttackSFX()
    {
        audioSource.PlayOneShot(attackAudioClips[Random.Range(0, attackAudioClips.Count)]);
    }

    public override void PlayDyingSFX()
    {
        audioSource.PlayOneShot(dyingAudioClip);
    }

    public override void PlayReviveSFX()
    {
        audioSource.PlayOneShot(revivingAudioClip);
    }

    public override void PlaySuperAttackSFX()
    {
        audioSource.PlayOneShot(superAttackAudioClip);
    }
}
