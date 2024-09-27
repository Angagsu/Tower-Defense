using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class DefenderMovement : BaseMovement
{
    public bool IsSelected { get; private set; }

    private Vector3 defendersNewPoint;
    private DefenderUnit[] defenderUnits;

    private Coroutine coroutine;

    private int groundLayer;

    private TowerUpgradeUI towerUpgradeUI;

    [SerializeField] private GameObject[] defendersObj;
    [SerializeField] private Transform towerTransform;
    [SerializeField] private float defendersSpeed;
    [SerializeField] private float timeOfRevive;
    [SerializeField] private float turnSpeed;
    private Camera mainCamera;
    private TowerOnBuy groundStartPoint;

    
 

    private void Awake()
    {
        mainCamera = Camera.main;
        groundStartPoint = GameObject.Find("GameManager").GetComponent<TowerOnBuy>();
        towerUpgradeUI = GameObject.Find("TowerUpgradeUI").GetComponent<TowerUpgradeUI>();
        defenderUnits = new DefenderUnit[3];

        groundLayer = LayerMask.NameToLayer("DefendersMoveZone");

    }
    void Start()
    {
        isMoves = true;
        IsSelected = false;
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

        if (IsSelected)
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
                    IsSelected = false;
                    coroutine = StartCoroutine(DefendersMoveTowerds(raycastHit.point));
                    defendersNewPoint = raycastHit.point;
                }
                else
                {
                    if (coroutine != null)
                    {
                        StopCoroutine(coroutine);
                    }

                    IsSelected = false;
                    isMoves = false;
                    towerUpgradeUI.DeActivateDefendersMoveZone();
                }
            }
        }

    }

    private void SetTheSworderDefenderArray()
    {
        for (int i = 0; i < defendersObj.Length; i++)
        {
            defenderUnits[i] = defendersObj[i].GetComponent<DefenderUnit>(); 
        }
    }
    private void DisableTheDefenderWhenDies()
    {
        for (int i = 0; i < defendersObj.Length; i++)
        {
            if (defenderUnits[i].IsDead)
            {
                defenderUnits[i].enabled = false;
                defenderUnits[i].Anim.SetDeadAnimation(true);
                //sworderDefendersObj[i].SetActive(false);
                StartCoroutine(TimerForDefenderRevive(defenderUnits[i]));
            }
        }

    }
    public IEnumerator DefendersMoveTowerds(Vector3 targetPosition)
    {
        isMoves = true;

        towerUpgradeUI.DeActivateDefendersMoveZone();

        float heroDistanceToFloor = transform.position.y - targetPosition.y;
        targetPosition.y += heroDistanceToFloor;

        while (Vector3.Distance(transform.position, targetPosition) > 0.5f && Vector3.Distance(towerTransform.position, targetPosition) < 12f && IsMoves)
        {
            Vector3 destination = Vector3.MoveTowards(transform.position, targetPosition, defendersSpeed * Time.deltaTime);
            transform.position = destination;
            for (int i = 0; i < defenderUnits.Length; i++)
            {
                defenderUnits[i].Anim.SetMoveAnimation(true);
                defenderUnits[i].RotatPart.rotation = Quaternion.Slerp(defenderUnits[i].RotatPart.rotation, Quaternion.LookRotation(destination.normalized),
                turnSpeed * Time.deltaTime);
            }

            yield return null;
        }

        isMoves = false;
        for (int i = 0; i < defenderUnits.Length; i++)
        {
            defenderUnits[i].Anim.SetMoveAnimation(false);
        }
    }

    public void SelectOrDeSelectDefenders(bool tof)
    {
        IsSelected = tof;
    }

    private IEnumerator TimerForDefenderRevive(DefenderUnit defender)
    {
        while (defender.IsDead)
        {
            yield return new WaitForSeconds(timeOfRevive);
            defender.Revive();
            defender.enabled = true;
            defender.gameObject.SetActive(true);
        }
    }

    private bool IsMouseOverUI()
    {
        //return EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
        return EventSystem.current.IsPointerOverGameObject();
    }
}


