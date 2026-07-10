using System;
using UnityEngine;

public class ArcherSuperAttack : HeroSuperAttack
{
    [SerializeField] SuperAttackProjectile fireProjectile;

    private SuperAttackProjectile projectile;
    private Camera mainCamera;
    private GameplayHeroActiveSkillUI activeSkillUI;
    LayerMask movableAreaLayer;

    public override void SetDependencies(BaseHero baseHero, PlayerInputHandler playerInputHandler)
    {
        base.SetDependencies(baseHero, playerInputHandler);

        this.playerInputHandler.TouchPressed += OnPlayerInput_TouchPressed;
        this.playerInputHandler.TouchedGround += OnPlayerInput_TouchedGround;
        this.baseHero.CanUseSuperAttack += OnCanUseSuperAttack;

        mainCamera = Camera.main;
        
        projectile = Instantiate(fireProjectile, transform.position, Quaternion.identity);
        projectile.Construct(baseHero.GameplayStates);

        bool hasPassiveSkill = baseHero.UpgradeLevel.PassiveSkillsAccessibility.Count > 0 ? true : false;

        projectile.Setup(baseHero, hasPassiveSkill);
        projectile.gameObject.SetActive(false);

        movableAreaLayer = LayerMask.GetMask("MovableArea");
    }

    private void OnCanUseSuperAttack(bool canUseSuperAttack)
    {
        if (!canUseSuperAttack)
        {
            isSuperAttackStarted = false;
        }
    }

    public override void OnSuperAttackPerformed(GameplayHeroActiveSkillUI activeSkillUI)
    {
        if (this.activeSkillUI == null)
        {
            this.activeSkillUI = activeSkillUI;
        }
    }

    private void OnPlayerInput_TouchedGround(Vector2 vector)
    {
        isSuperAttackStarted = false;
    }

    private void OnPlayerInput_TouchPressed(Vector2 touchPosition)
    {
        if (isSuperAttackStarted)
        {
            Ray ray = mainCamera.ScreenPointToRay(touchPosition);

            if (Physics.Raycast(ray, out RaycastHit raycastHit, 150f, movableAreaLayer))
            {
                projectile.Activate(baseHero.transform.position);
                projectile.SetTarget(raycastHit.point);
                activeSkillUI.StartCooldown();
                activeSkillUI.Deselect();
            }

            activeSkillUI.Deselect();
            isSuperAttackStarted = false;
        }
        else if (activeSkillUI)
        {
            activeSkillUI.Deselect();
        }  
    }

    private void OnDisable()
    {
        playerInputHandler.TouchPressed -= OnPlayerInput_TouchPressed;
        playerInputHandler.TouchedGround -= OnPlayerInput_TouchedGround;
        baseHero.CanUseSuperAttack -= OnCanUseSuperAttack;
    }
}
