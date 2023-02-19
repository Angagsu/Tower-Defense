using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections;

public class TowerBuildManager : MonoBehaviour
{
    public static TowerBuildManager Instance;

    
    private GroundBehavior selectedGround;
    [SerializeField] private TowersBuildUI towersBuildUI;
    [SerializeField] private TowerUpgradeUI towerUpgradeUI;
    private Hero archerHero, knightHero;
    
    [HideInInspector]
    public TowerBlueprint towerToBuild;

    [HideInInspector] public bool SelectMissileLauncherTower = false;
    [HideInInspector] public bool SelectStandardTower = false;
    [HideInInspector] public bool SelectLaserTower = false;

    public bool CanBuild { get { return towerToBuild != null; } }
    public bool HasManey { get { return PlayerStats.Money >= towerToBuild.Cost; } }

    private Camera mainCamera;
    private int groundLayer;

    private void Awake()
    {
        
        if (Instance != null)
        {
            Debug.LogError("More than one TowerBuilManager in scene!");
            return;
        }

        Instance = this;

        towerUpgradeUI = GameObject.Find("TowerUpgradeUI").GetComponent<TowerUpgradeUI>();
        mainCamera = Camera.main;
        groundLayer = LayerMask.NameToLayer("Ground");
        archerHero = GameObject.FindGameObjectWithTag("ArcherHero").GetComponent<Hero>();
        knightHero = GameObject.FindGameObjectWithTag("KnightHero").GetComponent<Hero>();
        
    }

    private void Update()
    {
        DeselectAllUIAndHeroesThenClickGround();
    }

    public void SelectedGroundForUpgradeTowerUI(GroundBehavior groundBehavior)
    {
        if (selectedGround == groundBehavior)
        {
            DeselectGround();
            return;
        }
        knightHero.DeselectHeroes();
        archerHero.DeselectHeroes();
        towersBuildUI.towerBuildUI.SetActive(!towersBuildUI.towerBuildUI.activeSelf);

        if (towersBuildUI.towerBuildUI.activeSelf)
        {
            towersBuildUI.towerBuildUI.SetActive(false);
        }

        selectedGround = groundBehavior;
        towerToBuild = null;

        if (selectedGround.defendersMovement == null)
        {
            //towerUpgradeUI.selectDefendersButton.interactable = false;
            //towerUpgradeUI.selectDefendersButton.enabled = false;
            towerUpgradeUI.selectDefendersButton.SetActive(false);
        }
        else
        {
            //towerUpgradeUI.selectDefendersButton.interactable = true;
            //towerUpgradeUI.selectDefendersButton.enabled = true;
            towerUpgradeUI.selectDefendersButton.SetActive(true);
        }

        towerUpgradeUI.SetTargetGround(groundBehavior);
    }

    public void SelectedGroundForBuildTowerUI(GroundBehavior groundBehavior)
    {
        if (selectedGround == groundBehavior)
        {
            DeselectGround();
            return;
        }
        knightHero.DeselectHeroes();
        archerHero.DeselectHeroes();
        towerUpgradeUI.towerUpgradeUI.SetActive(!towerUpgradeUI.towerUpgradeUI.activeSelf);

        if (towerUpgradeUI.towerUpgradeUI.activeSelf)
        {
            towerUpgradeUI.towerUpgradeUI.SetActive(false);
        }

        selectedGround = groundBehavior;
        towerToBuild = null;
        
        towersBuildUI.SetTargetGroundForBuilding(groundBehavior);
    }

    private void DeselectAllUIAndHeroesThenClickGround()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        //if (Input.GetMouseButtonDown(0) && !IsMouseOverUI())
        //{
        //    if (Physics.Raycast(ray, out RaycastHit raycastHit) && raycastHit.collider &&
        //        raycastHit.collider.gameObject.layer.CompareTo(groundLayer) == 0)
        //    {
        //        Invoke("DeselectGround", 0.1f);
        //        archerHero.DeselectHeroes();
        //        knightHero.DeselectHeroes();
        //        towerUpgradeUI.DeActivateDefendersMoveZone();
        //    }
        //}

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (!IsMouseOverUI())
            {
                if (Physics.Raycast(ray, out RaycastHit raycastHit) && raycastHit.collider.gameObject.layer.CompareTo(groundLayer) == 0)
                {
                    DeselectGround();
                    archerHero.DeselectHeroes();
                    knightHero.DeselectHeroes();
                    towerUpgradeUI.DeActivateDefendersMoveZone();
                }
            }
        }
    }
    
    public void DeselectGround()
    {
        selectedGround = null;
        towerUpgradeUI.HideCanvas();
        towersBuildUI.HideCanvas();
    }
    public void SelectTowerToBuild(TowerBlueprint tower)
    {
        towerToBuild = tower;
        DeselectGround();
    }

    public TowerBlueprint GetTowerToBuild()
    {
        return towerToBuild;
    }

    private bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
    }
    
}
