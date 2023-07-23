using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DefendersMovement : MonoBehaviour
{
    public bool isDefendersStoppedMove;
    public bool isDefendersSelected;

    private Vector3 defendersNewPoint;
    private SworderDefender[] sworderDefender;
    
    private Coroutine coroutine;
    
    private int groundLayer;

    private TowerUpgradeUI towerUpgradeUI;

    [SerializeField] private GameObject[] sworderDefendersObj;
    [SerializeField] private Transform towerTransform;
    [SerializeField] private float defendersSpeed;
    [SerializeField] private float timeOfRevive;
    private Camera mainCamera;
    private TowerOnBuy groundStartPoint;

    Animator[] defendersAnimators = new Animator[3];
    private int isStopedMoveHash;

    private void Awake()
    {
        mainCamera = Camera.main;
        groundStartPoint = GameObject.Find("GameManager").GetComponent<TowerOnBuy>();
        towerUpgradeUI = GameObject.Find("TowerUpgradeUI").GetComponent<TowerUpgradeUI>();
        sworderDefender = new SworderDefender[3];
        
        groundLayer = LayerMask.NameToLayer("DefendersMoveZone");

        isStopedMoveHash = Animator.StringToHash("isStopedMove");
        
    }
    void Start()
    {
        isDefendersStoppedMove = true;
        isDefendersSelected = false;
        defendersNewPoint = groundStartPoint.GroundBehavior.defendersStartPoint.position;
        SetTheSworderDefenderArray();
        StartCoroutine(DefendersMoveTowerds(defendersNewPoint));
        
    }

    
    void Update()
    {
        DisableTheDefenderWhenDies();
        GetDefendersNewPosition();
    }

    private void GetDefendersNewPosition()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (isDefendersSelected )
        {
            if (Input.GetMouseButtonDown(0) && !IsMouseOverUI())
            {
                if (Physics.Raycast(ray, out RaycastHit raycastHit) && 
                    raycastHit.collider.gameObject.layer.CompareTo(groundLayer) == 0)
                {
                    if (coroutine != null)
                    {
                        StopCoroutine(coroutine);
                    }
                    isDefendersSelected = false;
                    coroutine = StartCoroutine(DefendersMoveTowerds(raycastHit.point));
                    defendersNewPoint = raycastHit.point;
                }
                else
                {
                    if (coroutine != null)
                    {
                        StopCoroutine(coroutine);
                    }
                    
                    isDefendersSelected = false;
                    isDefendersStoppedMove = true;
                    towerUpgradeUI.DeActivateDefendersMoveZone();
                }
            }
        }
        
    }

    private void SetTheSworderDefenderArray()
    {
        for (int i = 0; i < sworderDefendersObj.Length; i++)
        {
            sworderDefender[i] = sworderDefendersObj[i].GetComponent<SworderDefender>();
            defendersAnimators[i] = sworderDefendersObj[i].GetComponentInChildren<Animator>();
            defendersAnimators[i].SetBool(isStopedMoveHash, false);
        }
    }
    private void DisableTheDefenderWhenDies()
    {
        for (int i = 0; i < sworderDefendersObj.Length; i++)
        {
            
            if (sworderDefender[i].IsDefenderDead)
            {
                sworderDefendersObj[i].SetActive(false);
                StartCoroutine(TimerForDefenderRevive(sworderDefender[i]));
            }
        }
        
    }
    public IEnumerator DefendersMoveTowerds(Vector3 target)
    {
        isDefendersStoppedMove = false;

        towerUpgradeUI.DeActivateDefendersMoveZone();

        float heroDistanceToFloor = transform.position.y - target.y;
        target.y += heroDistanceToFloor;
        
        while (Vector3.Distance(transform.position, target) > 0.5f && Vector3.Distance(towerTransform.position, target) < 12f && !isDefendersStoppedMove )
        {
            //Vector3 direction = target - transform.position;
            //Vector3 movement = direction.normalized * defendersSpeed * Time.deltaTime;
            
            //characterController.Move(movement);
            Vector3 destination = Vector3.MoveTowards(transform.position, target, defendersSpeed * Time.deltaTime);
            transform.position = destination;
            for (int i = 0; i < sworderDefender.Length; i++)
            {
                sworderDefender[i].defenderRotatPart.rotation = Quaternion.Slerp(sworderDefender[i].defenderRotatPart.rotation, Quaternion.LookRotation(destination.normalized),
                sworderDefender[i].turnSpeed * Time.deltaTime);
            }

            yield return null;
        }

        isDefendersStoppedMove = true;
        for (int i = 0; i < defendersAnimators.Length; i++)
        {
            defendersAnimators[i].SetBool(isStopedMoveHash, true);
        }
    }

    public void DeSelectDefenders()
    {
        isDefendersSelected = false;
    }

    private IEnumerator TimerForDefenderRevive(SworderDefender defender)
    {
         while (defender.IsDefenderDead)
         {
             yield return new WaitForSeconds(timeOfRevive);
             defender.ReviveDefender();
             defender.gameObject.SetActive(true);
         } 
    } 

    private bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
    }
}
