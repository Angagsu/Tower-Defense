using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


public class DefenderMovement : BaseMovement
{
    [SerializeField] private GameObject[] defendersObj;
    [SerializeField] private Transform towerTransform;
    [SerializeField] private float defendersSpeed;
    [SerializeField] private float timeOfRevive;
    [SerializeField] private float turnSpeed;

    private TouchBuildingArea defendersStartPoint;
    private TowerUpgradeUI towerUpgradeUI;
    private DetectionHelper detectionHelper;
    private DefenderUnit[] defenderUnits;

    private Camera mainCamera;
    private Coroutine coroutine;
    private Vector3 defendersNewPoint;
    private int groundLayer;
    private bool isSelected;

    private PlayerInputHandler playerInputHandler;

    private void Awake()
    {
        playerInputHandler = PlayerInputHandler.Instance;
        mainCamera = Camera.main;
        defendersStartPoint = GameObject.Find("GameManager").GetComponent<TouchBuildingArea>();
        towerUpgradeUI = GameObject.Find("TowerUpgradeUI").GetComponent<TowerUpgradeUI>();
        defenderUnits = new DefenderUnit[3];

        groundLayer = LayerMask.NameToLayer("DefendersMoveZone");
        detectionHelper = DetectionHelper.Instance;   
    }
    void Start()
    {
        isMoves = true;
        isSelected = false;
        defendersNewPoint = defendersStartPoint.BuildingArea.GetDefendersStartPoint().position;
        SetTheSworderDefenderArray();
        StartCoroutine(DefendersMoveTowerds(defendersNewPoint));
    }

    private void OnEnable()
    {
        playerInputHandler.TouchPressed += GetDefendersNewPosition;
    }

    private void OnDisable()
    {
        playerInputHandler.TouchPressed -= GetDefendersNewPosition;
    }

    void Update()
    {
        DisableTheDefenderWhenDies();
    }

    private void GetDefendersNewPosition(Vector2 touchPosition)
    {
        if (isSelected)
        {
            if (true)
            {
                Ray ray = mainCamera.ScreenPointToRay(touchPosition);

                if (Physics.Raycast(ray, out RaycastHit raycastHit) &&
                    raycastHit.collider.gameObject.layer.CompareTo(groundLayer) == 0)
                {
                    if (coroutine != null)
                    {
                        StopCoroutine(coroutine);
                    }
                    isSelected = false;
                    coroutine = StartCoroutine(DefendersMoveTowerds(raycastHit.point));
                    defendersNewPoint = raycastHit.point;
                }
                else
                {
                    if (coroutine != null)
                    {
                        StopCoroutine(coroutine);
                    }

                    isSelected = false;
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
            detectionHelper.OnHeroesCountIncreased(defenderUnits[i]);
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
                Vector3 direction = targetPosition - transform.position;

                defenderUnits[i].Anim.SetMoveAnimation(true);
                defenderUnits[i].RotatPart.rotation = Quaternion.Slerp(defenderUnits[i].RotatPart.rotation,
                    Quaternion.LookRotation(direction.normalized), turnSpeed);
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
        isSelected = tof;
    }

    private IEnumerator TimerForDefenderRevive(DefenderUnit defender)
    {
        while (defender.IsDead)
        {
            yield return new WaitForSeconds(timeOfRevive);
            defender.OnRevive();
            defender.enabled = true;
            defender.gameObject.SetActive(true);
        }
    }

    public DefenderUnit[] GetDefendersArray()
    {
        return defenderUnits;
    }
}


