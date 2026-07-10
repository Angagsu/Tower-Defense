using Assets.Scripts.Tower;
using System.Collections;
using UnityEngine;

public class DefenderUnit : BaseHero
{
    [SerializeField] DefendersController defendersController;
  
    public DefenderMovement DefenderMovement;
    public Vector3 PositionOffset;

    [SerializeField] private Transform startPosition;

    private DefenderTowerUpgradeLevelSO towerUpgradeLevel;
    private float stunChance;
    private float stunDuration;
    private float[] passiveSkills;

    public override void Construct(PlayerInputHandler playerInputHandler, GameplayStates gameplayStates, ProjectilesFactoriesService projectilesFactoriesService)
    {
        base.Construct(playerInputHandler, gameplayStates, projectilesFactoriesService);
    }

    public void SetDefenderParams(TowerSO defenderTowerSO)
    {
        towerUpgradeLevel = defenderTowerSO.CurrentUpgradeLevel as DefenderTowerUpgradeLevelSO;

        SetupAttackStats(defenderTowerSO.CurrentUpgradeLevel.AttackStats);

       // damage = towerUpgradeLevel.Damage;
       // startHealth = towerUpgradeLevel.TroopsHealth;
       // health = startHealth;

        if (towerUpgradeLevel.PassiveSkillsAccessibility.Capacity != 0)
        {
            hasPassiveSkill = true;
        }
        passiveSkills = new float[] { stunChance, stunDuration };
        passiveSkillsList = new();
        passiveSkillsList.Add(stunChance);
        passiveSkillsList.Add(stunDuration);

        if (hasPassiveSkill)
        {
            for (int i = 0; i < towerUpgradeLevel.PassiveSkillsAccessibility.Count; i++)
            {
                passiveSkillsList[i] = towerUpgradeLevel.PassiveSkillsValues[i];
            }
        }
    }

    protected override void SetupAttackStats(AttackStats attackStats)
    {
        AttackStats.Health = attackStats.Health;
        AttackStats.MaxHealth = attackStats.MaxHealth;

        AttackStats.Armor = attackStats.Armor;
        AttackStats.MaxArmor = attackStats.MaxArmor;

        AttackStats.PhysicalResistance = attackStats.PhysicalResistance;
        AttackStats.MagicalResistance = attackStats.MagicalResistance;

        AttackStats.DamageData = attackStats.DamageData;

        physicResistStartValue = AttackStats.PhysicalResistance;
        magicResistStartValue = AttackStats.MagicalResistance;
    }

    protected override void Awake()
    {
        DefenderMovement = move as DefenderMovement;
        
        isSelected = false;
        canAttack = true;
        health = startHealth;

        waitUntilRevive = new WaitForSeconds(ReviveTimer);
        waitUntilReviveAnimationEnd = new WaitForSeconds(reviveAnimationDuration);
        waitUntilDyingAnimationEnd = new WaitForSeconds(dyingAnimationDuration);
    }

    protected override void OnEnable()
    {
        defendersController.DefendersPositionChanged += OnDefendersLocationChanched;
    }

    protected override void OnDisable()
    {
        defendersController.DefendersPositionChanged -= OnDefendersLocationChanched;
        gameplayStates.Paused -= OnGameplayPause;
        gameplayStates.Unpaused -= OnGameplayUnpause;

        if (currentTargetedMonster != null)
        {
            currentTargetedMonster.RejectCurrentAttackerHero();
        }
    }

    private void OnDefendersLocationChanched(Vector3 newPosition)
    {
        if (!isDead)
        {
            DefenderMovement.MoveToTarget(moveSpeed, turnSpeed, newPosition, startPosition);
        }
    }

    protected override void Update()
    {
        if (isPaused) return;

        if (!DefenderMovement.IsMoves && !IsDead)
        {
            DetectTarget();
        }
    }

    protected override void DetectTarget()
    {
        if ((target = detect.DetectTarget(detectionRange, IsDead)))
        {
            var distance = (DefenderMovement.PreviousPosition - target.position).sqrMagnitude;

            if (distance < (detectionRange * detectionRange) && !DefenderMovement.IsMoves && !detect.DetectedMonster.IsKamikaze)
            {
                currentTargetedMonster = detect.DetectedMonster;

                if (currentTargetedMonster.CurrentAttackerHero == null)
                {
                    currentAttackerMonster = currentTargetedMonster;

                    currentTargetedMonster.SetCurrentAttackerHero(this);
                    currentTargetedMonster.SetCurrentTargetedHero();

                    currentTargetedMonster.Movement.SetIsMove(false);
                    currentTargetedMonster.Anim.StopAllAnimations();
                    currentTargetedMonster.SetCanAttack(false);
                    DefenderMovement.MoveToTargetMonster(moveSpeed, turnSpeed, target.position);
                }

                LockOnTarget();

                if (canAttack)
                {
                    currentAttackerMonster.SetCanAttack(true);
                    attackCountdown -= Time.deltaTime;

                    if (attackCountdown <= 0)
                    {
                        attackCountdown = attackRate;

                        Anim.SetAttackAnimation(true);
                    }
                }
                else
                {

                    Anim.SetAttackAnimation(false);
                }     
            }
            else
            {
                if (currentTargetedMonster != null)
                {
                    currentTargetedMonster.RejectCurrentAttackerHero();
                }

                Anim.SetAttackAnimation(false);

                if (!DefenderMovement.IsMoves && !DefenderMovement.IsOnPreviosPosition)
                {
                    DefenderMovement.ReturnToPreviousPosition(moveSpeed, turnSpeed, DefenderMovement.PreviousPosition);
                }
            }
        }
        else
        {
            if (currentTargetedMonster != null)
            {
                currentTargetedMonster.RejectCurrentAttackerHero();
            }

            Anim.SetAttackAnimation(false);

            if (!DefenderMovement.IsMoves && !DefenderMovement.IsOnPreviosPosition)
            {
                DefenderMovement.ReturnToPreviousPosition(moveSpeed, turnSpeed, DefenderMovement.PreviousPosition);
            }
        }
    }


