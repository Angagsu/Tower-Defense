using System.Collections;
using UnityEngine;
using UnityEngine.UI;



public class GameplayHeroActiveSkillUI : MonoBehaviour
{
    [SerializeField] private Image activeSkillImage;
    [SerializeField] private Image reloadImage;
    [SerializeField] private Image lockImage;
    [SerializeField] private Button activeSkillButton;

    private BaseHero hero;
    private Coroutine coroutine;
    private float cooldownTimer;
    private float skillCooldown;



    private void Awake()
    {
        reloadImage.gameObject.SetActive(false);
    }

    public void SetHero(HeroSO heroSO, BaseHero hero)
    {
        this.hero = hero;
        activeSkillImage.sprite = heroSO.ActiveSkillSprite;

        skillCooldown = hero.ActiveSkillCooldown;

        if (heroSO.CurrentUpgradeLevel.ActiveSkillAccessibility)
        {
            activeSkillButton.onClick.AddListener(OnClick);

            lockImage.gameObject.SetActive(false);
            activeSkillButton.enabled = true;
        }
        else
        {
            lockImage.gameObject.SetActive(true);
            activeSkillButton.enabled = false;
        }
    }

    private void OnClick()
    {
        if (!hero.IsDead)
        {
            hero.SuperAttack(StartCooldown);
        }
    }
    private void StartCooldown()
    {
        cooldownTimer = skillCooldown;
        coroutine = StartCoroutine(SkillCooldown());
    }

    private IEnumerator SkillCooldown()
    {
        activeSkillButton.enabled = false;
        reloadImage.gameObject.SetActive(true);

        while (cooldownTimer >= 0)
        {
            reloadImage.fillAmount = cooldownTimer / skillCooldown;

            cooldownTimer -= Time.deltaTime;
            yield return null;
        }

        activeSkillButton.enabled = true;
        reloadImage.gameObject.SetActive(false);
        coroutine = null;
    }

    private void OnDisable()
    {
        activeSkillButton.onClick.RemoveListener(OnClick);
    }
}
