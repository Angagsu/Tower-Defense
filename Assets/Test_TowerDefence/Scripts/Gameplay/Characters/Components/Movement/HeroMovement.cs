using System.Collections;
using UnityEngine;


public class HeroMovement : BaseMovement
{
    [SerializeField] private BaseHero hero;

    private Camera mainCamera;
    private int groundLayer;
    private Coroutine coroutine;
    private CharacterController characterController;

    public Vector3 LockedPosition {  get; private set; }
    public Vector3 NewPosition { get; private set; }
    public Vector3 PreviousPosition { get; private set; }

    public bool IsOnPreviosPosition;
    public bool IsOnLockedPosition;

    private bool isPaused => hero.IsPaused;

    

    private void Awake()
    {
        isMoves = false;
        mainCamera = Camera.main;
        characterController = GetComponent<CharacterController>();
        groundLayer = LayerMask.NameToLayer("Ground");
        PreviousPosition = NewPosition = transform.position;
        IsOnPreviosPosition = true;
    }

    public void MoveToTarget(float moveSpeed, float turnSpeed, Vector2 touchPosition)
    {
        if (hero.IsSelected && !hero.IsDead)
        {
            Ray ray = mainCamera.ScreenPointToRay(touchPosition);

            if (Physics.Raycast(ray, out RaycastHit raycastHit) && raycastHit.collider &&
                raycastHit.collider.gameObject.layer.CompareTo(groundLayer) == 0)
            {
                if (coroutine != null)
                {
                    StopCoroutine(coroutine);
                }
                coroutine = StartCoroutine(HeroMoveTowerds(raycastHit.point, moveSpeed, turnSpeed));
                NewPosition = raycastHit.point;
            }
        }
    }

    public void MoveToTargetMonster(float moveSpeed, float turnSpeed, Vector3 targetPosition)
    {
        if (IsMoves)
        {
            return;
        }

        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        
        coroutine = StartCoroutine(MoveToTargetedMonster(hero, targetPosition, moveSpeed, turnSpeed));      
    }

    public void ReturnToPreviousPosition(float moveSpeed, float turnSpeed, Vector3 targetPosition)
    {
        if (IsOnPreviosPosition)
        {
            return;
        }

        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        
        coroutine = StartCoroutine(GoToPreviousPosition(hero, targetPosition, moveSpeed, turnSpeed));
    }

    private IEnumerator HeroMoveTowerds(Vector3 targetPosition, float moveSpeed, float turnSpeed)
    {
        isMoves = true;
        hero.Deselect();
        hero.SetCanAttack(false);
        hero.Anim.SetAllAnimationsFalse();
        hero.Anim.SetMoveAnimation(true);
        

        float heroDistanceToFloor = transform.position.y - targetPosition.y;
        targetPosition.y += heroDistanceToFloor;

        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            while(isPaused) yield return null;

            Vector3 direction = targetPosition - transform.position;
            Vector3 movement = direction.normalized * moveSpeed * Time.deltaTime;

            characterController.Move(movement);

            hero.RotatPart.rotation = Quaternion.Slerp(hero.RotatPart.rotation, Quaternion.LookRotation(direction.normalized),
                turnSpeed * Time.deltaTime);

            yield return null;
        }

        IsOnLockedPosition = true;
        IsOnPreviosPosition = true;
        PreviousPosition = NewPosition;
        LockedPosition = NewPosition;
        isMoves = false;
        hero.SetCanAttack(true);
        hero.Anim.SetMoveAnimation(false);
    }

    private IEnumerator MoveToTargetedMonster(BaseHero hero, Vector3 targetPosition, float moveSpeed, float turnSpeed)
    {
        IsOnPreviosPosition = false;
        isMoves = true;
        hero.SetCanAttack(false);
        hero.Anim.SetAllAnimationsFalse();
        hero.Anim.SetMoveAnimation(true);


        float heroDistanceToFloor = transform.position.y - targetPosition.y;
        targetPosition.y += heroDistanceToFloor;

        while (Vector3.Distance(transform.position, targetPosition) > (hero.AttackRange))
        {
            while (isPaused) yield return null;

            Vector3 direction = targetPosition - transform.position;
            Vector3 movement = direction.normalized * moveSpeed * Time.deltaTime;

            characterController.Move(movement);

            hero.RotatPart.rotation = Quaternion.Slerp(hero.RotatPart.rotation, Quaternion.LookRotation(direction.normalized),
                turnSpeed * Time.deltaTime);

            yield return null;
        }

        NewPosition = targetPosition;
        isMoves = false;
        hero.SetCanAttack(true);
        hero.Anim.SetMoveAnimation(false);
    }

    private IEnumerator GoToPreviousPosition(BaseHero hero, Vector3 targetPosition, float moveSpeed, float turnSpeed)
    {
        isMoves = true;
        hero.SetCanAttack(false);
        hero.Anim.SetAllAnimationsFalse();
        hero.Anim.SetMoveAnimation(true);


        float heroDistanceToFloor = transform.position.y - targetPosition.y;
        targetPosition.y += heroDistanceToFloor;

        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            while (isPaused) yield return null;

            Vector3 direction = targetPosition - transform.position;
            Vector3 movement = direction.normalized * moveSpeed * Time.deltaTime;

            characterController.Move(movement);

            hero.RotatPart.rotation = Quaternion.Slerp(hero.RotatPart.rotation, Quaternion.LookRotation(direction.normalized),
                turnSpeed * Time.deltaTime);

            yield return null;
        }

        IsOnLockedPosition = true;
        IsOnPreviosPosition = true;
        NewPosition = targetPosition;
        isMoves = false;
        hero.SetCanAttack(true);
        hero.Anim.SetMoveAnimation(false);
    }
}
