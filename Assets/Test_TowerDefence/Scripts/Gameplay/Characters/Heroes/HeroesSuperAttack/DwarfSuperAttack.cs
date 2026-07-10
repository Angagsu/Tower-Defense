using UnityEngine;

public class DwarfSuperAttack : HeroSuperAttack
{
    [SerializeField] private ParticleSystem[] superAttackEffects;

    private GameplayHeroActiveSkillUI activeSkillUI;

    private float superAttackRadius = 10f;

    public override void SetDependencies(BaseHero baseHero, PlayerInputHandler playerInputHandler)
    {
        base.SetDependencies(baseHero, playerInputHandler);

        this.playerInputHandler.TouchPressed += OnPlayerInput_TouchPressed;
        this.playerInputHandler.TouchedGround += OnPlayerInput_TouchedGround;
        this.baseHero.CanUseSuperAttack += OnCanUseSuperAttack;
    }

    private void OnCanUseSuperAttack(bool canUseSuperAttack)
    {
        if (!canUseSuperAttack)
        {
            isSuperAttackStarted = false;
        }
    }
    private void OnPlayerInput_TouchedGround(Vector2 vector)
    {
        isSuperAttackStarted = false;
    }
    public override void OnSuperAttackPerformed(GameplayHeroActiveSkillUI activeSkillUI)
    {
        if (this.activeSkillUI == null)
        {
            this.activeSkillUI = activeSkillUI;
        }
    }

    private void OnPlayerInput_TouchPressed(Vector2 touchPosition)
    {
        if (isSuperAttackStarted)
        {
            activeSkillUI.Deselect();
            isSuperAttackStarted = false;
            baseHero.SetCanAttack(true);
        }
        else if (activeSkillUI)
        {
            activeSkillUI.Deselect();
        }
    }

    public override void AreaEffect(AttackStats stats, float areaRadius, float effectDuration = 0, float effectValue = 0)
    {
        superAttackRadius = areaRadius;
        ActivateSuperAttackEffects();
        ApplyMassiveBuff(stats, areaRadius, effectDuration, superAttackValue);
        isSuperAttackStarted = false;
    }

    protected override void ApplyMassiveBuff(AttackStats stats, float areaRadius, float effectDuration, float effectValue)
    {
        DetectHeroes(areaRadius);
        IncreaseAlliesResistance(stats, effectDuration, effectValue);
    }

    private void IncreaseAlliesResistance(AttackStats stats, float effectDuration, float effectValue)
    {
        foreach (BaseHero hero in heroes)
        {
            hero.IncreaseResistance(stats.DamageData.DamageType, effectValue, effectDuration);
        }

        affectableHeroes.Clear();
    }

    private void ActivateSuperAttackEffects()
    {
        foreach (var effect in superAttackEffects)
        {
            effect.Play();
        }
    }

    private void OnDestroy()
    {
        this.playerInputHandler.TouchPressed -= OnPlayerInput_TouchPressed;
        this.playerInputHandler.TouchedGround -= OnPlayerInput_TouchedGround;
        this.baseHero.CanUseSuperAttack -= OnCanUseSuperAttack;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, superAttackRadius);
    }
}
