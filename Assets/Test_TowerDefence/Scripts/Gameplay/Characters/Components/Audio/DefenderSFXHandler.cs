using System.Collections.Generic;
using UnityEngine;

public class DefenderSFXHandler : BaseSFXHandler
{
    [Space(10)]
    [SerializeField] private List<AudioClip> attackAudioClips;
    [Space(10)]
    [SerializeField] private AudioClip dyingAudioClip;
    [SerializeField] private AudioClip revivingAudioClip;

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
}
