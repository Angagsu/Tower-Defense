using System.Collections;
using UnityEngine;

public class DefenderUnit : BaseHero
{
    [SerializeField] DefendersController defendersController;

    public DefenderMovement DefenderMovement;
    public Vector3 PositionOffset;

    [SerializeField] private Transform startPosition;

    public override void Construct(PlayerInputHandler playerInputHandler, GameplayStates gameplayStates, ProjectilesFactoriesService projectilesFactoriesService)
    {
        base.Construct(playerInputHandler, gameplayStates, projectilesFactoriesService);
    }

    protected override void Awake()
    {
        DefenderMovement = move as DefenderMovement;
        
        isSelected = false;
        canAttack = true;
        health = startHealth;
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

        if (!DefenderMovement.IsMoves && target)
        {
            LockOnTarget();
        }
    }

    protected override void DetectTarget()
    {
        if ((target = detect.DetectTarget(detectionRange, IsDead)))
        {
            var distance = Vector3.Distance(DefenderMovement.PreviousPosition, target.position);

            if (distance < detectionRange && !DefenderMovement.IsMoves && (detect.DetectedMonster.CurrentAttackerHero == null || detect.DetectedMonster.CurrentAttackerHero == this))
            {
                currentTargetedMonster = detect.DetectedMonster;

                if (currentTargetedMonster.CurrentAttackerHero == null)
                {
                    currentTargetedMonster.SetCurrentAttackerHero(this);
                    currentTargetedMonster.Movement.SetIsMove(false);
                    currentTargetedMonster.Anim.StopAllAnimations();
                    DefenderMovement.MoveToTargetMonster(moveSpeed, turnSpeed, target.position);
                }

                LockOnTarget();

                if (canAttack && !currentTargetedMonster.Movement.IsMoves)
                {
                    Anim.SetAttackAnimation(true);

                    if (attackCountdown <= 0)
                    {
                        attackCountdown = 1 / attackRate;

                        Attack(target);
                    }
                }
                else
                {
                    Anim.SetAttackAnimation(false);
                }     
            }
            else
            {
                Anim.SetAttackAnimation(false);

                if (!DefenderMovement.IsMoves && !DefenderMovement.IsOnPreviosPosition)
                {
                    DefenderMovement.ReturnToPreviousPosition(moveSpeed, turnSpeed, DefenderMovement.PreviousPosition);
                }
            }
        }
        else
        {
            Anim.SetAttackAnimation(false);

            if (!DefenderMovement.IsMoves && !DefenderMovement.IsOnPreviosPosition)
            {
                DefenderMovement.ReturnToPreviousPosition(moveSpeed, turnSpeed, DefenderMovement.PreviousPosition);
            }
        }

        attackCountdown -= Time.deltaTime;

    }

    protected override void Attack(Transform target)
    {
        base.Attack(target);
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
        instancingEnabler.gameObject.SetActive(false);
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
    }

    protected override IEnumerator OnRevivingAnimationStarted()
    {
        yield return new WaitForSeconds(reviveAnimationDuration);
        
        health = startHealth;

        if (!DefenderMovement.IsOnPreviosPosition) 
        {
            DefenderMovement.ReturnToPreviousPosition(moveSpeed, turnSpeed, DefenderMovement.PreviousPosition);
        }
        

        while (isDead)
        {
            if (DefenderMovement.IsOnPreviosPosition)
            {
                instancingEnabler.SetMaterialProperty((health / startHealth));
                isDead = false;
                break;
            }

            yield return null;
        }   
    }

    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);
    }

    protected override IEnumerator OnDyingAnimationStarted()
    {
        yield return new WaitForSeconds(dyingAnimationDuration);

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
