using UnityEngine;


public class KnightSuperAttack : HeroSuperAttack
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

    public override void AreaEffect(AttackStats stats, float areaRadius, float passiveEffectDuration = 0, float passiveEffect_2 = 0)
    {
        superAttackRadius = areaRadius;
        ActivateSuperAttackEffects();
        ApplyMassiveDamage(stats, areaRadius, passiveEffectDuration, superAttackValue);
        isSuperAttackStarted = false;
    }

    protected override void ApplyMassiveDamage(AttackStats attackStats, float areaRadius, float stunDuration, float damage)
    {
        DetectMonsters(areaRadius);
        MassiveDamageWithStunEffect(attackStats, stunDuration, damage);
    }

    private void MassiveDamageWithStunEffect(AttackStats attackStats, float stunDuration, float damage)
    {
        foreach (var monster in affectableMonsters)
        {
            var finalDamage = DoDamage.CalculateDamage(monster.AttackStats, attackStats.DamageData, damage);

            monster.TakePureDamage(finalDamage);
            monster.GetStun(stunDuration);
        }

        affectableMonsters.Clear();
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
        playerInputHandler.TouchPressed -= OnPlayerInput_TouchPressed;
        playerInputHandler.TouchedGround -= OnPlayerInput_TouchedGround;
        baseHero.CanUseSuperAttack -= OnCanUseSuperAttack;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, superAttackRadius);
    }
}
