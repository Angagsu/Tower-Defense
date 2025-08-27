using System.Collections;
using UnityEngine;

public class DefenderMovement : BaseMovement
{
    public Vector3 PreviousPosition { get; private set; }
    public Vector3 DirectionOffset;
    public bool IsOnPreviosPosition;

    [SerializeField] private BaseHero defender;

    private Coroutine coroutine;
    private CharacterController characterController;

    private Vector3 newPosition;
    private Vector3 positionOffset;
    private Vector3 targetPositionOffset;
    private float distance;

    private bool isPaused => defender.IsPaused;



    private void Awake()
    {
        isMoves = false;

        characterController = GetComponent<CharacterController>();

        PreviousPosition = transform.position;
        IsOnPreviosPosition = true;
    }

    public void MoveToTarget(float moveSpeed, float turnSpeed, Vector3 touchPosition, Transform towerTransform)
    {

        if (!IsOnPreviosPosition)
        {
            return;
        }

        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        coroutine = StartCoroutine(HeroMoveTowerds(touchPosition, moveSpeed, turnSpeed, towerTransform));
    }

    public void MoveToTargetMonster(float moveSpeed, float turnSpeed, Vector3 targetPosition)
    {
        if (isMoves)
        {
            return;
        }

        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        coroutine = StartCoroutine(MoveToTargetedMonster(targetPosition, moveSpeed, turnSpeed));
    }

    public void ReturnToPreviousPosition(float moveSpeed, float turnSpeed, Vector3 targetPosition)
    {
        if (IsOnPreviosPosition)
        {
            Debug.Log("Yaqu Arraaa");
            return;
        }

        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        coroutine = StartCoroutine(GoToPreviousPosition(targetPosition, moveSpeed, turnSpeed));
    }

    private IEnumerator HeroMoveTowerds(Vector3 targetPosition, float moveSpeed, float turnSpeed, Transform towerTransform)
    {
        IsOnPreviosPosition = false;
        isMoves = true;
        defender.Deselect();
        defender.SetCanAttack(false);
        defender.Anim.SetMoveAnimation(true);

        float heroDistanceToFloor = transform.position.y - targetPosition.y;
        targetPosition.y += heroDistanceToFloor;

        positionOffset = new Vector3(transform.position.x - DirectionOffset.x, transform.position.y - DirectionOffset.y, transform.position.z - DirectionOffset.z);
        targetPositionOffset = new Vector3(DirectionOffset.x + targetPosition.x, DirectionOffset.y + targetPosition.y, DirectionOffset.z + targetPosition.z);


        while (Vector3.Distance(positionOffset, targetPositionOffset) > 0.1f)
        {
            while (isPaused) yield return null;

            Vector3 direction = targetPositionOffset - positionOffset;
            

            Vector3 movement = direction.normalized * moveSpeed * Time.deltaTime;
            
            characterController.Move(movement);

            defender.RotatPart.rotation = Quaternion.Slerp(defender.RotatPart.rotation, Quaternion.LookRotation(direction.normalized),
                turnSpeed * Time.deltaTime);

            
            positionOffset = new Vector3(transform.position.x - DirectionOffset.x, transform.position.y - DirectionOffset.y, transform.position.z - DirectionOffset.z);
            distance = Vector3.Distance(positionOffset, targetPositionOffset);
 
            yield return null;
        }

        positionOffset = Vector3.zero;
        targetPositionOffset = Vector3.zero;
        
        IsOnPreviosPosition = true;

        newPosition = transform.position;
        PreviousPosition = newPosition;
        
        isMoves = false;
        defender.SetCanAttack(true);
        defender.Anim.SetMoveAnimation(false);
    }

    private IEnumerator MoveToTargetedMonster(Vector3 targetPosition, float moveSpeed, float turnSpeed)
    {
        IsOnPreviosPosition = false;
        isMoves = true;
        defender.SetCanAttack(false);
        defender.Anim.SetMoveAnimation(true);


        float heroDistanceToFloor = transform.position.y - targetPosition.y;
        targetPosition.y += heroDistanceToFloor;

        while (Vector3.Distance(transform.position, targetPosition) > defender.AttackRange)
        {
            while(isPaused) yield return null;

            Vector3 direction = targetPosition - transform.position;
            Vector3 movement = direction.normalized * moveSpeed * Time.deltaTime;

            characterController.Move(movement);

            defender.RotatPart.rotation = Quaternion.Slerp(defender.RotatPart.rotation, Quaternion.LookRotation(direction.normalized),
                turnSpeed * Time.deltaTime);

            yield return null;
        }

        newPosition = targetPosition;
        isMoves = false;
        defender.SetCanAttack(true);
        defender.Anim.SetMoveAnimation(false);
    }

    private IEnumerator GoToPreviousPosition(Vector3 targetPosition, float moveSpeed, float turnSpeed)
    {
        isMoves = true;
        defender.SetCanAttack(false);
        defender.Anim.SetMoveAnimation(true);


        float heroDistanceToFloor = transform.position.y - targetPosition.y;
        targetPosition.y += heroDistanceToFloor;

        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            while (isPaused) yield return null;

            Vector3 direction = targetPosition - transform.position;
            Vector3 movement = direction.normalized * moveSpeed * Time.deltaTime;

            characterController.Move(movement);

            defender.RotatPart.rotation = Quaternion.Slerp(defender.RotatPart.rotation, Quaternion.LookRotation(direction.normalized),
                turnSpeed * Time.deltaTime);

            yield return null;
        }

        IsOnPreviosPosition = true;
        newPosition = targetPosition;
        isMoves = false;
        defender.SetCanAttack(true);
        defender.Anim.SetMoveAnimation(false);
    }
}
