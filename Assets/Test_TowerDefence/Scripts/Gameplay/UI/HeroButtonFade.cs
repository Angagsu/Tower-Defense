using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HeroButtonFade : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private Button heroButton;

    private BaseHero hero;
    private float startReviveTimer;
    private float reviveTimer;
    private float dyingAnimationDuration = 3;
    private float revivingAnimationDuration = 4.5f;

    private void Awake()
    {
        fadeImage.gameObject.SetActive(false);
    }

    public void SetHero(BaseHero hero)
    {
        this.hero = hero;
        this.hero.Died += OnHeroDied;

        startReviveTimer = hero.ReviveTimer + dyingAnimationDuration + revivingAnimationDuration;
    }

    private void OnHeroDied()
    {
        reviveTimer = startReviveTimer;
        heroButton.enabled = false;
        fadeImage.gameObject.SetActive(true);
        StartCoroutine(FadeTimer());
    }

    private IEnumerator FadeTimer()
    {
        while (reviveTimer >= 0)
        {
            fadeImage.fillAmount = reviveTimer / startReviveTimer;

            reviveTimer -= Time.deltaTime;
            yield return null;
        }

        heroButton.enabled = true;
        fadeImage.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        hero.Died -= OnHeroDied;
    }
}
