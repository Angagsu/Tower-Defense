using System;
using UnityEngine;

public class ArcherSuperAttack : HeroSuperAttack
{
    [SerializeField] SuperAttackProjectile fireProjectile;

    private SuperAttackProjectile projectile;
    private Camera mainCamera;
    private Action superAttackPerformed;


    public override void SetDependencies(BaseHero baseHero, PlayerInputHandler playerInputHandler)
    {
        base.SetDependencies(baseHero, playerInputHandler);

        this.playerInputHandler.TouchPressed += OnPlayerInput_TouchPressed;
        this.playerInputHandler.TouchedGround += OnPlayerInput_TouchedGround;
        mainCamera = Camera.main;
        
        projectile = Instantiate(fireProjectile, transform.position, Quaternion.identity);
        projectile.Construct(baseHero.GameplayStates);

        bool hasPassiveSkill = baseHero.UpgradeLevel.PassiveSkillsAccessibility.Count > 0 ? true : false;

        projectile.Setup(baseHero, hasPassiveSkill);
        projectile.gameObject.SetActive(false);
    }

    public override void OnSuperAttackPerformed(Action superAttackPerformed)
    {
        if (this.superAttackPerformed == null)
        {
            this.superAttackPerformed = superAttackPerformed;
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

            if (Physics.Raycast(ray, out RaycastHit raycastHit) && raycastHit.collider)
            {
                projectile.Activate(baseHero.transform.position);
                projectile.SetTarget(raycastHit.point);
                superAttackPerformed?.Invoke();
            }

            isSuperAttackStarted = false;
        }
    }

    private void OnDisable()
    {
        playerInputHandler.TouchPressed -= OnPlayerInput_TouchPressed;
        playerInputHandler.TouchedGround -= OnPlayerInput_TouchedGround;
    }
}