    protected override void Attack(Transform target)
    {
        attack.AttackWithPassiveSkill(AttackStats, target, damage, hasPassiveSkill, passiveValue_1: passiveSkillsList[0],
            passiveValue_2: passiveSkillsList[1], currentTargetedMonster, currentTargetedMonster);

        baseSFX.PlayAttackSFX();
    }

    protected override void Move(Transform target) { }

    public override void OnRevive()
    {
        Anim.SetDeadAnimation(false);

        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        coroutine = StartCoroutine(OnRevivingAnimationStarted());
    }

    protected override void OnDie()
    {
        healthBarInstance.gameObject.SetActive(false);
        isDead = true;
        Anim.SetAttackAnimation(false);
        Anim.SetMoveAnimation(false);
        Anim.SetDeadAnimation(true);
        

        if (currentTargetedMonster)
        {
            currentTargetedMonster.RejectCurrentAttackerHero();
            currentTargetedMonster = null;
            target = null;
        }

        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        coroutine = StartCoroutine(OnDyingAnimationStarted());

        if (buffCoroutine != null)
        {
            StopCoroutine(buffCoroutine);
        }

        SetDefaultResistence();
    }

    public void OnTowerCrush(Vector3 previousPosition)
    {
        healthBarInstance.gameObject.SetActive(false);
        isDead = true;
        Anim.SetAttackAnimation(false);
        Anim.SetMoveAnimation(false);
        Anim.SetDeadAnimation(true);

        if (currentTargetedMonster)
        {
            currentTargetedMonster.RejectCurrentAttackerHero();
            currentTargetedMonster = null;
            target = null;
        }

        if (buffCoroutine != null)
        {
            StopCoroutine(buffCoroutine);
        }

        SetDefaultResistence();

        var defenderNewPosition = new Vector3(startPosition.position.x, transform.position.y, startPosition.position.z);
        transform.position = defenderNewPosition;

        DefenderMovement.SetPreviousPosition(previousPosition);
    }

    public void OnTowerRepair()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(OnTowerRepairCoroutine());
    }

    private IEnumerator OnTowerRepairCoroutine()
    {
        Anim.SetDeadAnimation(false);

        health = startHealth;
        AttackStats.Health = AttackStats.MaxHealth;
        healthBarInstance.SetMaterialProperty((AttackStats.Health / AttackStats.MaxHealth));

        if (isDead)
        {
            if (!DefenderMovement.IsOnPreviosPosition)
            {
                DefenderMovement.ReturnToPreviousPosition(moveSpeed, turnSpeed, DefenderMovement.PreviousPosition);
            }

            DefenderMovement.MoveToTarget(moveSpeed, turnSpeed, DefenderMovement.PreviousPosition, startPosition);
        }

        while (isDead)
        {
            if (DefenderMovement.IsOnPreviosPosition)
            {
                isDead = false;
                break;
            }

            yield return null;
        }
    }

    protected override IEnumerator OnRevivingAnimationStarted()
    {
        yield return waitUntilReviveAnimationEnd;
        
        health = startHealth;
        AttackStats.Health = AttackStats.MaxHealth;

        if (!DefenderMovement.IsOnPreviosPosition) 
        {
            DefenderMovement.ReturnToPreviousPosition(moveSpeed, turnSpeed, DefenderMovement.PreviousPosition);
        }
        

        while (isDead)
        {
            if (DefenderMovement.IsOnPreviosPosition)
            {
                healthBarInstance.SetMaterialProperty((AttackStats.Health / AttackStats.MaxHealth));
                isDead = false;
                break;
            }

            yield return null;
        }   
    }

    protected override IEnumerator OnDyingAnimationStarted()
    {
        yield return waitUntilDyingAnimationEnd;

        var defenderNewPosition = new Vector3(startPosition.position.x, transform.position.y, startPosition.position.z);
        transform.position = defenderNewPosition;

        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        coroutine = StartCoroutine(TimerBeforeRevive());
    }

    protected override IEnumerator TimerBeforeRevive()
    {
        return base.TimerBeforeRevive();
    }

    public override void Select() { }
    
    public override void Deselect() { }
}
