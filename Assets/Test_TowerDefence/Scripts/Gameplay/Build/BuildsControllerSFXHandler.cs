using UnityEngine;

public class BuildsControllerSFXHandler : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    [Space(10)]
    [SerializeField] private AudioClip destroyAudioClip;
    [SerializeField] private AudioClip buildAudioClip;
    [SerializeField] private AudioClip upgradeAudioClip;

    private float volume;



    private void Awake()
    {
        volume = audioSource.volume;
    }

    public void PlayBuildSFX()
    {
        audioSource.volume = volume;
        audioSource.PlayOneShot(buildAudioClip);
    }

    public void PlayUpgradeSFX()
    {
        audioSource.volume = volume;
        audioSource.PlayOneShot(upgradeAudioClip);
    }

    public void PlayDestroySFX()
    {
        audioSource.volume = 0.5f;
        audioSource.PlayOneShot(destroyAudioClip);
    }
}
