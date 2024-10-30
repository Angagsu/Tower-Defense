using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

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

    public void MoveToTarget(BaseHero hero, float moveSpeed, float turnSpeed)
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (hero.IsSelected && !hero.IsDead)
        {
            //Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began
            if (Input.GetMouseButtonDown(0))
            {
                if (!IsMouseOverUI())
                {
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

    private bool IsMouseOverUI()
    {
        //return EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
        return EventSystem.current.IsPointerOverGameObject();
    }
}
