using System.Collections;
using UnityEngine;


public class HeroMovement : BaseMovement
{
    private Camera mainCamera;
    private int groundLayer;
    private Coroutine coroutine;
    private CharacterController characterController;
    private Vector3 heroNewPosition;


    private void Awake()
    {
        isMoves = false;
        mainCamera = Camera.main;
        characterController = GetComponent<CharacterController>();
        groundLayer = LayerMask.NameToLayer("Ground");
    }

    public void MoveToTarget(BaseHero hero, float moveSpeed, float turnSpeed, Vector2 touchPosition)
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
                coroutine = StartCoroutine(HeroMoveTowerds(hero, raycastHit.point, moveSpeed, turnSpeed));
                heroNewPosition = raycastHit.point;
            }
        }
    }

    public IEnumerator HeroMoveTowerds(BaseHero hero, Vector3 targetPosition, float moveSpeed, float turnSpeed)
    {
        isMoves = true;
        hero.Deselect();
        hero.Anim.SetAllAnimationsFalse();
        hero.Anim.SetMoveAnimation(true);
        

        float heroDistanceToFloor = transform.position.y - targetPosition.y;
        targetPosition.y += heroDistanceToFloor;

        while (Vector3.Distance(transform.position, targetPosition) > 0.5f)
        {
            Vector3 direction = targetPosition - transform.position;
            Vector3 movement = direction.normalized * moveSpeed * Time.deltaTime;

            characterController.Move(movement);

            hero.RotatPart.rotation = Quaternion.Slerp(hero.RotatPart.rotation, Quaternion.LookRotation(direction.normalized),
                turnSpeed * Time.deltaTime);

            yield return null;
        }

        isMoves = false;
        hero.Anim.SetMoveAnimation(false);
    }
}
