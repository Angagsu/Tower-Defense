using System.Collections;
using UnityEngine;
using UnityEngine.UI;



public class GameplayHeroActiveSkillUI : MonoBehaviour
{
    [SerializeField] private Image activeSkillImage;
    [SerializeField] private Image reloadImage;
    [SerializeField] private Image lockImage;
    [SerializeField] private Image selectImage;
    [SerializeField] private Button activeSkillButton;

    private GameplayHeroesButtonsController heroesButtonsController;
    private BaseHero hero;
    private Coroutine coroutine;

    private float cooldownTimer;
    private float skillCooldown;
    public bool IsSelected { get; private set; }
    public int ID { get; private set; }


    private void Awake()
    {
        reloadImage.gameObject.SetActive(false);
    }

    public void Setup(GameplayHeroesButtonsController heroesButtonsController, HeroSO heroSO, BaseHero hero, int index)
    {
        this.heroesButtonsController = heroesButtonsController;
        this.hero = hero;
        this.ID = index;
        activeSkillImage.sprite = heroSO.ActiveSkillSprite;

        skillCooldown = hero.ActiveSkillCooldown;

        if (heroSO.CurrentUpgradeLevel.ActiveSkillAccessibility)
        {
            activeSkillButton.onClick.AddListener(OnClick);

            lockImage.gameObject.SetActive(false);
            activeSkillButton.enabled = true;
            hero.CanUseSuperAttack += CanUseSuperAttack;
            hero.SetActiveSkillButtonUI(this);
        }
        else
        {
            lockImage.gameObject.SetActive(true);
            activeSkillButton.enabled = false;
        }
    }

    private void OnClick()
    {
        heroesButtonsController.SetCurrentSelectedHeroActiveSkillButton(this);  
    }

    public void StartCooldown()
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

    private void CanUseSuperAttack(bool canUse)
    {
        if (canUse)
        {
            activeSkillButton.interactable = true;
        }
        else
        {
            activeSkillButton.interactable = false;
            Deselect();
        }
    }

    public void Select()
    {
        IsSelected = true;
        selectImage.gameObject.SetActive(true);
    }

    public void Deselect()
    {
        IsSelected = false;
        selectImage.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        activeSkillButton.onClick.RemoveListener(OnClick);
        hero.CanUseSuperAttack -= CanUseSuperAttack;
    }
}
