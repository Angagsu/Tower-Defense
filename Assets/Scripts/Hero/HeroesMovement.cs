using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
//[RequireComponent(typeof(Rigidbody))]
public class HeroesMovement : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    private Vector3 heroNewPosition;
    
    
    [SerializeField] private float heroSpeed = 40f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] public Transform heroRotatPart;

    private Coroutine coroutine;
    private CharacterController characterController;
    private Hero hero;
    //private Rigidbody rb;
    private int groundLayer;
    
    public bool isHeroSelected;
    public bool isHeroStoppedMove;

    private void Awake()
    {
        
    }
    private void Start()
    {
        
        isHeroStoppedMove = true;
        isHeroSelected = false;
        characterController = GetComponent<CharacterController>();
        hero = GetComponent<Hero>();
        mainCamera = Camera.main;
        groundLayer = LayerMask.NameToLayer("Ground");
        
    }

    private void Update()
    {
        GetHeroNewPosition();
    }

    private void GetHeroNewPosition()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        
        if (isHeroSelected && !hero.isHeroDead)
        {
            if (Input.GetMouseButtonDown(0) && !IsMouseOverUI())
            {
                if (Physics.Raycast(ray, out RaycastHit raycastHit) && raycastHit.collider &&
                    raycastHit.collider.gameObject.layer.CompareTo(groundLayer) == 0)
                {
                    if (coroutine != null)
                    {
                        StopCoroutine(coroutine);
                    }
                    coroutine = StartCoroutine(HeroMoveTowerds(raycastHit.point));
                    heroNewPosition = raycastHit.point;
                    
                }
            }
        }
    }

    public IEnumerator HeroMoveTowerds(Vector3 target)
    {
        isHeroStoppedMove = false;
        isHeroSelected = false;
        float heroDistanceToFloor = transform.position.y - target.y;
        target.y += heroDistanceToFloor;
        while (Vector3.Distance(transform.position, target) > 0.5f)
        {
            Vector3 direction = target - transform.position;
            Vector3 movement = direction.normalized * heroSpeed * Time.deltaTime;

            characterController.Move(movement);

            heroRotatPart.rotation = Quaternion.Slerp(heroRotatPart.rotation, Quaternion.LookRotation(direction.normalized),
                rotationSpeed * Time.deltaTime);

            yield return null;
        }

        isHeroStoppedMove = true;
    }

    private bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(heroNewPosition, 1f);
    }
}
