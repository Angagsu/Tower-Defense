using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameplayHeroButton : MonoBehaviour
{
    [SerializeField] private Image heroImage;
    [SerializeField] private Image healthBarImage;
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

    public void SetHero(HeroSO heroSO, BaseHero hero)
    {
        this.hero = hero;
        heroImage.sprite = heroSO.Sprite;

        hero.Died += OnHeroDied;
        hero.DamageTaked += OnHeroDamageTaked;
        
        startReviveTimer = hero.ReviveTimer + dyingAnimationDuration + revivingAnimationDuration;
    }

    private void OnHeroDamageTaked(BaseHero hero, float health, float startHealth)
    {
        healthBarImage.fillAmount = health / startHealth;
    }

    private void OnHeroDied(bool isDied)
    {
        if (!isDied) return;

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
        hero.DamageTaked -= OnHeroDamageTaked;
    }
}
