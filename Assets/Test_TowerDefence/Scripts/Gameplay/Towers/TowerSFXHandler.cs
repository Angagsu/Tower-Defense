using System.Collections.Generic;
using UnityEngine;

public class TowerSFXHandler : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;    
    [Space(10)]
    [SerializeField] private List<AudioClip> attackAudioClips;
    [Space(10)]
    [SerializeField] private AudioClip superAttackAudioClip;

    public  void PlayAttackSFX()
    {
        audioSource.PlayOneShot(attackAudioClips[Random.Range(0, attackAudioClips.Count)]);
    }

    public  void PlaySuperAttackSFX()
    {
        audioSource.PlayOneShot(superAttackAudioClip);
    }
}
