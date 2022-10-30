using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DefendersMovement : MonoBehaviour
{
    private bool isDefendersStoppedMove;
    public bool isDefendersSelected;

    private Vector3 defendersNewPoint;
    private SworderDefender[] sworderDefender;
    
    private Coroutine coroutine;
    private CharacterController characterController;
    private int groundLayer;

    private TowerUpgradeUI towerUpgradeUI;

    [SerializeField] private GameObject[] sworderDefendersObj;
    [SerializeField] private Transform towerTransform;
    [SerializeField] private float defendersSpeed;
    private Camera mainCamera;
    private TowerOnBuy groundStartPoint;

    

    private void Awake()
    {
        mainCamera = Camera.main;
        groundStartPoint = GameObject.Find("GameManager").GetComponent<TowerOnBuy>();
        towerUpgradeUI = GameObject.Find("TowerUpgradeUI").GetComponent<TowerUpgradeUI>();
        characterController = GetComponent<CharacterController>();
        sworderDefender = new SworderDefender[3];
        //sworderDefender = GetComponentsInChildren<SworderDefender>();
        
        groundLayer = LayerMask.NameToLayer("DefendersMoveZone");
    }
    void Start()
    {
        isDefendersStoppedMove = true;
        isDefendersSelected = false;
        defendersNewPoint = groundStartPoint.groundBehavior.defendersStartPoint.position;
        ReviveOrMakeADeadTheDefender();
        StartCoroutine(DefendersMoveTowerds(defendersNewPoint));
    }

    
    void Update()
    {
        ReviveOrMakeADeadTheDefender();
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

    private void ReviveOrMakeADeadTheDefender()
    {
        for (int i = 0; i < sworderDefendersObj.Length; i++)
        {
            sworderDefender[i] = sworderDefendersObj[i].GetComponent<SworderDefender>();
            if (sworderDefender[i].isDefenderDead)
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
        
    }

    public void DeSelectDefenders()
    {
        isDefendersSelected = false;
    }

    public IEnumerator TimerForDefenderRevive(SworderDefender defender)
    {
         while (defender.isDefenderDead)
         {
             yield return new WaitForSeconds(8);
             defender.ReviveDefender();
             defender.gameObject.SetActive(true);
         } 
    } 

    private bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
